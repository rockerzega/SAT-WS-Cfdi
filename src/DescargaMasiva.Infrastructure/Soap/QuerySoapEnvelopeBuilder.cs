using System.Security.Cryptography.X509Certificates;
using System.Xml;
using DescargaMasiva.DescargaMasiva.Domain.Constants;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Security;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

internal sealed class QuerySoapEnvelopeBuilder : ISoapEnvelopeBuilder<QueryRequest>
{
  private readonly X509Certificate2 _certificate;

  public QuerySoapEnvelopeBuilder(X509Certificate2 certificate)
  {
    _certificate = certificate;
  }
  
  public string Build(QueryRequest queryRequest)
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
      "SolicitaDescarga",
      CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
    bodyElement.AppendChild(solicitaDescargaElement);

    XmlElement solicitudElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
      "solicitud",
      CfdiDescargaMasivaNamespaces.DesNamespaceUrl);

    if (!queryRequest.HasUuid)
      solicitudElement.SetAttribute("FechaInicial", queryRequest.StartDate.ToSoapStartDateString());

    if (!queryRequest.HasUuid)
      solicitudElement.SetAttribute("FechaFinal", queryRequest.EndDate.ToSoapEndDateString());

    if (!queryRequest.HasUuid)
      solicitudElement.SetAttribute("RfcEmisor", queryRequest.SenderRfc);

    solicitudElement.SetAttribute("RfcSolicitante", queryRequest.RequestingRfc);

    solicitudElement.SetAttribute("TipoSolicitud", queryRequest.RequestType.Name);

    // Optional
    if (queryRequest.HasThirdPartyRfc)
      solicitudElement.SetAttribute("RfcACuentaTerceros", queryRequest.ThirdPartyRfc);

    // Optional
    if (queryRequest.HasDocumentType)
      solicitudElement.SetAttribute("TipoComprobante", queryRequest.DocumentType.Value);

    // Optional
    if (queryRequest.HasDocumentStatus)
      solicitudElement.SetAttribute("EstadoComprobante", queryRequest.DocumentStatus.Value.ToString());

    //Optional
    if (queryRequest.HasComplement)
      solicitudElement.SetAttribute("Complemento", queryRequest.Complement);

    // Optional
    if (queryRequest.HasUuid)
      solicitudElement.SetAttribute("Folio", queryRequest.Uuid);

    if (!queryRequest.HasUuid)
    {
      XmlElement rfcReceptores = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
        "RfcReceptores",
        CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
      foreach (string item in queryRequest.RecipientsRfcs)
      {
        XmlElement rfcReceptorElement = xmlDocument.CreateElement(CfdiDescargaMasivaNamespaces.DesPrefix,
            "RfcReceptor",
            CfdiDescargaMasivaNamespaces.DesNamespaceUrl);
        rfcReceptorElement.InnerText = item;
        rfcReceptores.AppendChild(rfcReceptorElement);
      }

      solicitudElement.AppendChild(rfcReceptores);
    }

    XmlElement signatureElement = SignedXmlHelper.SignRequest(solicitudElement, _certificate);
    solicitudElement.AppendChild(signatureElement);
    solicitaDescargaElement.AppendChild(solicitudElement);

    return xmlDocument.OuterXml;
  }
}