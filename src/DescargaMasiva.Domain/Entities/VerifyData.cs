namespace DescargaMasiva.DescargaMasiva.Domain.Entities;

public sealed class VerifyData
{
  public VerifyData(
    IReadOnlyList<string> packageIds,
    string downloadRequestStatusNumber,
    string downloadRequestStatusCode,
    string numberOfCfdis,
    string requestStatusCode,
    string requestStatusMessage)
  {
    PackageIds = packageIds ?? throw new ArgumentNullException(nameof(packageIds));
    DownloadRequestStatusNumber = downloadRequestStatusNumber ?? throw new ArgumentNullException(nameof(downloadRequestStatusNumber));
    DownloadRequestStatusCode = downloadRequestStatusCode ?? throw new ArgumentNullException(nameof(downloadRequestStatusCode));
    NumberOfCfdis = numberOfCfdis ?? throw new ArgumentNullException(nameof(numberOfCfdis));
    RequestStatusCode = requestStatusCode ?? throw new ArgumentNullException(nameof(requestStatusCode));
    RequestStatusMessage = requestStatusMessage ?? throw new ArgumentNullException(nameof(requestStatusMessage));
  }

  public IReadOnlyList<string> PackageIds { get; }

  public string DownloadRequestStatusNumber { get; }

  public string DownloadRequestStatusCode { get; }

  public string NumberOfCfdis { get; }

  public string RequestStatusCode { get; }

  public string RequestStatusMessage { get; }
}