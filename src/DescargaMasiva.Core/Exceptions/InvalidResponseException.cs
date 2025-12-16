namespace DescargaMasiva.DescargaMasiva.Core.Exceptions;

public sealed class InvalidResponseContentException(string message, string content)
  : Exception($"{DefaultMessage} Message: {message} Content: {content}")
{
  private static readonly string DefaultMessage = "Response content is not in a valid format.";

  public string Content { get; } = content;
}