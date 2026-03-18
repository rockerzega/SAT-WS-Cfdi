using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Domain.Ports;

public interface IQueryPort
{
  Task<QueryResult> ExecuteAsync(
    QueryRequest request,
    CancellationToken cancellationToken = default);
}