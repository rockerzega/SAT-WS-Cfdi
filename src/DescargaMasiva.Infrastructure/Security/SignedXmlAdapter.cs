using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Security;

public class SignedXmlAdapter : IXmlRequestSigner
{
  public XmlElement SignRequest(XmlElement xml, byte[] pfx, string password)
  {
    using var cert = new X509Certificate2(pfx, password);

    var signedXml = new SignedXml(xml)
    {
      SigningKey = SignedXmlHelper.GetSigningKeyForRsaSha1XmlDsig(cert)
    };

    signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;

    var reference = new Reference
    {
      Uri = "",
      DigestMethod = SignedXml.XmlDsigSHA1Url
    };

    reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
    signedXml.AddReference(reference);

    var keyInfoX509Data = new KeyInfoX509Data(cert);
    keyInfoX509Data.AddIssuerSerial(cert.Issuer, cert.SerialNumber);

    var keyInfo = new KeyInfo();
    keyInfo.AddClause(keyInfoX509Data);
    signedXml.KeyInfo = keyInfo;

    signedXml.ComputeSignature();
    return signedXml.GetXml();
  }

  public XmlElement SignAuthenticationRequest(
    XmlElement xml,
    byte[] pfx,
    string password,
    string referenceUri,
    XmlElement securityTokenReferenceElement)
  {
    using var cert = new X509Certificate2(pfx, password);

    var signedXml = new SignedXmlWithId(xml)
    {
      SigningKey = SignedXmlHelper.GetSigningKeyForRsaSha1XmlDsig(cert)
    };

    signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;
    signedXml.SignedInfo.CanonicalizationMethod =
      SignedXml.XmlDsigExcC14NTransformUrl;

    var reference = new Reference
    {
      Uri = referenceUri,
      DigestMethod = SignedXml.XmlDsigSHA1Url
    };

    reference.AddTransform(new XmlDsigExcC14NTransform());
    signedXml.AddReference(reference);

    var keyInfo = new KeyInfo();
    keyInfo.AddClause(new KeyInfoNode { Value = securityTokenReferenceElement });
    signedXml.KeyInfo = keyInfo;

    signedXml.ComputeSignature();
    return signedXml.GetXml();
  }
}