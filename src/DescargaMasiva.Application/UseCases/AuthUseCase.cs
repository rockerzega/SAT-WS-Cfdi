using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Ports;

namespace DescargaMasiva.DescargaMasiva.Application.UseCases;

public class AuthUseCase
{
  private readonly IAuthPort _authPort;

  public AuthUseCase(IAuthPort authPort)
  {
    _authPort = authPort;
  }

  public async Task<Result<AccessToken>> ExecuteAsync(
    CancellationToken cancellationToken = default)
  {
    // Aquí decides cómo crear la petición
    var authRequest = AuthRequest.CreateInstance();

    return await _authPort.ExecuteAsync(authRequest, cancellationToken);
  }
}