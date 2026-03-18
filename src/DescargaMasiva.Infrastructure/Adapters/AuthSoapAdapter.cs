using System.Threading;
using System.Threading.Tasks;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Ports;
using DescargaMasiva.DescargaMasiva.Domain.Constants;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Adapters;

public sealed class AuthSoapAdapter 
  : BaseSoapAdapter<AuthRequest, Result<AccessToken>>, IAuthPort
{
  public AuthSoapAdapter(
    IHttpSoapClient httpSoapClient,
    ISoapEnvelopeBuilder<AuthRequest> builder,
    ISoapResponseParser<Result<AccessToken>> parser)
    : base(
      httpSoapClient,
      builder,
      parser,
      new SoapEndpoint(WsUri.AuthUri, WsUri.AuthSoapActionUri))
  {
  }

  public Task<Result<AccessToken>> ExecuteAsync(
    AuthRequest request,
    CancellationToken cancellationToken = default)
  {
    return ExecuteInternalAsync(
      request,
      AccessToken.CreateEmpty(),
      cancellationToken);
  }

  public Task<AuthResult> AuthenticateAsync(AuthRequest request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }
}