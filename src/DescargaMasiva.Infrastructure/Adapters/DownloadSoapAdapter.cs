using DescargaMasiva.DescargaMasiva.Domain.Constants;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Adapters;

public sealed class DownloadSoapAdapter
  : BaseSoapAdapter<DownloadRequest, DownloadResult>, IDownloadPort
{
  public DownloadSoapAdapter(
    IHttpSoapClient httpSoapClient,
    ISoapEnvelopeBuilder<DownloadRequest> builder,
    ISoapResponseParser<DownloadResult> parser)
    : base(
      httpSoapClient,
      builder,
      parser,
      new SoapEndpoint(WsUri.DownloadUri, WsUri.DownloadSoapActionUri))
  {
  }

  public Task<DownloadResult> ExecuteAsync(
    DownloadRequest request,
    CancellationToken cancellationToken = default)
  {
    return ExecuteInternalAsync(
      request,
      request.AccessToken,
      cancellationToken);
  }
}