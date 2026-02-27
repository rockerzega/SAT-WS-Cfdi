using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

public interface IAuthSoapResponseParser
{
  AuthResult Parse(SoapRequestResult soapRequestResult);
}