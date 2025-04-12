using System.Net;

namespace Chaski.Application.Common.Estension;

public static class HttpStatusCodeExtensions
{
    public static string GetMessage(this HttpStatusCode statusCode) =>
        HttpStatusMessages.GetMessage((int)statusCode);
}