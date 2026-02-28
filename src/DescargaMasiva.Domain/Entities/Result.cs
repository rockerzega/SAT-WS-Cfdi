namespace DescargaMasiva.DescargaMasiva.Domain.Entities;

public sealed class Result<T>
{
  private Result(bool isSuccess, T? value, string? errorCode, string? errorMessage)
  {
    IsSuccess = isSuccess;
    Value = value;
    ErrorCode = errorCode;
    ErrorMessage = errorMessage;
  }

  public bool IsSuccess { get; }
  public bool IsFailure => !IsSuccess;

  public T? Value { get; }

  public string? ErrorCode { get; }
  public string? ErrorMessage { get; }

  public static Result<T> Success(T value)
    => new(true, value, null, null);

  public static Result<T> Failure(string errorCode, string errorMessage)
    => new(false, default, errorCode, errorMessage);

  // 🔵 MAP
  public Result<TResult> Map<TResult>(Func<T, TResult> mapper)
  {
    if (IsFailure)
      return Result<TResult>.Failure(ErrorCode!, ErrorMessage!);

    return Result<TResult>.Success(mapper(Value!));
  }

  // 🔵 BIND (Async)
  public async Task<Result<TResult>> BindAsync<TResult>(
    Func<T, Task<Result<TResult>>> binder)
  {
    if (IsFailure)
      return Result<TResult>.Failure(ErrorCode!, ErrorMessage!);

    return await binder(Value!);
  }

  // 🔵 MATCH
  public TResult Match<TResult>(
    Func<T, TResult> onSuccess,
    Func<string, string, TResult> onFailure)
  {
    return IsSuccess
      ? onSuccess(Value!)
      : onFailure(ErrorCode!, ErrorMessage!);
  }
}