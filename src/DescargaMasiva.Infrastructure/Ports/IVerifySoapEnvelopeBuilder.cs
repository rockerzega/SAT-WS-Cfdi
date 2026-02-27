using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

public interface IVerifySoapEnvelopeBuilder
{
  string Build(VerifyRequest request);
}