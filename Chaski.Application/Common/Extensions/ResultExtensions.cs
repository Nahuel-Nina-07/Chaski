using Chaski.Application.Common.Helpers;

namespace Chaski.Application.Common.Extensions;

public static class ResultExtensions
{
    public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
    {
        if (result.IsSuccess)
            action(result.Data!);

        return result;
    }

    public static Result<T> OnFailure<T>(this Result<T> result, Action<List<string>> action)
    {
        if (!result.IsSuccess)
            action(result.Errors);

        return result;
    }

    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapFunc) =>
        result.IsSuccess
            ? Result<TOut>.Success(mapFunc(result.Data!), result.StatusCode)
            : Result<TOut>.Failure(result.Errors, result.StatusCode);

    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(this Result<TIn> result, Func<TIn, Task<TOut>> mapFunc) =>
        result.IsSuccess
            ? Result<TOut>.Success(await mapFunc(result.Data!), result.StatusCode)
            : Result<TOut>.Failure(result.Errors, result.StatusCode);

    public static string GetFirstError<T>(this Result<T> result) =>
        result.Errors?.FirstOrDefault() ?? "Unspecified error.";

    public static bool IsSuccessful<T>(this Result<T> result) => result.IsSuccess;
    public static bool IsFailure<T>(this Result<T> result) => !result.IsSuccess;
}