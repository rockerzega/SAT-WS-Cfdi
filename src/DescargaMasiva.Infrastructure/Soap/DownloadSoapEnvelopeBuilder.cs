using System.Security.Cryptography.X509Certificates;
using System.Xml;
using DescargaMasiva.DescargaMasiva.Domain.Constants;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Security;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

internal sealed class DownloadSoapEnvelopeBuilder : ISoapEnvelopeBuilder<DownloadRequest>
{
  private readonly X509Certificate2 _certificate;

    public DownloadSoapEnvelopeBuilder(X509Certificate2 certificate)
    {
        _certificate = certificate;
    }

    public string Build(DownloadRequest downloadRequest)
    {
        var xmlDocument = new XmlDocument();

        XmlElement envelopElement = xmlDocument.CreateElement(
            CfdiDescargaMasivaNamespaces.S11Prefix,
            "Envelope",
            CfdiDescargaMasivaNamespaces.S11NamespaceUrl);

        envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.S11Prefix}",
            CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
        envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.DesPrefix}",
            CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
        envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.DsPrefix}",
            CfdiDescargaMasivaNamespaces.DsNamespaceUrl);

        xmlDocument.AppendChild(envelopElement);

        XmlElement headerElement = xmlDocument.CreateElement(
            CfdiDescargaMasivaNamespaces.S11Prefix,
            "Header",
            CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
        envelopElement.AppendChild(headerElement);

        XmlElement bodyElement = xmlDocument.CreateElement(
            CfdiDescargaMasivaNamespaces.S11Prefix,
            "Body",
            CfdiDescargaMasivaNamespaces.S11NamespaceUrl);
        envelopElement.AppendChild(bodyElement);

        XmlElement entradaElement = xmlDocument.CreateElement(
            CfdiDescargaMasivaNamespaces.DesPrefix,
            "PeticionDescargaMasivaTercerosEntrada",
            CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
        bodyElement.AppendChild(entradaElement);

        XmlElement peticionDescargaElement = xmlDocument.CreateElement(
            CfdiDescargaMasivaNamespaces.DesPrefix,
            "peticionDescarga",
            CfdiDescargaMasivaNamespaces.DesNamespaceUrl);

        peticionDescargaElement.SetAttribute("IdPaquete", downloadRequest.PackageId);
        peticionDescargaElement.SetAttribute("RfcSolicitante", downloadRequest.RequestingRfc);

        XmlElement signatureElement = SignedXmlHelper.SignRequest(peticionDescargaElement, _certificate);
        peticionDescargaElement.AppendChild(signatureElement);

        entradaElement.AppendChild(peticionDescargaElement);

        return xmlDocument.OuterXml;
    }
}