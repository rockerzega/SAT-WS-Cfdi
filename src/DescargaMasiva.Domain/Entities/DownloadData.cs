namespace DescargaMasiva.DescargaMasiva.Domain.Entities;

public sealed class DownloadData
{
  public DownloadData(
    string package,
    string requestStatusCode,
    string requestStatusMessage)
  {
    Package = package ?? throw new ArgumentNullException(nameof(package));
    RequestStatusCode = requestStatusCode ?? throw new ArgumentNullException(nameof(requestStatusCode));
    RequestStatusMessage = requestStatusMessage ?? throw new ArgumentNullException(nameof(requestStatusMessage));
  }

  /// <summary>
  /// Contenido del paquete descargado
  /// </summary>
  public string Package { get; }

  /// <summary>
  /// CodEstatus
  /// </summary>
  public string RequestStatusCode { get; }

  /// <summary>
  /// Mensaje
  /// </summary>
  public string RequestStatusMessage { get; }
}