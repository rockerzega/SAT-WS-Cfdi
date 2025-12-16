using Ardalis.SmartEnum;

namespace DescargaMasiva.DescargaMasiva.Application.Enumerations;

public class StatusQuery: SmartEnum<StatusQuery>
{
  /// <summary>
  ///     1 = Aceptada
  /// </summary>
  public static readonly StatusQuery Aceptada = new StatusQuery("Aceptada", 1);

  /// <summary>
  ///     2 = EnProceso
  /// </summary>
  public static readonly StatusQuery EnProceso = new StatusQuery("EnProceso", 2);

  /// <summary>
  ///     3 = Terminada
  /// </summary>
  public static readonly StatusQuery Terminada = new StatusQuery("Terminada", 3);

  /// <summary>
  ///     4 = Error
  /// </summary>
  public static readonly StatusQuery Error = new StatusQuery("Error", 4);

  /// <summary>
  ///     5 = Rechazada
  /// </summary>
  public static readonly StatusQuery Rechazada = new StatusQuery("Rechazada", 5);

  /// <summary>
  ///     6 = Vencida
  /// </summary>
  public static readonly StatusQuery Vencida = new StatusQuery("Vencida", 6);

  private StatusQuery(string name, int value) : base(name, value)
  {
  }

}