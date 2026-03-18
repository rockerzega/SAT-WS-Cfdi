using System.Xml;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Exceptions;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

public class DownloadSoapResponseParser : ISoapResponseParser<Result<DownloadData>>
{
  public Result<DownloadData> Parse(SoapRequestResult soapRequestResult)
  {
    var xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(soapRequestResult.ResponseContent);

    XmlNode element = xmlDocument.GetElementsByTagName("h:respuesta")[0];
    if (element is null)
      throw new InvalidResponseContentException(
        "Element h:respuesta is missing in response.",
        soapRequestResult.ResponseContent);

    if (element.Attributes is null)
      throw new InvalidResponseContentException(
        "Attributes property of Element h:respuesta is null.",
        soapRequestResult.ResponseContent);

    string package = xmlDocument.GetElementsByTagName("Paquete")[0]?.InnerXml
                     ?? throw new InvalidOperationException("Element Paquete not found.");

    string requestStatusCode = element.Attributes.GetNamedItem("CodEstatus")?.Value ?? string.Empty;
    string requestStatusMessage = element.Attributes.GetNamedItem("Mensaje")?.Value ?? string.Empty;
    var data = new DownloadData(
      package,
      requestStatusCode,
      requestStatusMessage
    );
    return Result<DownloadData>.Success(data);
  }
}