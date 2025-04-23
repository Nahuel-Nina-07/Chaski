namespace Chaski.Application.Dtos.Auth;

// ForgotPasswordDto.cs
public record ForgotPasswordDto(string Email);

// ResetPasswordDto.cs
public record ResetPasswordDto(
    string Email,
    string Token,
    string NewPassword,
    string ConfirmPassword);