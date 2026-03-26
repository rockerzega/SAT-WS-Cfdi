using System.Xml;
using DescargaMasiva.DescargaMasiva.Domain.Constants;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

internal sealed class AuthSoapEnvelopeBuilder: ISoapEnvelopeBuilder<AuthRequest>
{
  private readonly IAuthRequestSigner _signer;

  public AuthSoapEnvelopeBuilder(IAuthRequestSigner signer)
  {
    _signer = signer;
  }

  public string Build(AuthRequest authRequest)
  {
    var xmlDocument = new XmlDocument();

    XmlElement envelopElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.SPrefix,
                "Envelope",
                CfdiDescargaMasivaNamespaces.SNamespaceUrl);
    envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.SPrefix}", CfdiDescargaMasivaNamespaces.SNamespaceUrl);
    envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.UPrefix}", CfdiDescargaMasivaNamespaces.UNamespaceUrl);
    xmlDocument.AppendChild(envelopElement);

    XmlElement headerElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.SPrefix,
        "Header",
        CfdiDescargaMasivaNamespaces.SNamespaceUrl);
    envelopElement.AppendChild(headerElement);

    XmlElement securityElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.OPrefix,
        "Security",
        CfdiDescargaMasivaNamespaces.ONamespaceUrl);
    securityElement.SetAttribute("mustUnderstand", CfdiDescargaMasivaNamespaces.SNamespaceUrl, "1");
    headerElement.AppendChild(securityElement);

    XmlElement timestampElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.UPrefix,
        "Timestamp",
        CfdiDescargaMasivaNamespaces.UNamespaceUrl);
    timestampElement.SetAttribute("Id", CfdiDescargaMasivaNamespaces.UNamespaceUrl, "_0"); // Probar con _0 si falla
    securityElement.AppendChild(timestampElement);

    XmlElement createdElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.UPrefix,
        "Created",
        CfdiDescargaMasivaNamespaces.UNamespaceUrl);
    createdElement.InnerText = authRequest.TokenCreatedDateUtc.ToSoapSecurityTimestampString();
    timestampElement.AppendChild(createdElement);

    XmlElement expiresElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.UPrefix,
        "Expires",
        CfdiDescargaMasivaNamespaces.UNamespaceUrl);
    expiresElement.InnerText = authRequest.TokenExpiresDateUtc.ToSoapSecurityTimestampString();
    timestampElement.AppendChild(expiresElement);

    XmlElement binarySecurityTokenElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.OPrefix,
        "BinarySecurityToken",
        CfdiDescargaMasivaNamespaces.ONamespaceUrl);
    binarySecurityTokenElement.SetAttribute("Id",
        CfdiDescargaMasivaNamespaces.UNamespaceUrl,
        authRequest.Uuid.ToBinarySecurityTokenId());
    binarySecurityTokenElement.SetAttribute("ValueType",
        $"{CfdiDescargaMasivaNamespaces.BaseUrl}x509-token-profile-1.0#X509v3");
    binarySecurityTokenElement.SetAttribute("EncodingType",
        $"{CfdiDescargaMasivaNamespaces.BaseUrl}soap-message-security-1.0#Base64Binary");
    binarySecurityTokenElement.InnerText =
      _signer.GetCertificateBase64();

    securityElement.AppendChild(binarySecurityTokenElement);

    XmlElement securityTokenReferenceElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.OPrefix,
        "SecurityTokenReference",
        CfdiDescargaMasivaNamespaces.ONamespaceUrl);
    XmlElement securityTokenReferenceReferenceElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.OPrefix,
        "Reference",
        CfdiDescargaMasivaNamespaces.ONamespaceUrl);
    XmlAttribute valueType = xmlDocument.CreateAttribute("ValueType");
    valueType.Value = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3";
    securityTokenReferenceReferenceElement.Attributes.Append(valueType);
    XmlAttribute encodingType = xmlDocument.CreateAttribute("URI");
    encodingType.Value = $"#{authRequest.Uuid.ToBinarySecurityTokenId()}";
    securityTokenReferenceReferenceElement.Attributes.Append(encodingType);
    securityTokenReferenceElement.AppendChild(securityTokenReferenceReferenceElement);

    XmlElement signatureElement =
      _signer.Sign(
        timestampElement,
        "#_0",
        securityTokenReferenceElement);
    securityElement.AppendChild(signatureElement);

    XmlElement bodyElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.SPrefix,
        "Body",
        CfdiDescargaMasivaNamespaces.SNamespaceUrl);
    

    XmlElement autenticaElement = xmlDocument.CreateElement("Autentica");
    autenticaElement.SetAttribute("xmlns", "http://DescargaMasivaTerceros.gob.mx");
    bodyElement.AppendChild(autenticaElement);
    envelopElement.AppendChild(bodyElement);
    return xmlDocument.OuterXml;
  }
}