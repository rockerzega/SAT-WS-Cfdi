using Ardalis.SmartEnum.SystemTextJson;
using DescargaMasiva.DescargaMasiva.Application.UseCases;
using DescargaMasiva.DescargaMasiva.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DescargaMasiva.DescargaMasiva.Domain.Entities;
using DescargaMasiva.DescargaMasiva.Domain.Enums;
using DescargaMasiva.DescargaMasiva.Domain.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Adapters;
using DescargaMasiva.DescargaMasiva.Infrastructure.Ports;
using DescargaMasiva.DescargaMasiva.Infrastructure.Security;
using DescargaMasiva.DescargaMasiva.Infrastructure.Soap;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(o =>
{
  o.SerializerOptions.Converters.Add(new SmartEnumNameConverter<QueryType, int>());
  o.SerializerOptions.Converters.Add(new SmartEnumValueConverter<TypeCfdi, string>());
  o.SerializerOptions.Converters.Add(new SmartEnumNameConverter<StatusCfdi, int>());
});

builder.Services.AddDescargaMasiva(options =>
{
  var section = builder.Configuration.GetSection("DescargaMasiva");
  options.CertificatePath = section["CertificatePath"] ?? string.Empty;
  options.CertificatePassword = section["CertificatePassword"] ?? string.Empty;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// =========================
// AUTH
// =========================
app.MapPost("/auth", async (AuthUseCase useCase) =>
{
  var result = await useCase.ExecuteAsync();

  return result.Match(
    success => Results.Ok(success),
    (code, message) => Results.BadRequest(new { code, message })
  );
});

// =========================
// AUTH (PFX via form file)
// =========================
app.MapPost("/auth-file", async (
  IFormFile pfx,
  [FromForm] string password,
  IHttpSoapClient httpSoapClient,
  ISoapResponseParser<Result<AccessToken>> parser,
  CancellationToken cancellationToken) =>
{
  if (pfx is null || pfx.Length == 0)
    return Results.BadRequest(new { code = "PFX_REQUIRED", message = "El archivo .pfx es requerido." });

  if (string.IsNullOrWhiteSpace(password))
    return Results.BadRequest(new { code = "PASSWORD_REQUIRED", message = "La contraseña del .pfx es requerida." });

  byte[] pfxBytes;
  await using (var ms = new MemoryStream())
  {
    await pfx.CopyToAsync(ms, cancellationToken);
    pfxBytes = ms.ToArray();
  }

  X509Certificate2 certificate;
  try
  {
    certificate = new X509Certificate2(
      pfxBytes,
      password,
      X509KeyStorageFlags.EphemeralKeySet | X509KeyStorageFlags.Exportable);
  }
  catch (Exception ex)
  {
    return Results.BadRequest(new { code = "PFX_INVALID", message = $"No se pudo cargar el certificado .pfx: {ex.Message}" });
  }

  var signer = new X509AuthRequestSigner(certificate);
  ISoapEnvelopeBuilder<AuthRequest> envelopeBuilder = new AuthSoapEnvelopeBuilder(signer);
  IAuthPort authPort = new AuthSoapAdapter(httpSoapClient, envelopeBuilder, parser);

  var useCase = new AuthUseCase(authPort);
  var result = await useCase.ExecuteAsync(cancellationToken);

  return result.Match(
    success => Results.Ok(success),
    (code, message) => Results.BadRequest(new { code, message })
  );
}).DisableAntiforgery();


// =========================
// QUERY
// =========================
app.MapPost("/query-issued", async (QueryRequest request, [FromKeyedServices(QueryPortKeys.Issued)] QueryUseCase useCase) =>
{
  try
  {
    var result = await useCase.ExecuteAsync(request);

    return result.Match(
      success => Results.Ok(success),
      (code, message) => Results.BadRequest(new { code, message }));
  }
  catch (CryptographicException ex)
  {
    return Results.BadRequest(new { code = "SIGNING_CERT_ERROR", message = ex.Message });
  }
  catch (InvalidOperationException ex)
  {
    return Results.BadRequest(new { code = "SIGNING_CERT_CONFIG", message = ex.Message });
  }
});

app.MapPost("/query-received", async (QueryRequest request, [FromKeyedServices(QueryPortKeys.Received)] QueryUseCase useCase) =>
{
  try
  {
    var result = await useCase.ExecuteAsync(request);

    return result.Match(
      success => Results.Ok(success),
      (code, message) => Results.BadRequest(new { code, message }));
  }
  catch (CryptographicException ex)
  {
    return Results.BadRequest(new { code = "SIGNING_CERT_ERROR", message = ex.Message });
  }
  catch (InvalidOperationException ex)
  {
    return Results.BadRequest(new { code = "SIGNING_CERT_CONFIG", message = ex.Message });
  }
});

// =========================
// VERIFY
// =========================
app.MapPost("/verify", async (VerifyRequest request, VerifyUseCase useCase) =>
{
  try
  {
    var result = await useCase.ExecuteAsync(request);

    return result.Match(
      success => Results.Ok(success),
      (code, message) => Results.BadRequest(new { code, message }));
  }
  catch (CryptographicException ex)
  {
    return Results.BadRequest(new { code = "SIGNING_CERT_ERROR", message = ex.Message });
  }
  catch (InvalidOperationException ex)
  {
    return Results.BadRequest(new { code = "SIGNING_CERT_CONFIG", message = ex.Message });
  }
});


// =========================
// DOWNLOAD
// =========================
app.MapPost("/download", async (DownloadRequest request, DownloadUseCase useCase) =>
{
  try
  {
    var result = await useCase.ExecuteAsync(request);

    return result.Match(
      success => Results.Ok(success),
      (code, message) => Results.BadRequest(new { code, message }));
  }
  catch (CryptographicException ex)
  {
    return Results.BadRequest(new { code = "SIGNING_CERT_ERROR", message = ex.Message });
  }
  catch (InvalidOperationException ex)
  {
    return Results.BadRequest(new { code = "SIGNING_CERT_CONFIG", message = ex.Message });
  }
});


app.Run();
