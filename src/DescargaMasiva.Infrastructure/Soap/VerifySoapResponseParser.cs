using System.Xml;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Enums;
using DescargaMasiva.DescargaMasiva.Domain.Exceptions;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

internal sealed class VerifySoapResponseParser : IVerifySoapResponseParser
{
    public VerifyResult Parse(SoapRequestResult soapRequestResult)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(soapRequestResult.ResponseContent);

        XmlNode resultElement =
            xmlDocument.GetElementsByTagName("VerificaSolicitudDescargaResult")[0];

        if (resultElement is null)
            throw new InvalidResponseContentException(
                "Element VerificaSolicitudDescargaResult is missing in response.",
                soapRequestResult.ResponseContent);

        string downloadRequestStatusNumber =
            resultElement.Attributes?.GetNamedItem("EstadoSolicitud")?.Value ?? string.Empty;

        string downloadRequestStatusCode =
            resultElement.Attributes?.GetNamedItem("CodigoEstadoSolicitud")?.Value ?? string.Empty;

        string numberOfCfdis =
            resultElement.Attributes?.GetNamedItem("NumeroCFDIs")?.Value ?? string.Empty;

        string requestStatusCode =
            resultElement.Attributes?.GetNamedItem("CodEstatus")?.Value ?? string.Empty;

        string requestStatusMessage =
            resultElement.Attributes?.GetNamedItem("Mensaje")?.Value ?? string.Empty;

        var packageIdsList = new List<string>();

        if (downloadRequestStatusNumber == StatusQuery.Terminada.Value.ToString())
        {
            XmlNodeList idsPaquetesElements =
                xmlDocument.GetElementsByTagName("IdsPaquetes");

            foreach (XmlNode idPaqueteElement in idsPaquetesElements)
                packageIdsList.Add(idPaqueteElement.InnerText);
        }

        return VerifyResult.CreateInstance(
            packageIdsList,
            downloadRequestStatusNumber,
            downloadRequestStatusCode,
            numberOfCfdis,
            requestStatusCode,
            requestStatusMessage,
            soapRequestResult.HttpStatusCode,
            soapRequestResult.ResponseContent);
    }
}