using System;
using System.Net;

namespace DescargaMasiva.DescargaMasiva.Domain.Entities;
/// <summary>
///     Resultado de la peticion de autenticacion.
/// </summary>
public sealed class AuthResult
{
  private AuthResult(
    bool isSuccess,
    AccessToken? accessToken,
    string? errorCode,
    string? errorMessage)
  {
    IsSuccess = isSuccess;
    AccessToken = accessToken;
    ErrorCode = errorCode;
    ErrorMessage = errorMessage;
  }

  public bool IsSuccess { get; }

  public AccessToken? AccessToken { get; }

  public string? ErrorCode { get; }

  public string? ErrorMessage { get; }

  public static AuthResult CreateSuccess(AccessToken accessToken)
    => new AuthResult(true, accessToken, null, null);

  public static AuthResult CreateFailure(string errorCode, string errorMessage)
    => new AuthResult(false, null, errorCode, errorMessage);
}
