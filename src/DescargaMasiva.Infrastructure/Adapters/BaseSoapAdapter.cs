using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Adapters;

public abstract class BaseSoapAdapter<TRequest, TResult>
{
  private readonly IHttpSoapClient _httpSoapClient;
  private readonly ISoapEnvelopeBuilder<TRequest> _builder;
  private readonly ISoapResponseParser<TResult> _parser;
  private readonly SoapEndpoint _endpoint;

  protected BaseSoapAdapter(
    IHttpSoapClient httpSoapClient,
    ISoapEnvelopeBuilder<TRequest> builder,
    ISoapResponseParser<TResult> parser,
    SoapEndpoint endpoint)
  {
    _httpSoapClient = httpSoapClient;
    _builder = builder;
    _parser = parser;
    _endpoint = endpoint;
  }

  protected async Task<TResult> ExecuteInternalAsync(
    TRequest request,
    AccessToken accessToken,
    CancellationToken cancellationToken)
  {
    string soapContent = _builder.Build(request);

    SoapRequestResult soapResult =
      await _httpSoapClient.SendRequestAsync(
        _endpoint.Uri,
        _endpoint.SoapAction,
        accessToken,
        soapContent,
        cancellationToken);

    return _parser.Parse(soapResult);
  }
}