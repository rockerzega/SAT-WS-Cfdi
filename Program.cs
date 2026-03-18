using DescargaMasiva.DescargaMasiva.Application.UseCases;
using DescargaMasiva.DescargaMasiva.Infrastructure.Configuration;
using DescargaMasiva.DescargaMasiva.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDescargaMasiva(options =>
{
  options.CertificatePath = "miCertificado.pfx";
  options.CertificatePassword = "123456";
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
// QUERY
// =========================
app.MapPost("/query", async (QueryRequest request, QueryUseCase useCase) =>
{
  var result = await useCase.ExecuteAsync(request);

  return result.Match(
    success => Results.Ok(success),
    (code, message) => Results.BadRequest(new { code, message })
  );
});


// =========================
// VERIFY
// =========================
app.MapPost("/verify", async (VerifyRequest request, VerifyUseCase useCase) =>
{
  var result = await useCase.ExecuteAsync(request);

  return result.Match(
    success => Results.Ok(success),
    (code, message) => Results.BadRequest(new { code, message })
  );
});


// =========================
// DOWNLOAD
// =========================
app.MapPost("/download", async (DownloadRequest request, DownloadUseCase useCase) =>
{
  var result = await useCase.ExecuteAsync(request);

  return result.Match(
    success => Results.Ok(success),
    (code, message) => Results.BadRequest(new { code, message })
  );
});


app.Run();

