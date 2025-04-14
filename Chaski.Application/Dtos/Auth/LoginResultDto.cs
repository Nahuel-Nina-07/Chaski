namespace Chaski.Application.Dtos.Auth;

public record LoginResultDto
    (string AccessToken, string RefreshToken);