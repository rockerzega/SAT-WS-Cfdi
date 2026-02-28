using System.Security.Cryptography.X509Certificates;
using DescargaMasiva.DescargaMasiva.Application.UseCases;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Adapters;
using DescargaMasiva.DescargaMasiva.Infrastructure.Http;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Soap;

namespace DescargaMasiva.DescargaMasiva.Infrastructure.Configuration;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddDescargaMasiva(
    this IServiceCollection services,
    Action<DescargaMasivaOptions> configure)
  {
    var options = new DescargaMasivaOptions();
    configure(options);

    // 1️⃣ Registrar certificado como Singleton
    services.AddSingleton(provider =>
      new X509Certificate2(
        options.CertificatePath,
        options.CertificatePassword,
        X509KeyStorageFlags.MachineKeySet |
        X509KeyStorageFlags.Exportable));

    // 2️⃣ Registrar HttpSoapClient
    services.AddHttpClient<IHttpSoapClient, HttpSoapClient>();

    // =========================
    // AUTH
    // =========================

    services.AddScoped<
      ISoapEnvelopeBuilder<AuthRequest>,
      AuthSoapEnvelopeBuilder>();

    services.AddScoped<
      ISoapResponseParser<Result<AccessToken>>,
      AuthSoapResponseParser>();

    services.AddScoped<IAuthPort, AuthSoapAdapter>();
    services.AddScoped<AuthUseCase>();

    // =========================
    // QUERY
    // =========================    
    
    services.AddScoped<
      ISoapEnvelopeBuilder<QueryRequest>,
      QuerySoapEnvelopeBuilder>();

    services.AddScoped<
      ISoapResponseParser<Result<QueryData>>,
      QuerySoapResponseParser>();

    services.AddScoped<IQueryPort, QuerySoapAdapter>();
    services.AddScoped<QueryUseCase>();
    
    // =========================
    // VERIFY
    // =========================    
    
    services.AddScoped<
      ISoapEnvelopeBuilder<VerifyRequest>,
      VerifySoapEnvelopeBuilder>();

    services.AddScoped<
      ISoapResponseParser<Result<VerifyData>>,
      VerifySoapResponseParser>();

    services.AddScoped<IVerifyPort, VerifySoapAdapter>();
    services.AddScoped<VerifyUseCase>();
    
    // =========================
    // DOWNLOAD
    // =========================

    services.AddScoped<
      ISoapEnvelopeBuilder<DownloadRequest>,
      DownloadSoapEnvelopeBuilder>();

    services.AddScoped<
      ISoapResponseParser<Result<DownloadData>>,
      DownloadSoapResponseParser>();

    services.AddScoped<IDownloadPort, DownloadSoapAdapter>();
    services.AddScoped<DownloadUseCase>();
    
    return services;
  }
}