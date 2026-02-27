using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Domain.Ports;

public interface IAuthPort
{
  /// <summary>
  /// Metodo de authenticación
  /// </summary>
  /// <param name="request">Petición</param>
  /// <param name="cancellationToken">Certificado Sat (.pfx)</param>
  /// <returns></returns>
  Task<AuthResult> AuthenticateAsync(
    AuthRequest request,
    CancellationToken cancellationToken = default);
}