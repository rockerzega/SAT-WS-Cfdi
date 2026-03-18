using DescargaMasiva.DescargaMasiva.Domain.Constants;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Adapters;

public sealed class VerifySoapAdapter
  : BaseSoapAdapter<VerifyRequest, VerifyResult>, IVerifyPort
{
  public VerifySoapAdapter(
    IHttpSoapClient httpSoapClient,
    ISoapEnvelopeBuilder<VerifyRequest> builder,
    ISoapResponseParser<VerifyResult> parser)
    : base(
      httpSoapClient,
      builder,
      parser,
      new SoapEndpoint(WsUri.VerifyUri, WsUri.VerifySoapActionUri))
  {
  }

  public Task<VerifyResult> ExecuteAsync(
    VerifyRequest request,
    CancellationToken cancellationToken = default)
  {
    return ExecuteInternalAsync(
      request,
      request.AccessToken,
      cancellationToken);
  }
}