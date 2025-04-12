using System.Net;
using Chaski.Application.Common.Estension;

namespace Chaski.Application.Common;

public class Result<T>
{
    public T? Data { get; private set; }
    public bool IsSuccess { get; private set; }
    public HttpStatusCode StatusCode { get; private set; }
    public string Message { get; private set; }
    public List<string> Errors { get; private set; }

    private Result(T? data, bool isSuccess, HttpStatusCode statusCode, string message, List<string> errors)
    {
        Data = data;
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Message = message;
        Errors = errors;
    }

    public static Result<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK) =>
        new(data, true, statusCode, statusCode.GetMessage(), new());

    public static Result<T> Failure(List<string> errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest) =>
        new(default, false, statusCode, statusCode.GetMessage(), errors);

    public static Result<T> Failure(string error, HttpStatusCode statusCode = HttpStatusCode.BadRequest) =>
        new(default, false, statusCode, statusCode.GetMessage(), new List<string> { error });

    public static Result<T> FailureFromException(Exception ex, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) =>
        new(default, false, statusCode, statusCode.GetMessage(), new List<string> { ex.Message });
}