using DescargaMasiva.DescargaMasiva.Domain.Constants;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Adapters;

public sealed class QuerySoapAdapter 
  : BaseSoapAdapter<QueryRequest, QueryResult>, IQueryPort
{
  public QuerySoapAdapter(
    IHttpSoapClient httpSoapClient,
    ISoapEnvelopeBuilder<QueryRequest> builder,
    ISoapResponseParser<QueryResult> parser)
    : base(
      httpSoapClient,
      builder,
      parser,
      new SoapEndpoint(WsUri.QueryUri, WsUri.QuerySoapActionIssuedUri))
  {
  }

  public Task<QueryResult> ExecuteAsync(
    QueryRequest request,
    CancellationToken cancellationToken = default)
  {
    return ExecuteInternalAsync(
      request,
      request.AccessToken,
      cancellationToken);
  }
}