namespace DescargaMasiva.DescargaMasiva.Domain.Entities;

/// <summary>
///     Token de autorizacion para autenticar peticiones con el web service de descarga masiva de CFDIs del SAT
/// </summary>
public sealed class AccessToken
{
  private AccessToken(string value)
  {
    Value = value ?? throw new ArgumentNullException(nameof(value));
  }

  public string Value { get; }

  public bool IsValid => !string.IsNullOrWhiteSpace(Value);

  public static AccessToken CreateInstance(string token)
    => new AccessToken(token);

  public static AccessToken CreateEmpty()
    => new AccessToken(string.Empty);
}