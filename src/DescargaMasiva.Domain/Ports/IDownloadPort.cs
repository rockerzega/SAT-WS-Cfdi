using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Domain.Ports;

public interface IDownloadPort
{
  Task<Result<DownloadData>> ExecuteAsync(
    DownloadRequest request,
    CancellationToken cancellationToken = default);
}