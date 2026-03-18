using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Ports;

namespace DescargaMasiva.DescargaMasiva.Application.UseCases;

public sealed class QueryUseCase
{
  private readonly IQueryPort _queryPort;

  public QueryUseCase(IQueryPort queryPort)
  {
    _queryPort = queryPort;
  }

  public async Task<QueryResult> ExecuteAsync(
    QueryRequest request,
    CancellationToken cancellationToken = default)
  {
    if (!request.HasUuid && request.StartDate > request.EndDate)
      throw new ArgumentException("StartDate cannot be greater than EndDate.");

    return await _queryPort.ExecuteAsync(request, cancellationToken);
  }
}