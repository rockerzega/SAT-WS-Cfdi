using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Domain.Ports;

public interface IDownloadPort
{
  Task<DownloadResult> ExecuteAsync(
    DownloadRequest request,
    CancellationToken cancellationToken = default);
}