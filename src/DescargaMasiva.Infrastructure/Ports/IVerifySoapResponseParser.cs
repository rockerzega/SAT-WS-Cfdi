using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

public interface IVerifySoapResponseParser
{
  VerifyResult Parse(SoapRequestResult soapRequestResult);
}