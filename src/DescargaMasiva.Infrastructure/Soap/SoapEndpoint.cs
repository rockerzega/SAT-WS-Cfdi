namespace DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

public sealed class SoapEndpoint(string uri, string soapAction)
{
  public string Uri { get; } = uri;
  public string SoapAction { get; } = soapAction;
}