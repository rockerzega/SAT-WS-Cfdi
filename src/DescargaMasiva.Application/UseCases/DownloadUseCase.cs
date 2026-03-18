using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Ports;

namespace DescargaMasiva.DescargaMasiva.Application.UseCases;

public sealed class DownloadUseCase
{
  private readonly IDownloadPort _downloadPort;

  public DownloadUseCase(IDownloadPort downloadPort)
  {
    _downloadPort = downloadPort;
  }

  public async Task<Result<DownloadData>> ExecuteAsync(
    DownloadRequest request,
    CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(request.PackageId))
      throw new ArgumentException("PackageId cannot be empty.");

    return await _downloadPort.ExecuteAsync(request, cancellationToken);
  }
}