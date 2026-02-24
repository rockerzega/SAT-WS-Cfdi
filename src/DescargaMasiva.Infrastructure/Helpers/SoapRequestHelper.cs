using System.Web;

namespace DescargaMasiva.DescargaMasiva.Application.Helpers;

public class SoapRequestHelper
{
  public const string SoapSecurityTimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";

  public static string ToSoapSecurityTimestampString(DateTime date)
  {
    return date.ToString(SoapSecurityTimestampFormat);
  }

  public static string ToBinarySecurityTokenId(Guid uuid)
  {
    return $"uuid-{uuid}-1";
  }

  public static string ToSoapStartDateString(DateTime date)
  {
    return date.ToString("yyyy-MM-dd") + "T00:00:00";
  }

  public static string ToSoapEndDateString(DateTime date)
  {
    return date.ToString("yyyy-MM-dd") + "T23:59:59";
  }

  public static string CreateHttpAuthorizationHeaderFromToken(string token)
  {
    return $@"WRAP access_token=""{HttpUtility.UrlDecode(token)}""";
  }
}