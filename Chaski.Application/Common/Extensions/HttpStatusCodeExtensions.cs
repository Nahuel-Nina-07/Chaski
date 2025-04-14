using System.Net;

namespace Chaski.Application.Common.Extensions;

public static class HttpStatusCodeExtensions
{
    public static string GetMessage(this HttpStatusCode statusCode) =>
        HttpStatusMessages.GetMessage((int)statusCode);
}