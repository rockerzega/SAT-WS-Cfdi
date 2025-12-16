using Ardalis.SmartEnum;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Adapters.Soap.Enumerations;

public sealed class StatusCfdi : SmartEnum<StatusCfdi>
{
  /// <summary>
  ///     Null
  /// </summary>
  public static readonly StatusCfdi Null = new StatusCfdi("Null", -1);

  /// <summary>
  ///     Cancelado = 0
  /// </summary>
  public static readonly StatusCfdi Cancelado = new StatusCfdi("Cancelado", 0);

  /// <summary>
  ///     Vigente = 1
  /// </summary>
  public static readonly StatusCfdi Vigente = new StatusCfdi("Vigente", 1);

  private StatusCfdi(string name, int value) : base(name, value)
  {
  }  
}