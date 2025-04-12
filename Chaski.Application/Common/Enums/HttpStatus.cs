namespace Chaski.Application.Common.Enums;

public enum HttpStatus
{
    OK = 200,
    Created = 201,
    Accepted = 202,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    InternalServerError = 500,
    ServiceUnavailable = 503,
    GatewayTimeout = 504
}