
namespace DescargaMasiva.DescargaMasiva.Domain.Entities;

/// <summary>
///     Peticion de autenticacion.
/// </summary>
public sealed class AuthRequest
{
  private AuthRequest(DateTime tokenCreatedDateUtc, DateTime tokenExpiresDateUtc, Guid uuid)
  {
      TokenCreatedDateUtc = tokenCreatedDateUtc;
      TokenExpiresDateUtc = tokenExpiresDateUtc;
      Uuid = uuid;
  }

  /// <summary>
  ///     Fecha de cuando el token fue creado en formato UTC.
  /// </summary>
  public DateTime TokenCreatedDateUtc { get; }

  /// <summary>
  ///     Fecha de cuando el token expira en formato UTC.
  /// </summary>
  public DateTime TokenExpiresDateUtc { get; }

  /// <summary>
  ///     UUID unico para asociar a la peticion.
  /// </summary>
  public Guid Uuid { get; }

  public static AuthRequest CreateInstance()
  {
      DateTime tokenCreationDateUtc = DateTime.UtcNow;
      return new AuthRequest(tokenCreationDateUtc, tokenCreationDateUtc.AddMinutes(5), Guid.NewGuid());
  }

  public static AuthRequest CreateInstance(DateTime tokenCreatedDateUtc, DateTime tokenExpiresDateUtc, Guid uuid)
  {
      return new AuthRequest(tokenCreatedDateUtc, tokenExpiresDateUtc, uuid);
  }
}
