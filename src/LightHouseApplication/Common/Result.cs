namespace LightHouseApplication.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? ErrorMessage { get; }

    private Result(bool isSuccess, T? value = default, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Ok(T value) => new(true, value);
    public static Result<T> Fail(string errorMessage) => new(false, default, errorMessage);
}

public class Result
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }

    protected Result(bool isSuccess, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static Result Ok() => new(true);
    public static Result Fail(string errorMessage) => new(false, errorMessage);
}
