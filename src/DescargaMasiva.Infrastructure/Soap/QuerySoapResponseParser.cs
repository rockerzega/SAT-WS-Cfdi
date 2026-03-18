using System.Xml;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Exceptions;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

internal sealed class QuerySoapResponseParser 
  : ISoapResponseParser<Result<QueryData>>
{
  public Result<QueryData> Parse(SoapRequestResult soapRequestResult)
  {
    var xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(soapRequestResult.ResponseContent);

    XmlNode element =
      xmlDocument.GetElementsByTagName("SolicitaDescargaResult")[0];

    if (element is null)
      throw new InvalidResponseContentException(
        "Element SolicitaDescargaResult is missing in response.",
        soapRequestResult.ResponseContent);

    string requestId =
      element.Attributes?.GetNamedItem("IdSolicitud")?.Value ?? string.Empty;

    string requestStatusCode =
      element.Attributes?.GetNamedItem("CodEstatus")?.Value ?? string.Empty;

    string requestStatusMessage =
      element.Attributes?.GetNamedItem("Mensaje")?.Value ?? string.Empty;
    var QueryData = new QueryData(
      requestId,
      requestStatusCode,
      requestStatusMessage);
    return Result<QueryData>.Success(QueryData);
  }
}