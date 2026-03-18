using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

public interface IQuerySoapResponseParser
{
  QueryResult Parse(SoapRequestResult soapRequestResult);
}