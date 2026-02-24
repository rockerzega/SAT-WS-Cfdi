using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Application.Interfaces;

public interface IHttpSoapClient
{
  Task<SoapRequestResult> SendRequestAsync(string url,
                                           string soapAction,
                                           AccessToken accessToken,
                                           string requestContent,
                                           CancellationToken cancellationToken);
}

