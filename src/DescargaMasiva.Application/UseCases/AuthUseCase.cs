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

  public async Task<AuthResult> ExecuteAsync(
    CancellationToken cancellationToken = default)
  {
    // Aquí decides cómo crear la petición
    var authRequest = AuthRequest.CreateInstance();

    return await _authPort.AuthenticateAsync(authRequest, cancellationToken);
  }
}