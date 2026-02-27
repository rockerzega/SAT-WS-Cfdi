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

    XmlElement envelopElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix,
                "Envelope",
                CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
    envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.S11Prefix}", CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
    envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.WsuPrefix}", CfdiDescargaMasivaNamespaces.WsuNamespaceUrl);
    xmlDocument.AppendChild(envelopElement);

    XmlElement headerElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix,
        "Header",
        CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
    envelopElement.AppendChild(headerElement);

    XmlElement securityElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WssePrefix,
        "Security",
        CfdiDescargaMasivaNamespaces.WsseNamespaceUrl);
    securityElement.SetAttribute("mustUnderstand", CfdiDescargaMasivaNamespaces.S11NamespaceUrl, "1");
    headerElement.AppendChild(securityElement);

    XmlElement timestampElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WsuPrefix,
        "Timestamp",
        CfdiDescargaMasivaNamespaces.WsuNamespaceUrl);
    timestampElement.SetAttribute("Id", CfdiDescargaMasivaNamespaces.WsuNamespaceUrl, "_0");
    securityElement.AppendChild(timestampElement);

    XmlElement createdElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WsuPrefix,
        "Created",
        CfdiDescargaMasivaNamespaces.WsuNamespaceUrl);
    createdElement.InnerText = authRequest.TokenCreatedDateUtc.ToSoapSecurityTimestampString();
    timestampElement.AppendChild(createdElement);

    XmlElement expiresElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WsuPrefix,
        "Expires",
        CfdiDescargaMasivaNamespaces.WsuNamespaceUrl);
    expiresElement.InnerText = authRequest.TokenExpiresDateUtc.ToSoapSecurityTimestampString();
    timestampElement.AppendChild(expiresElement);

    XmlElement binarySecurityTokenElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WssePrefix,
        "BinarySecurityToken",
        CfdiDescargaMasivaNamespaces.WsseNamespaceUrl);
    binarySecurityTokenElement.SetAttribute("Id",
        CfdiDescargaMasivaNamespaces.WsuNamespaceUrl,
        authRequest.Uuid.ToBinarySecurityTokenId());
    binarySecurityTokenElement.SetAttribute("ValueType",
        "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
    binarySecurityTokenElement.SetAttribute("EncodingType",
        "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
    binarySecurityTokenElement.InnerText =
      _signer.GetCertificateBase64();

    securityElement.AppendChild(binarySecurityTokenElement);

    XmlElement securityTokenReferenceElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WssePrefix,
        "SecurityTokenReference",
        CfdiDescargaMasivaNamespaces.WsseNamespaceUrl);
    XmlElement securityTokenReferenceReferenceElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.WssePrefix,
        "Reference",
        CfdiDescargaMasivaNamespaces.WsseNamespaceUrl);
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

    XmlElement bodyElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix,
        "Body",
        CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
    envelopElement.AppendChild(bodyElement);

    XmlElement autenticaElement = xmlDocument.CreateElement("Autentica");
    autenticaElement.SetAttribute("xmlns", "http://DescargaMasivaTerceros.gob.mx");
    bodyElement.AppendChild(autenticaElement);

    return xmlDocument.OuterXml;
  }
}