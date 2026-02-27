using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

public interface IDownloadSoapEnvelopeBuilder
{
  string Build(DownloadRequest request);
}