namespace DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

public interface IPfxGenerator
{
  byte[] GenerarPfx(
    string cert,
    string key,
    string password
  );
}