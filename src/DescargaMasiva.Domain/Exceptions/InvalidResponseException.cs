namespace DescargaMasiva.DescargaMasiva.Domain.Exceptions;

public sealed class InvalidResponseContentException(string message, string content)
  : Exception($"{DefaultMessage} Message: {message} Content: {content}")
{
  private static readonly string DefaultMessage = "La respuesta no tiene un formato válido.";

  public string Content { get; } = content;
}