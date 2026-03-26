using System.Xml;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

internal sealed class AuthSoapResponseParser 
  : ISoapResponseParser<Result<AccessToken>>
{
  public Result<AccessToken> Parse(SoapRequestResult soapRequestResult)
  {
    var xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(soapRequestResult.ResponseContent);
    Console.WriteLine("--------------------------------------------------------------------------");
    Console.WriteLine(xmlDocument.OuterXml);
    Console.WriteLine("--------------------------------------------------------------------------");
    XmlNode autenticaResultElement =
      xmlDocument.GetElementsByTagName("AutenticaResult")[0];

    if (autenticaResultElement != null)
    {
      var token =
        AccessToken.CreateInstance(autenticaResultElement.InnerXml);
      return Result<AccessToken>.Success(token);
    }

    string faultCode =
      xmlDocument.GetElementsByTagName("faultcode")[0]?.InnerXml ?? "UNKNOWN";

    string faultString =
      xmlDocument.GetElementsByTagName("faultstring")[0]?.InnerXml ?? "Unknown error";

    return Result<AccessToken>.Failure(faultCode, faultString);
  }
}