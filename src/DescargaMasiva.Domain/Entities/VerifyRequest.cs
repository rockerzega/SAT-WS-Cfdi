using System;

namespace DescargaMasiva.DescargaMasiva.Domain.Entities;

/// <summary>
///     Peticion de verificacion.
/// </summary>
public sealed class VerifyRequest
{
  public VerifyRequest(
    string requestId,
    string requestingRfc,
    AccessToken accessToken,
    string? certificate = null,
    string? password = null)
  {
    RequestId = requestId ?? throw new ArgumentNullException(nameof(requestId));
    RequestingRfc = requestingRfc ?? throw new ArgumentNullException(nameof(requestingRfc));
    AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
    Certificate = certificate;
    Password = password;
  }

  /// <summary>
  ///     IdSolicitud - Contiene el Identificador de la solicitud que se pretende consultar.
  /// </summary>
  public string RequestId { get; }

  /// <summary>
  ///     RfcSolicitante - Contiene el RFC del solicitante que genero la petición de solicitud de descarga masiva.
  /// </summary>
  public string RequestingRfc { get; }

  /// <summary>
  ///     Token de autorizacion.
  /// </summary>
  public AccessToken AccessToken { get; }

  /// <summary>
  ///     Contenido del .pfx en Base64 (opcional, JSON: "certificate"). Si se envía, se usa para firmar en lugar del certificado de configuración.
  /// </summary>
  public string? Certificate { get; }

  /// <summary>
  ///     Contraseña del .pfx (JSON: "password"; puede ser vacía si el PFX no tiene clave).
  /// </summary>
  public string? Password { get; }

  public bool HasInlineSigningPfx => !string.IsNullOrWhiteSpace(Certificate);

  public static VerifyRequest CreateInstance(string requestId, string requestingRfc, AccessToken accessToken)
  {
    return new VerifyRequest(requestId, requestingRfc, accessToken, null, null);
  }
}

