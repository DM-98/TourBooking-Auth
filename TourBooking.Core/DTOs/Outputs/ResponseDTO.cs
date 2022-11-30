namespace TourBooking.Core.DTOs.Outputs;

public sealed record ResponseDTO<T> where T : class
{
    public bool IsSuccess { get; set; }

    public T? Content { get; set; }

    public string? ErrorMessage { get; set; }

    public string? ExceptionMessage { get; set; }

    public string? StackTrace { get; set; }

    public Enum? ErrorType { get; set; }

    public ResponseDTO(bool isSuccess, string? errorMessage = null, Enum? errorType = null, string? exceptionMessage = null, string? stackTrace = null, T? content = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        ExceptionMessage = exceptionMessage;
        StackTrace = stackTrace;
        ErrorType = errorType;
        Content = content;
    }
}