namespace DescargaMasiva.DescargaMasiva.Domain.Entities;

public sealed class QueryData
{
  public QueryData(
    string requestId,
    string requestStatusCode,
    string requestStatusMessage)
  {
    RequestId = requestId;
    RequestStatusCode = requestStatusCode;
    RequestStatusMessage = requestStatusMessage;
  }

  public string RequestId { get; }
  public string RequestStatusCode { get; }
  public string RequestStatusMessage { get; }
}