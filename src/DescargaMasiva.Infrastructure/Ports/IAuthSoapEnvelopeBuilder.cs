using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

public interface IAuthSoapEnvelopeBuilder
{
  string Build(AuthRequest request);
}