using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

public interface IQuerySoapEnvelopeBuilder
{
  string Build(QueryRequest request);
}