using System.Security.Cryptography.X509Certificates;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using DescargaMasiva.DescargaMasiva.Application.UseCases;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Adapters;
using DescargaMasiva.DescargaMasiva.Infrastructure.Http;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Security;
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

    // 1️⃣ Certificado por archivo (opcional si /query, /verify o /download envían PFX en JSON). Auth sigue exigiendo archivo configurado.
    var certHolder = new ApplicationCertificateHolder(options);
    services.AddSingleton(certHolder);
    services.AddSingleton(sp => sp.GetRequiredService<ApplicationCertificateHolder>().GetRequiredCertificate());

    // Registrar el signer que usa el certificado para firmar requests.
    services.AddScoped<IAuthRequestSigner, X509AuthRequestSigner>();

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
    // QUERY (emitidos vs recibidos comparten tipos; el último AddScoped<IQueryPort> reemplaza al anterior)
    // =========================

    services.AddKeyedScoped<IQueryPort>(QueryPortKeys.Issued, (sp, _) =>
    {
      var holder = sp.GetRequiredService<ApplicationCertificateHolder>();
      return new QueryIssuedSoapAdapter(
        sp.GetRequiredService<IHttpSoapClient>(),
        new QueryIssuedSoapEnvelopeBuilder(holder.DefaultCertificate),
        new QueryIssuedSoapResponseParser());
    });

    services.AddKeyedScoped<IQueryPort>(QueryPortKeys.Received, (sp, _) =>
    {
      var holder = sp.GetRequiredService<ApplicationCertificateHolder>();
      return new QueryReceivedSoapAdapter(
        sp.GetRequiredService<IHttpSoapClient>(),
        new QueryReceivedSoapEnvelopeBuilder(holder.DefaultCertificate),
        new QueryReceivedSoapResponseParser());
    });

    services.AddKeyedScoped<QueryUseCase>(QueryPortKeys.Issued, (sp, _) =>
      new QueryUseCase(sp.GetRequiredKeyedService<IQueryPort>(QueryPortKeys.Issued)));

    services.AddKeyedScoped<QueryUseCase>(QueryPortKeys.Received, (sp, _) =>
      new QueryUseCase(sp.GetRequiredKeyedService<IQueryPort>(QueryPortKeys.Received)));
    
    // =========================
    // VERIFY
    // =========================    
    
    services.AddScoped<ISoapEnvelopeBuilder<VerifyRequest>>(sp =>
      new VerifySoapEnvelopeBuilder(sp.GetRequiredService<ApplicationCertificateHolder>().DefaultCertificate));

    services.AddScoped<
      ISoapResponseParser<Result<VerifyData>>,
      VerifySoapResponseParser>();

    services.AddScoped<IVerifyPort, VerifySoapAdapter>();
    services.AddScoped<VerifyUseCase>();
    
    // =========================
    // DOWNLOAD
    // =========================

    services.AddScoped<ISoapEnvelopeBuilder<DownloadRequest>>(sp =>
      new DownloadSoapEnvelopeBuilder(sp.GetRequiredService<ApplicationCertificateHolder>().DefaultCertificate));

    services.AddScoped<
      ISoapResponseParser<Result<DownloadData>>,
      DownloadSoapResponseParser>();

    services.AddScoped<IDownloadPort, DownloadSoapAdapter>();
    services.AddScoped<DownloadUseCase>();
    
    return services;
  }

  private static X509Certificate2? TryLoadCertificateFromOptions(DescargaMasivaOptions options)
  {
    if (string.IsNullOrWhiteSpace(options.CertificatePath))
      return null;

    if (!File.Exists(options.CertificatePath))
    {
      throw new FileNotFoundException(
        "No se encontró el certificado (.pfx) en la ruta configurada.",
        options.CertificatePath);
    }

    return new X509Certificate2(
      options.CertificatePath,
      options.CertificatePassword,
      X509KeyStorageFlags.EphemeralKeySet | X509KeyStorageFlags.Exportable);
  }

  private sealed class ApplicationCertificateHolder
  {
    private readonly Lazy<X509Certificate2?> _lazy;

    public ApplicationCertificateHolder(DescargaMasivaOptions options)
    {
      _lazy = new Lazy<X509Certificate2?>(() => TryLoadCertificateFromOptions(options));
    }

    public X509Certificate2? DefaultCertificate => _lazy.Value;

    public X509Certificate2 GetRequiredCertificate() =>
      DefaultCertificate ?? throw new InvalidOperationException(
        "DescargaMasiva:CertificatePath no está configurado. " +
        "Define la ruta absoluta a tu .pfx en appsettings (sección DescargaMasiva), variable de entorno o User Secrets.");
  }
}