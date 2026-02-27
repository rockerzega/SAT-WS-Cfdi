using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

public interface IDownloadSoapResponseParser
{
  DownloadResult Parse(SoapRequestResult soapRequestResult);
}