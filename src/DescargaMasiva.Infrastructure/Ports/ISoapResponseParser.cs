using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

public interface ISoapResponseParser<TResult>
{
  TResult Parse(SoapRequestResult soapRequestResult);
}