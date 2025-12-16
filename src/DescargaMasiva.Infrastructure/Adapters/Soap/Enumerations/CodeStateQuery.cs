using Ardalis.SmartEnum;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Adapters.Soap.Enumerations;

public sealed class CodeStateQuery : SmartEnum<CodeStateQuery>
{
  public static readonly CodeStateQuery _5000 = new CodeStateQuery("5000",
    5000,
    "Solicitud recibida con éxito",
    "Indica que la solicitud de descarga que se está verificando fue aceptada."
  );
  
  /// <summary>
  ///     5002 = Se agotó las solicitudes de por vida
  /// </summary>
  public static readonly CodeStateQuery _5002 = new CodeStateQuery("5002",
    5002,
    "Se agotó las solicitudes de por vida",
    "Para el caso de descarga de tipo CFDI, se tiene unlímite máximo para solicitudes con los mismos parámetros(Fecha Inicial, Fecha Final, RfcEmisor, RfcReceptor)."
  );

  /// <summary>
  ///     5003 = Tope máximo
  /// </summary>
  public static readonly CodeStateQuery _5003 = new CodeStateQuery("5003",
    5003,
    "Tope máximo",
    "Indica que en base a los parámetros de consulta se está superando el tope máximo de CFDI o Metadata, por solicitud de descarga masiva.");

  /// <summary>
  ///     5004 = No se encontró la información
  /// </summary>
  public static readonly CodeStateQuery _5004 = new CodeStateQuery("5004",
    5004,
    "No se encontró la información",
    "Indica que la solicitud de descarga que se está verificando no generó paquetes por falta de información.");

  /// <summary>
  ///     5005 = Solicitud duplicada
  /// </summary>
  public static readonly CodeStateQuery _5005 = new CodeStateQuery("5005",
    5005,
    "Solicitud duplicada",
    "En caso de que exista una solicitud vigente con los mismos parámetros (Fecha Inicial, Fecha Final, RfcEmisor, RfcReceptor, TipoSolicitud), no se permitirá generar una nueva solicitud.");

  /// <summary>
  ///     404 = Error no Controlado
  /// </summary>
  public static readonly CodeStateQuery _404 = new CodeStateQuery("404",
    404,
    "Error no Controlado",
    "Error genérico, en caso de presentarse realizar nuevamente la petición y si persiste el error levantar un RMA.");
  
  private CodeStateQuery(string name, int value, string message, string observations) 
    : base(name, value)
  {
    Message = message;
    Observations = observations;
  }
  public string Message { get; }
  public string Observations { get; }
}