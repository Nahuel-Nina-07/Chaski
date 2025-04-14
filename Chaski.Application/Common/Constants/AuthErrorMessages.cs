namespace Chaski.Application.Common.Constants;

public static class AuthErrorMessages
{
    public const string InvalidCredentials = "Credenciales inválidas";
    public const string EmailNotConfirmed = "Por favor confirma tu correo electrónico antes de iniciar sesión";
    public const string AccountNotActive = "Tu cuenta no está activa. Contacta al administrador.";
    public const string AccountLocked = "Tu cuenta está temporalmente bloqueada. Intenta nuevamente más tarde.";
    public const string TokenExpired = "El token de confirmación ha expirado";
    public const string InvalidToken = "Token de confirmación inválido";
}