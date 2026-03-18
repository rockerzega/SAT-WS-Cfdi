using System.Xml;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Exceptions;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

internal sealed class AuthSoapResponseParser : ISoapResponseParser<AuthResult>
{
  public AuthResult Parse(SoapRequestResult soapRequestResult)
  {
    var xmlDocument = new XmlDocument();
    xmlDocument.LoadXml(soapRequestResult.ResponseContent);

    XmlNode autenticaResultElement =
      xmlDocument.GetElementsByTagName("AutenticaResult")[0];

    if (autenticaResultElement != null)
    {
      var accessToken =
        AccessToken.CreateInstance(autenticaResultElement.InnerXml);

      return AuthResult.CreateSuccess(
        accessToken,
        soapRequestResult.HttpStatusCode,
        soapRequestResult.ResponseContent);
    }

    XmlNode faultElement =
      xmlDocument.GetElementsByTagName("s:Fault")[0];

    if (faultElement == null)
      throw new InvalidResponseContentException(
        "Elements AutenticaResult and s:Fault are missing in response.",
        soapRequestResult.ResponseContent);

    string faultCode =
      xmlDocument.GetElementsByTagName("faultcode")[0]?.InnerXml
      ?? throw new InvalidOperationException("Element faultcode not found.");

    string faultString =
      xmlDocument.GetElementsByTagName("faultstring")[0]?.InnerXml
      ?? throw new InvalidOperationException("Element faultstring not found.");

    return AuthResult.CreateFailure(
      faultCode,
      faultString,
      soapRequestResult.HttpStatusCode,
      soapRequestResult.ResponseContent);
  }
}