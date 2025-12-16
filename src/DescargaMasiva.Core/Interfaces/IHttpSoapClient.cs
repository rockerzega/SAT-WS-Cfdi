using DescargaMasiva.DescargaMasiva.Core.Entities;

namespace DescargaMasiva.DescargaMasiva.Core.Interfaces;

public interface IHttpSoapClient
{
  Task<SoapRequestResult> SendRequestAsync(string url,
                                           string soapAction,
                                           AccessToken accessToken,
                                           string requestContent,
                                           CancellationToken cancellationToken);
}

