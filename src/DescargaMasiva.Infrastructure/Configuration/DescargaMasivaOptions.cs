namespace DescargaMasiva.DescargaMasiva.Infrastructure.Configuration;

public sealed class DescargaMasivaOptions
{
  public string CertificatePath { get; set; } = default!;
  public string CertificatePassword { get; set; } = default!;
}