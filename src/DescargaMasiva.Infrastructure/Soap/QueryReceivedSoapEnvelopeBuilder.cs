using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using DescargaMasiva.DescargaMasiva.Domain.Constants;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Security;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

internal sealed class QueryReceivedSoapEnvelopeBuilder : ISoapEnvelopeBuilder<QueryRequest>
{
  private readonly X509Certificate2? _defaultCertificate;

  public QueryReceivedSoapEnvelopeBuilder(X509Certificate2? defaultCertificate)
  {
    _defaultCertificate = defaultCertificate;
  }

  public string Build(QueryRequest queryRequest)
  {
    if (queryRequest.HasInlineSigningPfx)
    {
      using var inlineCert = LoadPfxFromRequest(queryRequest);
      return BuildEnvelope(queryRequest, inlineCert);
    }

    if (_defaultCertificate is null)
    {
      throw new InvalidOperationException(
        "No hay certificado para firmar: configura DescargaMasiva:CertificatePath o envía \"certificate\" (Base64 del .pfx) y \"password\" en el JSON.");
    }

    return BuildEnvelope(queryRequest, _defaultCertificate);
  }

  private static X509Certificate2 LoadPfxFromRequest(QueryRequest queryRequest)
  {
    byte[] pfxBytes;
    try
    {
      pfxBytes = Convert.FromBase64String(queryRequest.Certificate!.Trim());
    }
    catch (FormatException ex)
    {
      throw new CryptographicException("El campo certificate no es Base64 válido.", ex);
    }

    try
    {
      return new X509Certificate2(
        pfxBytes,
        queryRequest.Password ?? string.Empty,
        X509KeyStorageFlags.EphemeralKeySet | X509KeyStorageFlags.Exportable);
    }
    catch (Exception ex)
    {
      throw new CryptographicException("No se pudo cargar el .pfx con la contraseña indicada.", ex);
    }
  }

  private static string BuildEnvelope(QueryRequest queryRequest, X509Certificate2 certificate)
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
    
    XmlElement solicitaDescargaElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
      "SolicitaDescargaRecibidos",
      CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
    bodyElement.AppendChild(solicitaDescargaElement);

    XmlElement solicitudElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
      "solicitud",
      CfdiDescargaMasivaNamespaces.DesNamespaceUrl);

      solicitudElement.SetAttribute("FechaInicial", queryRequest.StartDate.ToSoapStartDateString());

      solicitudElement.SetAttribute("FechaFinal", queryRequest.EndDate.ToSoapEndDateString());

    if (!queryRequest.HasSenderRfc)
      solicitudElement.SetAttribute("RfcEmisor", queryRequest.SenderRfc);

    solicitudElement.SetAttribute("RfcSolicitante", queryRequest.RequestingRfc);
    solicitudElement.SetAttribute("RfcReceptor", queryRequest.RequestingRfc);

    solicitudElement.SetAttribute("TipoSolicitud", queryRequest.RequestType.Name);

    // Optional
    if (queryRequest.HasThirdPartyRfc)
      solicitudElement.SetAttribute("RfcACuentaTerceros", queryRequest.ThirdPartyRfc);

    // Optional
    if (queryRequest.HasDocumentType)
      solicitudElement.SetAttribute("TipoComprobante", queryRequest.DocumentType.Value);

    // Optional
    if (queryRequest.HasDocumentStatus)
      solicitudElement.SetAttribute("EstadoComprobante", queryRequest.DocumentStatus.Name.ToString());

    //Optional
    if (queryRequest.HasComplement)
      solicitudElement.SetAttribute("Complemento", queryRequest.Complement);

    // Optional
    if (queryRequest.HasUuid)
      solicitudElement.SetAttribute("Folio", queryRequest.Uuid);

    XmlElement signatureElement = SignedXmlHelper.SignRequest(solicitudElement, certificate);
    solicitudElement.AppendChild(signatureElement);
    solicitaDescargaElement.AppendChild(solicitudElement);
    Console.WriteLine("XML de salida");
    Console.WriteLine(xmlDocument.OuterXml);
    return xmlDocument.OuterXml;
  }
}