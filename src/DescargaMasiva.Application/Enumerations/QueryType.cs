using Ardalis.SmartEnum;

namespace DescargaMasiva.DescargaMasiva.Application.Enumerations;

public sealed class QueryType : SmartEnum<QueryType>
{
  /// <summary>
  ///     0 = CFDI
  /// </summary>
  public static readonly QueryType Cfdi = new QueryType("CFDI", 0);

  /// <summary>
  ///     1 = Metadata
  /// </summary>
  public static readonly QueryType Metadata = new QueryType("Metadata", 1);

  private QueryType(string name, int value) : base(name, value)
  {
  }

}