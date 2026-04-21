using System;

namespace DescargaMasiva.DescargaMasiva.Domain.Entities;

/// <summary>
///     Peticion de descarga.
/// </summary>
public sealed class DownloadRequest
{
  public DownloadRequest(
    string packageId,
    string requestingRfc,
    AccessToken accessToken,
    string? certificate = null,
    string? password = null)
  {
    PackageId = packageId ?? throw new ArgumentNullException(nameof(packageId));
    RequestingRfc = requestingRfc ?? throw new ArgumentNullException(nameof(requestingRfc));
    AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
    Certificate = certificate;
    Password = password;
  }

  /// <summary>
  ///     IdPaquete - Contiene el identificador del paquete que se desea descargar.
  /// </summary>
  public string PackageId { get; }

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

  public static DownloadRequest CreateInstace(string packageId, string requestingRfc, AccessToken accessToken)
  {
    return new DownloadRequest(packageId, requestingRfc, accessToken, null, null);
  }
}
