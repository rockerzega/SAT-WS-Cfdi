using DescargaMasiva.DescargaMasiva.Application.Enumerations;

namespace DescargaMasiva.DescargaMasiva.Core.Entities;

/// <summary>
///     Peticion de solicitud.
/// </summary>
public sealed class SolicitudRequest
{
    private SolicitudRequest(DateTime startDate,
                             DateTime endDate,
                             QueryType requestType,
                             string senderRfc,
                             IEnumerable<string> recipientsRfcs,
                             string requestingRfc,
                             AccessToken accessToken,
                             TypeCfdi documentType,
                             StatusCfdi documentStatus,
                             string thirdPartyRfc,
                             string complement,
                             string uuid)
    {
        StartDate = startDate;
        EndDate = endDate;
        RequestType = requestType ?? throw new ArgumentNullException(nameof(requestType));
        SenderRfc = senderRfc ?? throw new ArgumentNullException(nameof(senderRfc));
        RecipientsRfcs = recipientsRfcs ?? throw new ArgumentNullException(nameof(recipientsRfcs));
        RequestingRfc = requestingRfc ?? throw new ArgumentNullException(nameof(requestingRfc));
        AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        DocumentType = documentType ?? throw new ArgumentNullException(nameof(documentType));
        DocumentStatus = documentStatus ?? throw new ArgumentNullException(nameof(documentStatus));
        ThirdPartyRfc = thirdPartyRfc ?? throw new ArgumentNullException(nameof(thirdPartyRfc));
        Complement = complement ?? throw new ArgumentNullException(nameof(complement));
        Uuid = uuid ?? throw new ArgumentNullException(nameof(uuid));
    }

    /// <summary>
    ///     FechaInicial - Solo se buscarán CFDI, cuya fecha de emisión sea igual o mayor a la fecha inicial indicada en este
    ///     parámetro
    ///     Parámetro obligatorio. Este parámetro no debe declararse en caso de realizar una consulta por el folio fiscal
    ///     (UUID).
    /// </summary>
    public DateTime StartDate { get; }

    /// <summary>
    ///     Solo se buscarán CFDI, cuya fecha de emisión sea igual o menor a la fecha final indicada en este parámetro
    ///     Parámetro obligatorio. Este parámetro no debe declararse en caso de realizar una consulta por el folio fiscal
    ///     (UUID).
    /// </summary>
    public DateTime EndDate { get; }

    /// <summary>
    ///     TipoSolicitud - Define el tipo de descarga: [Metadata, CFDI]
    /// </summary>
    public QueryType RequestType { get; }

    /// <summary>
    ///     RfcEmisor - Contiene el RFC del emisor del cual se quiere consultar los CFDI.
    ///     Parámetro obligatorio. Este parámetro no debe declararse en caso de realizar una consulta por el folio fiscal
    ///     (UUID).
    /// </summary>
    public string SenderRfc { get; }

    /// <summary>
    ///     RfcReceptores - Contiene el/los RFCs receptores de los cuales se quiere consultar los CFDIs
    ///     Importante: El campo RfcReceptor, únicamente permite la captura de 5 registros como máximo.
    /// </summary>
    public IEnumerable<string> RecipientsRfcs { get; }

    /// <summary>
    ///     RfcSolicitante - Contiene el RFC del que está realizando la solicitud de descarga.
    ///     Parámetro obligatorio. Este parámetro no debe declararse en caso de realizar una consulta por el folio fiscal
    ///     (UUID). Adicionalmente debe corresponder al parámetro RfcEmisor
    /// </summary>
    public string RequestingRfc { get; }

    /// <summary>
    ///     Token de autorizacion.
    /// </summary>
    public AccessToken AccessToken { get; }

    /// <summary>
    ///     TipoComprobante - Define el tipo de comprobante: [Null, I = Ingreso, E = Egreso, T= Traslado, N = Nomina, P = Pago]
    ///     *Null es el valor predeterminado y en caso de no declararse, se obtendrán todos los comprobantes sin importar el
    ///     tipo comprobante.
    /// </summary>
    public TypeCfdi DocumentType { get; }

    /// <summary>
    ///     EstadoComprobante - Define el estado del comprobante: [Null, 0 = Cancelado, 1 = Vigente]
    ///     * Null es el valor predeterminado y en caso de no declararse, se obtendrán todos los comprobantes sin importar el
    ///     estado de los mismos.
    ///     Consideraciones:
    ///     • Para efectos de la información de la metadata, el listado incluirá los comprobantes vigentes y cancelados
    ///     • En el caso para la descarga de XML, solo incluirán los CFDI vigentes.Por lo que, el servicio no descargará XML
    ///     cancelados.
    /// </summary>
    public StatusCfdi DocumentStatus { get; }

    /// <summary>
    ///     RfcACuentaTerceros - Contiene el RFC del a cuenta a tercero del cual se quiere consultar los CFDIs
    /// </summary>
    public string ThirdPartyRfc { get; }

    /// <summary>
    ///     Complemento - Define el complemento de CFDI a descargar:
    /// </summary>
    public string Complement { get; }

    /// <summary>
    ///     UUID - Folio Fiscal con formato: XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX
    ///     Parámetro obligatorio. En caso de utilizarse este parámetro no deben declararse los siguientes criterios de
    ///     búsqueda: FechaInicial, FechaFinal, RfcEmisor y RfcSolicitante.
    /// </summary>
    public string Uuid { get; }

    public bool HasDocumentType => DocumentType != TypeCfdi.Null;
    public bool HasDocumentStatus => DocumentStatus != StatusCfdi.Null;
    public bool HasComplement => !string.IsNullOrWhiteSpace(Complement);
    public bool HasUuid => !string.IsNullOrWhiteSpace(Uuid);
    public bool HasThirdPartyRfc => !string.IsNullOrWhiteSpace(ThirdPartyRfc);

    public static SolicitudRequest CreateInstance(DateTime startDate,
                                                  DateTime endDate,
                                                  QueryType requestType,
                                                  string senderRfc,
                                                  IEnumerable<string> recipientsRfcs,
                                                  string requestingRfc,
                                                  AccessToken accessToken,
                                                  TypeCfdi documentType,
                                                  StatusCfdi documentStatus,
                                                  string thirdPartyRfc,
                                                  string complement,
                                                  string uuid)
    {
        List<string> recipients = recipientsRfcs.ToList();
        if (recipients.Any() && recipients.Count > 5)
            throw new ArgumentOutOfRangeException(nameof(RecipientsRfcs), "There can only be a maximum of 5 recipient RFC.");

        return new SolicitudRequest(startDate,
            endDate,
            requestType,
            senderRfc,
            recipients,
            requestingRfc,
            accessToken,
            documentType,
            documentStatus,
            thirdPartyRfc,
            complement,
            uuid);
    }

    public static SolicitudRequest CreateInstance(DateTime startDate,
                                                  DateTime endDate,
                                                  QueryType requestType,
                                                  string senderRfc,
                                                  IEnumerable<string> recipientsRfcs,
                                                  string requestingRfc,
                                                  AccessToken accessToken)
    {
        List<string> recipients = recipientsRfcs.ToList();
        if (recipients.Any() && recipients.Count > 5)
            throw new ArgumentOutOfRangeException(nameof(RecipientsRfcs), "There can only be a maximum of 5 recipient RFC.");

        return new SolicitudRequest(startDate,
            endDate,
            requestType,
            senderRfc,
            recipients,
            requestingRfc,
            accessToken,
            TypeCfdi.Null,
            StatusCfdi.Null,
            string.Empty,
            string.Empty,
            string.Empty);
    }

    public static SolicitudRequest CreateInstance(string uuid, string requestingRfc, AccessToken accessToken)
    {
        return new SolicitudRequest(DateTime.MinValue,
            DateTime.MinValue,
            QueryType.Cfdi,
            "",
            Enumerable.Empty<string>(),
            requestingRfc,
            accessToken,
            TypeCfdi.Null,
            StatusCfdi.Null,
            string.Empty,
            string.Empty,
            uuid);
    }
}
