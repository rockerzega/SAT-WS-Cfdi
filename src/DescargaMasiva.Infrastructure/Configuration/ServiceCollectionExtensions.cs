using System.Security.Cryptography.X509Certificates;
using DescargaMasiva.DescargaMasiva.Application.UseCases;
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

    // 3️⃣ Registrar IAuth
    services.AddScoped<IAuthSoapEnvelopeBuilder, AuthSoapEnvelopeBuilder>();
    services.AddScoped<IAuthSoapResponseParser, AuthSoapResponseParser>();

    // 4️⃣ Registrar Adapter
    services.AddScoped<IAuthPort, AuthSoapAdapter>();

    // 5️⃣ Registrar UseCase
    services.AddScoped<AuthUseCase>();
    
    services.AddScoped<IDownloadSoapEnvelopeBuilder, DownloadSoapEnvelopeBuilder>();
    services.AddScoped<IDownloadSoapResponseParser, DownloadSoapResponseParser>();
    services.AddScoped<IDownloadPort, DownloadSoapAdapter>();
    services.AddScoped<DownloadUseCase>();

    return services;
  }
}