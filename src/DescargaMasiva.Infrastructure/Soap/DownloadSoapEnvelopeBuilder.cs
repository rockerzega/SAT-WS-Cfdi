using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using DescargaMasiva.DescargaMasiva.Domain.Constants;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Security;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

internal sealed class DownloadSoapEnvelopeBuilder : ISoapEnvelopeBuilder<DownloadRequest>
{
  private readonly X509Certificate2? _defaultCertificate;

  public DownloadSoapEnvelopeBuilder(X509Certificate2? defaultCertificate)
  {
    _defaultCertificate = defaultCertificate;
  }

  public string Build(DownloadRequest downloadRequest)
  {
    if (downloadRequest.HasInlineSigningPfx)
    {
      using var inlineCert = LoadPfxFromRequest(downloadRequest);
      return BuildEnvelope(downloadRequest, inlineCert);
    }

    if (_defaultCertificate is null)
    {
      throw new InvalidOperationException(
        "No hay certificado para firmar: configura DescargaMasiva:CertificatePath o envía \"certificate\" (Base64 del .pfx) y \"password\" en el JSON.");
    }

    return BuildEnvelope(downloadRequest, _defaultCertificate);
  }

  private static X509Certificate2 LoadPfxFromRequest(DownloadRequest downloadRequest)
  {
    byte[] pfxBytes;
    try
    {
      pfxBytes = Convert.FromBase64String(downloadRequest.Certificate!.Trim());
    }
    catch (FormatException ex)
    {
      throw new CryptographicException("El campo certificate no es Base64 válido.", ex);
    }

    try
    {
      return new X509Certificate2(
        pfxBytes,
        downloadRequest.Password ?? string.Empty,
        X509KeyStorageFlags.EphemeralKeySet | X509KeyStorageFlags.Exportable);
    }
    catch (Exception ex)
    {
      throw new CryptographicException("No se pudo cargar el .pfx con la contraseña indicada.", ex);
    }
  }

  private static string BuildEnvelope(DownloadRequest downloadRequest, X509Certificate2 certificate)
  {
    var xmlDocument = new XmlDocument();

    XmlElement envelopElement = xmlDocument.CreateElement(
      CfdiDescargaMasivaNamespaces.SPrefix,
      "Envelope",
      CfdiDescargaMasivaNamespaces.SNamespaceUrl
    );

    envelopElement.SetAttribute(
      $"xmlns:{CfdiDescargaMasivaNamespaces.SPrefix}",
      CfdiDescargaMasivaNamespaces.SNamespaceUrl
    );
    envelopElement.SetAttribute(
      $"xmlns:{CfdiDescargaMasivaNamespaces.DesPrefix}",
      CfdiDescargaMasivaNamespaces.DesNamespaceUrl
    );
    envelopElement.SetAttribute(
      $"xmlns:{CfdiDescargaMasivaNamespaces.DsPrefix}",
      CfdiDescargaMasivaNamespaces.DsNamespaceUrl
    );

    xmlDocument.AppendChild(envelopElement);

    XmlElement headerElement = xmlDocument.CreateElement(
      CfdiDescargaMasivaNamespaces.SPrefix,
      "Header",
      CfdiDescargaMasivaNamespaces.SNamespaceUrl
    );
    envelopElement.AppendChild(headerElement);

    XmlElement bodyElement = xmlDocument.CreateElement(
      CfdiDescargaMasivaNamespaces.SPrefix,
      "Body",
      CfdiDescargaMasivaNamespaces.SNamespaceUrl
    );
    envelopElement.AppendChild(bodyElement);

    XmlElement entradaElement = xmlDocument.CreateElement(
      CfdiDescargaMasivaNamespaces.DesPrefix,
      "PeticionDescargaMasivaTercerosEntrada",
      CfdiDescargaMasivaNamespaces.DesNamespaceUrl
    );
    bodyElement.AppendChild(entradaElement);

    XmlElement peticionDescargaElement = xmlDocument.CreateElement(
      CfdiDescargaMasivaNamespaces.DesPrefix,
      "peticionDescarga",
      CfdiDescargaMasivaNamespaces.DesNamespaceUrl
    );

    peticionDescargaElement.SetAttribute("IdPaquete", downloadRequest.PackageId);
    peticionDescargaElement.SetAttribute("RfcSolicitante", downloadRequest.RequestingRfc);

    XmlElement signatureElement = SignedXmlHelper.SignRequest(peticionDescargaElement, certificate);
    peticionDescargaElement.AppendChild(signatureElement);

    entradaElement.AppendChild(peticionDescargaElement);
    Console.WriteLine("**************************************************************************");
    Console.WriteLine("Salida de download");
    Console.WriteLine(xmlDocument.OuterXml);
    Console.WriteLine("**************************************************************************");
    return xmlDocument.OuterXml;
  }
}