using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using DescargaMasiva.DescargaMasiva.Domain.Constants;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Security;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

internal sealed class VerifySoapEnvelopeBuilder : ISoapEnvelopeBuilder<VerifyRequest>
{
  private readonly X509Certificate2? _defaultCertificate;

  public VerifySoapEnvelopeBuilder(X509Certificate2? defaultCertificate)
  {
    _defaultCertificate = defaultCertificate;
  }

  public string Build(VerifyRequest verifyRequest)
  {
    if (verifyRequest.HasInlineSigningPfx)
    {
      using var inlineCert = LoadPfxFromRequest(verifyRequest);
      return BuildEnvelope(verifyRequest, inlineCert);
    }

    if (_defaultCertificate is null)
    {
      throw new InvalidOperationException(
        "No hay certificado para firmar: configura DescargaMasiva:CertificatePath o envía \"certificate\" (Base64 del .pfx) y \"password\" en el JSON.");
    }

    return BuildEnvelope(verifyRequest, _defaultCertificate);
  }

  private static X509Certificate2 LoadPfxFromRequest(VerifyRequest verifyRequest)
  {
    byte[] pfxBytes;
    try
    {
      pfxBytes = Convert.FromBase64String(verifyRequest.Certificate!.Trim());
    }
    catch (FormatException ex)
    {
      throw new CryptographicException("El campo certificate no es Base64 válido.", ex);
    }

    try
    {
      return new X509Certificate2(
        pfxBytes,
        verifyRequest.Password ?? string.Empty,
        X509KeyStorageFlags.EphemeralKeySet | X509KeyStorageFlags.Exportable);
    }
    catch (Exception ex)
    {
      throw new CryptographicException("No se pudo cargar el .pfx con la contraseña indicada.", ex);
    }
  }

  private static string BuildEnvelope(VerifyRequest verifyRequest, X509Certificate2 certificate)
  {
    var xmlDocument = new XmlDocument();

    XmlElement envelopElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.SPrefix,
      "Envelope",
      CfdiDescargaMasivaNamespaces.SNamespaceUrl);
    envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.SPrefix}", CfdiDescargaMasivaNamespaces.SNamespaceUrl);
    envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.DesPrefix}", CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
    envelopElement.SetAttribute($"xmlns:{CfdiDescargaMasivaNamespaces.DsPrefix}", CfdiDescargaMasivaNamespaces.DsNamespaceUrl);
    xmlDocument.AppendChild(envelopElement);

    XmlElement headerElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.SPrefix,
      "Header",
      CfdiDescargaMasivaNamespaces.SNamespaceUrl);
    envelopElement.AppendChild(headerElement);

    XmlElement bodyElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.SPrefix,
      "Body",
      CfdiDescargaMasivaNamespaces.SNamespaceUrl);
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

    XmlElement signatureElement = SignedXmlHelper.SignRequest(solicitudElement, certificate);
    solicitudElement.AppendChild(signatureElement);
    verificaSolicitudDescargaElement.AppendChild(solicitudElement);
    Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
    Console.WriteLine("XML de salida verify");
    Console.WriteLine(xmlDocument.OuterXml);
    Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
    return xmlDocument.OuterXml;
  }
}