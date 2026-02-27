using System.Security.Cryptography.X509Certificates;
using DescargaMasiva.DescargaMasiva.Domain.Entities;

namespace DescargaMasiva.DescargaMasiva.Domain.Ports;

/// <summary>
///     Servicio para mandar peticiones de verificacion al web service de descarga masiva de CFDIs del SAT
/// </summary>
public interface IVerifyService
{
  /// <summary>
  ///     Genera el contenido para la peticion SOAP enviada al web service
  /// </summary>
  /// <param name="verifyRequest">Peticion</param>
  /// <param name="certificate">Certificado del SAT (.pfx)</param>
  /// <returns>El contenido para la peticion SOAP</returns>
  string GenerateSoapRequestEnvelopeXmlContent(VerifyRequest verifyRequest, X509Certificate2 certificate);

  /// <summary>
  ///     Envia la peticion al web service de descarga masiva de CFDIs del SAT.
  /// </summary>
  /// <param name="soapRequestContent">Contenido para la peticion SOAP generado por GenerateSoapRequestEnvelopeXmlContent</param>
  /// <param name="accessToken">Token de autorizacion que regresa la peticion de Autenticacion</param>
  /// <param name="cancellationToken">Token de cancelacion</param>
  Task<SoapRequestResult> SendSoapRequestAsync(string soapRequestContent,
                                               AccessToken accessToken,
                                               CancellationToken cancellationToken);

  /// <summary>
  ///     Envia la peticion al web service de descarga masiva de CFDIs del SAT.
  /// </summary>
  /// <param name="verifyRequest">Peicion</param>
  /// <param name="certificate">Certificado SAT (.pfx)</param>
  /// <param name="cancellationToken">Token de cancelacion</param>
  /// <returns>El resultado de la peticion.</returns>
  Task<VerifyResult> SendSoapRequestAsync(VerifyRequest verifyRequest,
                                                X509Certificate2 certificate,
                                                CancellationToken cancellationToken);

  /// <summary>
  ///     Transforma el resultado de la peticion SOAP en un resultado con los valores asignados al tipo de peticion.
  /// </summary>
  /// <param name="soapRequestResult">Resultado SOAP</param>
  /// <returns>Resultado de la peticion</returns>
  VerifyResult GetSoapResponseResult(SoapRequestResult soapRequestResult);
}
