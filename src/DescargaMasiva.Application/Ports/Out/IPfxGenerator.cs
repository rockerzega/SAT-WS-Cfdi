namespace DescargaMasiva.DescargaMasiva.Application.Ports.Out;

public interface IPfxGenerator
{
  byte[] GenerarPfx(
    string cert,
    string key,
    string password
  );
}