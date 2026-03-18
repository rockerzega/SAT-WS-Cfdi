using System.Net;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Http;

internal sealed class SoapExecutionResult<TResult>
{
  public TResult Result { get; }
  public HttpStatusCode HttpStatusCode { get; }
  public string RawResponse { get; }

  public SoapExecutionResult(
    TResult result,
    HttpStatusCode httpStatusCode,
    string rawResponse)
  {
    Result = result;
    HttpStatusCode = httpStatusCode;
    RawResponse = rawResponse;
  }
}