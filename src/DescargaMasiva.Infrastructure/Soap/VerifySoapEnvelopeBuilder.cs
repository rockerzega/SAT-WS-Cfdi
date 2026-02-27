using System.Security.Cryptography.X509Certificates;
using System.Xml;
using DescargaMasiva.DescargaMasiva.Domain.Constants;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Security;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

internal sealed class VerifySoapEnvelopeBuilder : ISoapEnvelopeBuilder<VerifyRequest>
{
  private readonly X509Certificate2 _certificate;

  public VerifySoapEnvelopeBuilder(X509Certificate2 certificate)
  {
    _certificate = certificate;
  }

  public string Build(VerifyRequest verifyRequest)
  {
    var xmlDocument = new XmlDocument();

    XmlElement envelopElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix,
      "Envelope",
      CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
    envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.S11Prefix}", CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
    envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.DesPrefix}", CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
    envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.DsPrefix}", CfdiDescargaMasivaNamespaces.DsNamespaceUrl);
    xmlDocument.AppendChild(envelopElement);

    XmlElement headerElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix,
      "Header",
      CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
    envelopElement.AppendChild(headerElement);

    XmlElement bodyElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.S11Prefix,
      "Body",
      CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
    envelopElement.AppendChild(bodyElement);

    XmlElement verificaSolicitudDescargaElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
      "VerificaSolicitudDescarga",
      CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
    bodyElement.AppendChild(verificaSolicitudDescargaElement);

    XmlElement solicitudElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
      "solicitud",
      CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
    solicitudElement.SetAttribute("IdSolicitud", verifyRequest.RequestId);
    solicitudElement.SetAttribute("RfcSolicitante", verifyRequest.RequestingRfc);

    XmlElement signatureElement = SignedXmlHelper.SignRequest(solicitudElement, _certificate);
    solicitudElement.AppendChild(signatureElement);
    verificaSolicitudDescargaElement.AppendChild(solicitudElement);

    return xmlDocument.OuterXml;
  }
}