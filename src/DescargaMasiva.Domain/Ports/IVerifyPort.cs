using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Domain.Ports;

public interface IVerifyPort
{
  Task<VerifyResult> ExecuteAsync(
    VerifyRequest request,
    CancellationToken cancellationToken = default);
}