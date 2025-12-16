namespace DescargaMasiva.DescargaMasiva.Domain.Constants;

public static class CfdiDescargaMasivaWsUri
{
  public const string AuthUri = "https://cfdidescargamasivasolicitud.clouda.sat.gob.mx/Autenticacion/Autenticacion.svc";
  public const string AuthSoapActionUri = "http://DescargaMasivaTerceros.gob.mx/IAutenticacion/Autentica";
  public const string QueryUri = "https://cfdidescargamasivasolicitud.clouda.sat.gob.mx/SolicitaDescargaService.svc";
  public const string QuerySoapActionIssuedUri = "http://DescargaMasivaTerceros.sat.gob.mx/ISolicitaDescargaService/SolicitaDescargaEmitidos";
  public const string QuerySoapActionReceivedUri = "http://DescargaMasivaTerceros.sat.gob.mx/ISolicitaDescargaService/SolicitaDescargaRecibidos";
  public const string VerifyUri = "https://cfdidescargamasivasolicitud.clouda.sat.gob.mx/VerificaSolicitudDescargaService.svc";
  public const string VerifySoapActionUri =
    "http://DescargaMasivaTerceros.sat.gob.mx/IVerificaSolicitudDescargaService/VerificaSolicitudDescarga";
  public const string DownloadUri = "https://cfdidescargamasiva.clouda.sat.gob.mx/DescargaMasivaService.svc";
  public const string DownloadSoapActionUri = "http://DescargaMasivaTerceros.sat.gob.mx/IDescargaMasivaTercerosService/Descargar";
}