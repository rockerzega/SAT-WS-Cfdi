namespace DescargaMasiva.DescargaMasiva.Infrastructure.Ports;

public interface ISoapEnvelopeBuilder<TRequest>
{
  string Build(TRequest request);
}