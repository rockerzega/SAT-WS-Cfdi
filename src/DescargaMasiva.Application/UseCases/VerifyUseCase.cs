using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Ports;

namespace DescargaMasiva.DescargaMasiva.Application.UseCases;

public sealed class VerifyRequestUseCase
{
  private readonly IVerifyPort _verifyPort;

  public VerifyRequestUseCase(IVerifyPort verifyPort)
  {
    _verifyPort = verifyPort;
  }

  public async Task<VerifyResult> ExecuteAsync(
    VerifyRequest request,
    CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(request.RequestId))
      throw new ArgumentException("RequestId cannot be empty.");

    return await _verifyPort.ExecuteAsync(request, cancellationToken);
  }
}