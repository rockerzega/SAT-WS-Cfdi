using System.Web;
using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Http;

public static class AccessTokenExtensions
{
  public static string ToDecodedValue(this AccessToken token)
  {
    return HttpUtility.UrlDecode(token.Value);
  }

  public static string ToAuthorizationHeader(this AccessToken token)
  {
    return $@"WRAP access_token=""{HttpUtility.UrlDecode(token.Value)}""";
  }
}