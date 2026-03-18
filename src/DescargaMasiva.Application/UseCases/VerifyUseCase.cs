using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Ports;

namespace DescargaMasiva.DescargaMasiva.Application.UseCases;

public sealed class VerifyUseCase
{
  private readonly IVerifyPort _verifyPort;

  public VerifyUseCase(IVerifyPort verifyPort)
  {
    _verifyPort = verifyPort;
  }

  public async Task<Result<VerifyData>> ExecuteAsync(
    VerifyRequest request,
    CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(request.RequestId))
      throw new ArgumentException("RequestId cannot be empty.");

    return await _verifyPort.ExecuteAsync(request, cancellationToken);
  }
}