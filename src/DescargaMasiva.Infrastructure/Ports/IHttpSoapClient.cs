using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

public interface IHttpSoapClient
{
  Task<SoapRequestResult> SendRequestAsync(string url,
                                           string soapAction,
                                           AccessToken accessToken,
                                           string requestContent,
                                           CancellationToken cancellationToken);
}
