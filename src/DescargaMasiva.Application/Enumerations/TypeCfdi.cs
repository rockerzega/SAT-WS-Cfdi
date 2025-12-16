using Ardalis.SmartEnum;

namespace DescargaMasiva.DescargaMasiva.Application.Enumerations;

public sealed class TypeCfdi : SmartEnum<TypeCfdi>
{
  /// <summary>
  ///     Null = Null
  /// </summary>
  public static readonly TypeCfdi Null = new TypeCfdi("Null", "Ninguno");

  /// <summary>
  ///     I = Ingreso
  /// </summary>
  public static readonly TypeCfdi Ingreso = new TypeCfdi("Ingreso", "I");

  /// <summary>
  ///     E = Egreso
  /// </summary>
  public static readonly TypeCfdi Egreso = new TypeCfdi("Egreso", "E");

  /// <summary>
  ///     T = Traslado
  /// </summary>
  public static readonly TypeCfdi Traslado = new TypeCfdi("Traslado", "T");

  /// <summary>
  ///     N = Nomina
  /// </summary>
  public static readonly TypeCfdi Nomina = new TypeCfdi("Nomina", "N");

  /// <summary>
  ///     P = Pago
  /// </summary>
  public static readonly TypeCfdi Pago = new TypeCfdi("Pago", "P");

  private TypeCfdi(string name, string value) : base(name, value)
  {
  }
}