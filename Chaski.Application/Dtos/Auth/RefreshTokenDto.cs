namespace Chaski.Application.Dtos.Auth;

// LoginResponseDto.cs
public record LoginResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiration,
    DateTime RefreshTokenExpiration,
    string UserEmail,
    string Username,
    IEnumerable<string> Roles);

// RefreshTokenRequestDto.cs
public record RefreshTokenRequestDto(
    string AccessToken,
    string RefreshToken);

// RefreshTokenResponseDto.cs
public record RefreshTokenResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiration,
    DateTime RefreshTokenExpiration);

// RevokeTokenRequestDto.cs
public record RevokeTokenRequestDto(
    string RefreshToken);

// ActiveRefreshTokenDto.cs
public record ActiveRefreshTokenDto(
    string Token,
    DateTime Created,
    DateTime Expires,
    string CreatedByIp);