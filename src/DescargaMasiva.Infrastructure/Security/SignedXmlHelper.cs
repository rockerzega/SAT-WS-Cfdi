using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Xml;
using DescargaMasiva.DescargaMasiva.Domain.Constants;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Security;

public static class SignedXmlHelper
{
    internal static AsymmetricAlgorithm GetSigningKeyForRsaSha1XmlDsig(X509Certificate2 certificate)
    {
        var rsa = certificate.GetRSAPrivateKey();
        if (rsa is null)
            throw new CryptographicException("La clave privada RSA del certificado no está disponible.");

        return new RsaSha1Pkcs1BouncyCastle(rsa);
    }

    #region agent log
    private static void WriteAgentNdjson(string hypothesisId, string location, string message,
        IReadOnlyDictionary<string, object?> data)
    {
        const string logPath = "/home/luis/personal/csharp/SAT-WS-Cfdi/.cursor/debug-92c31b.log";
        try
        {
            var payload = new Dictionary<string, object?>
            {
                ["sessionId"] = "92c31b",
                ["runId"] = Environment.GetEnvironmentVariable("DEBUG_RUN_ID") ?? "run1",
                ["hypothesisId"] = hypothesisId,
                ["location"] = location,
                ["message"] = message,
                ["data"] = data,
                ["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            File.AppendAllText(logPath,
                JsonSerializer.Serialize(payload) + Environment.NewLine);
        }
        catch { /* debug ingest */ }
    }
    #endregion

    /// <summary>
    ///     This method is used to sign all requests like solicitud, verificacion and descarga.
    ///     For autenticacion use the other method
    /// </summary>
    public static XmlElement SignRequest(XmlElement xmlElement, X509Certificate2 x509Certificate2)
    {
        var signedXml = new SignedXml(xmlElement) { SigningKey = GetSigningKeyForRsaSha1XmlDsig(x509Certificate2) };
        signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;

        var reference = new Reference { Uri = "", DigestMethod = SignedXml.XmlDsigSHA1Url };
        reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
        signedXml.AddReference(reference);

        var keyInfoX509Data = new KeyInfoX509Data(x509Certificate2);
        keyInfoX509Data.AddIssuerSerial(x509Certificate2.Issuer, x509Certificate2.SerialNumber);

        var keyInfo = new KeyInfo();
        keyInfo.AddClause(keyInfoX509Data);
        signedXml.KeyInfo = keyInfo;

        signedXml.ComputeSignature();

        return signedXml.GetXml();
    }

    /// <summary>
    ///     This method is only used to sign the autenticacion service
    /// </summary>
    public static XmlElement SignAuthenticationRequest(XmlElement xmlElement,
                                                       X509Certificate2 x509Certificate2,
                                                       string referenceUri,
                                                       XmlElement securityTokenReferenceElement)
    {
        #region agent log
        var rsaKeyProbe = x509Certificate2.GetRSAPrivateKey();
        string signHashProbe;
        if (rsaKeyProbe is null)
            signHashProbe = "skipped_rsa_null";
        else
            try
            {
                rsaKeyProbe.SignHash(new byte[20], HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                signHashProbe = "ok";
            }
            catch (Exception ex)
            {
                signHashProbe = $"{ex.GetType().Name}: {ex.Message}";
            }

        WriteAgentNdjson("H1,H2,H4,H5", "SignedXmlHelper.SignAuthenticationRequest:entry",
            "cert key + direct SignHash(SHA1,PKCS1) probe",
            new Dictionary<string, object?>
            {
                ["keyAlgorithmOid"] = x509Certificate2.GetKeyAlgorithm(),
                ["publicKeyOid"] = x509Certificate2.PublicKey.Oid.Value,
                ["rsaKeyNull"] = rsaKeyProbe == null,
                ["rsaKeySize"] = rsaKeyProbe?.KeySize,
                ["signatureMethod"] = SignedXml.XmlDsigRSASHA1Url,
                ["digestMethod"] = SignedXml.XmlDsigSHA1Url,
                ["referenceUri"] = referenceUri,
                ["signHashSha1Pkcs1Probe"] = signHashProbe,
                ["osPlatform"] = System.Runtime.InteropServices.RuntimeInformation.OSDescription
            });
        #endregion

        var signedXml = new SignedXmlWithId(xmlElement) { SigningKey = GetSigningKeyForRsaSha1XmlDsig(x509Certificate2) };
        signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;
        signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

        var reference = new Reference { Uri = referenceUri, DigestMethod = SignedXml.XmlDsigSHA1Url };
        reference.AddTransform(new XmlDsigExcC14NTransform());
        signedXml.AddReference(reference);

        var keyInfo = new KeyInfo();
        var keyInfoNode = new KeyInfoNode { Value = securityTokenReferenceElement };
        keyInfo.AddClause(keyInfoNode);
        signedXml.KeyInfo = keyInfo;

        signedXml.ComputeSignature();

        #region agent log
        WriteAgentNdjson("verify", "SignedXmlHelper.SignAuthenticationRequest:afterCompute",
            "ComputeSignature completed without exception",
            new Dictionary<string, object?> { ["signingKeyWrapper"] = nameof(RsaSha1Pkcs1BouncyCastle) });
        #endregion

        return signedXml.GetXml();
    }

    /// <summary>
    ///     Custom SignedXml class to be able to work with soap security Ids because the original implementation will not find
    ///     them.
    ///     This class is only used in the authenticacion service
    /// </summary>
    internal sealed class SignedXmlWithId : SignedXml
    {
        public SignedXmlWithId(XmlDocument xml) : base(xml)
        {
        }

        public SignedXmlWithId(XmlElement xmlElement) : base(xmlElement)
        {
        }

        public override XmlElement GetIdElement(XmlDocument doc, string id)
        {
            // check to see if it's a standard ID reference
            XmlElement idElem = base.GetIdElement(doc, id);
            var baseFound = idElem != null;

            if (idElem is null)
            {
                var nsManager = new XmlNamespaceManager(doc.NameTable);
                nsManager.AddNamespace(CfdiDescargaMasivaNamespaces.UPrefix, CfdiDescargaMasivaNamespaces.UNamespaceUrl);

                idElem = doc.SelectSingleNode("//*[@u:Id=\"" + id + "\"]", nsManager) as XmlElement;
            }

            #region agent log
            WriteAgentNdjson("H3", "SignedXmlHelper.SignedXmlWithId.GetIdElement", "reference id resolution",
                new Dictionary<string, object?>
                {
                    ["id"] = id,
                    ["baseGetIdFound"] = baseFound,
                    ["finalFound"] = idElem != null,
                    ["finalName"] = idElem is null ? null : $"{idElem.Prefix}:{idElem.LocalName}"
                });
            #endregion

            return idElem;
        }
    }
}