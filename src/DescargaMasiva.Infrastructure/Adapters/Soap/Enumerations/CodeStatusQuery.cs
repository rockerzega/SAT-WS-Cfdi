using Ardalis.SmartEnum;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Adapters.Soap.Enumerations;

public sealed class CodeStatusQuery : SmartEnum<CodeStatusQuery>
{
  
  private CodeStatusQuery(string name, int value, string message, string observations) : base(name, value)
  {
    Message = message;
    Observations = observations;
  }

  public string Message { get; }
  public string Observations { get; }
  
  /// <summary>
  ///     300 = Usuario No Válido
  /// </summary>
  public static readonly CodeStatusQuery _300 = new CodeStatusQuery(
    "300",
    300,
    "Usuario No Válido",
    ""
  );
  
  /// <summary>
  ///     301 = XML Mal Formado
  /// </summary>
  public static readonly CodeStatusQuery _301 = new CodeStatusQuery("301",
    301,
    "XML Mal Formado",
    "Este código de error se regresa cuando el request posee información invalida, ejemplo: un RFC de receptor no valido");

  /// <summary>
  ///     302 = Sello Mal Formado
  /// </summary>
  public static readonly CodeStatusQuery _302 = new CodeStatusQuery("302", 302, "Sello Mal Formado", "");

  /// <summary>
  ///     303 = Sello no corresponde con RfcSolicitante
  /// </summary>
  public static readonly CodeStatusQuery _303 =
    new CodeStatusQuery("303", 303, "Sello no corresponde con RfcSolicitante", "");

  /// <summary>
  ///     304 = Certificado Revocado o Caduco
  /// </summary>
  public static readonly CodeStatusQuery _304 = new CodeStatusQuery("304",
    304,
    "Certificado Revocado o Caduco",
    "El certificado fue revocado o bien la fecha de vigencia expiró");

  /// <summary>
  ///     305 = Certificado Inválido
  /// </summary>
  public static readonly CodeStatusQuery _305 = new CodeStatusQuery("305",
    305,
    "Certificado Inválido",
    "El certificado puede ser invalido por múltiples razones como son el tipo, codificación incorrecta, etc.");

  /// <summary>
  ///     404 = Error no Controlado
  /// </summary>
  public static readonly CodeStatusQuery _404 = new CodeStatusQuery("404",
    404,
    "Error no Controlado",
    "Error genérico, en caso de presentarse realizar nuevamente la petición y si persiste el error levantar un RMA.");

  /// <summary>
  ///     5000 = Solicitud de descarga recibida con éxito
  /// </summary>
  public static readonly CodeStatusQuery _5000 =
    new CodeStatusQuery("5000", 5000, "Solicitud de descarga recibida con éxito", "");

  /// <summary>
  ///     5001 = Tercero no autorizado
  /// </summary>
  public static readonly CodeStatusQuery _5001 = new CodeStatusQuery("5001",
    5001,
    "Tercero no autorizado",
    "El solicitante no tiene autorización de descarga de xml de los contribuyentes");
  
  /// <summary>
  ///     5002 = Se han agotado las solicitudes de por vida
  /// </summary>
  public static readonly CodeStatusQuery _5002 = new CodeStatusQuery("5002",
    5002,
    "Se han agotado las solicitudes de por vida",
    "Se ha alcanzado el límite de solicitudes, con el mismo criterio");

  /// <summary>
  ///     5003 = Se han agotado las solicitudes de por vida
  /// </summary>
  public static readonly CodeStatusQuery _5003 = new CodeStatusQuery("5003",
    5003,
    "Tope máximo",
    "Indica que en base a los parámetros de consulta se está superando el tope máximo de CFDI o Metadata, por solicitud de descarga masiva.");
  /// <summary>
  ///     5004 = No se encontró la información
  /// </summary>
  public static readonly CodeStatusQuery _5004 = new CodeStatusQuery("5004",
    5004,
    "No se encontró la información",
    "No se encontró la información del paquete solicitado");

  /// <summary>
  ///     5005 = Ya se tiene una solicitud registrada
  /// </summary>
  public static readonly CodeStatusQuery _5005 = new CodeStatusQuery("5005",
    5005,
    "Ya se tiene una solicitud registrada",
    "Ya existe una solicitud activa con los mismos criterios");

  /// <summary>
  ///     5006 = Error interno en el proceso
  /// </summary>
  public static readonly CodeStatusQuery _5006 = new CodeStatusQuery("5006", 5006, "Error interno en el proceso", "");

  /// <summary>
  ///     5007 = No existe el paquete solicitado
  /// </summary>
  public static readonly CodeStatusQuery _5007 = new CodeStatusQuery("5007",
    5007,
    "No existe el paquete solicitado",
    "Los paquetes solo tienen un periodo de vida de 72hrs");

  /// <summary>
  ///     5008 = Máximo de descargas permitidas
  /// </summary>
  public static readonly CodeStatusQuery _5008 = new CodeStatusQuery(
    "5008",
    5008,
    "Máximo de descargas permitidas",
    "Un paquete solo puede descargarse un total de 2 veces, una vez agotadas, ya no se podrá volver a descargar"
  );
}