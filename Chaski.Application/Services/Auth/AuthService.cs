using System.Net;
using Chaski.Application.Common;
using Chaski.Application.Common.Constants;
using Chaski.Application.Common.Helpers;
using Chaski.Application.Dtos.Auth;
using Chaski.Application.Services.Email;
using Chaski.Domain.Entities.Users;
using Chaski.Domain.Enums;
using Chaski.Domain.Repositories.Users;
using Chaski.Domain.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Chaski.Application.Services.Auth;

public class AuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _config;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepo, 
        IPasswordHasher hasher, 
        ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration config,
        IRefreshTokenService refreshTokenService,
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IEmailService emailService,
        IConfiguration configuration
        )
    {
        _userRepo = userRepo;
        _hasher = hasher;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
        _config = config;
        _refreshTokenService = refreshTokenService;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _emailService = emailService;
        _configuration = configuration;
    }
    
    public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto dto)
    {
        // Validar usuario y credenciales
        var user = await _userRepo.GetByUsernameAsync(dto.Username);
        if (user == null || !_hasher.VerifyPassword(user.PasswordHash, dto.Password))
        {
            await Task.Delay(Random.Shared.Next(100, 500)); // Prevenir timing attacks
            return Result<LoginResponseDto>.Failure("Credenciales inválidas", HttpStatusCode.Unauthorized);
        }

        // Validar estado del usuario
        if (!user.IsEmailConfirmed)
            return Result<LoginResponseDto>.Failure("Por favor confirma tu correo electrónico", HttpStatusCode.Forbidden);

        if (user.Status != UserStatus.Active)
            return Result<LoginResponseDto>.Failure("Tu cuenta no está activa", HttpStatusCode.Forbidden);

        // Obtener roles del usuario
        var roles = await _userRepo.GetUserRolesAsync(user.Id);
        
        // Generar tokens
        var accessToken = _tokenService.GenerateAccessToken(user, roles);
        var ipAddress = GetIpAddress();
        var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id, ipAddress);
        var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_config.GetValue<double>("Jwt:AccessTokenExpirationInMinutes"));

        if (DateTime.UtcNow.Minute % 10 == 0)
        {
            await _refreshTokenService.CleanExpiredRefreshTokensAsync();
        }

        return Result<LoginResponseDto>.Success(
            new LoginResponseDto(
                AccessToken: accessToken,
                RefreshToken: refreshToken.Token,
                AccessTokenExpiration: accessTokenExpiration,
                RefreshTokenExpiration: refreshToken.Expires,
                UserEmail: user.Email,
                Username: user.Username,
                Roles: roles
            ),
            HttpStatusCode.OK);
    }
    
    public async Task<Result<RefreshTokenResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal?.Identity?.Name == null)
            return Result<RefreshTokenResponseDto>.Failure("Token inválido", HttpStatusCode.Unauthorized);

        var userId = int.Parse(principal.Identity.Name);
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null)
            return Result<RefreshTokenResponseDto>.Failure("Usuario no encontrado", HttpStatusCode.NotFound);

        var isValid = await _refreshTokenService.IsRefreshTokenValid(request.RefreshToken, userId);
        if (!isValid)
            return Result<RefreshTokenResponseDto>.Failure("Refresh token inválido", HttpStatusCode.Unauthorized);

        var ipAddress = GetIpAddress();
        var roles = await _userRepo.GetUserRolesAsync(user.Id);
        var newAccessToken = _tokenService.GenerateAccessToken(user, roles);
        var newRefreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id, ipAddress);
        var expiration = DateTime.UtcNow.AddMinutes(_config.GetValue<double>("Jwt:AccessTokenExpirationInMinutes"));

        await _refreshTokenService.RotateRefreshTokenAsync(
            request.RefreshToken,
            userId,
            ipAddress,
            newRefreshToken.Token);

        return Result<RefreshTokenResponseDto>.Success(
            new RefreshTokenResponseDto(
                AccessToken: newAccessToken,
                RefreshToken: newRefreshToken.Token,
                AccessTokenExpiration: expiration,
                RefreshTokenExpiration: newRefreshToken.Expires
            ),
            HttpStatusCode.OK);
    }

    public async Task<Result<bool>> RevokeTokenAsync(RevokeTokenRequestDto request)
    {
        var token = await _refreshTokenService.GetRefreshTokenAsync(request.RefreshToken);
        if (token == null || token.IsRevoked)
            return Result<bool>.Failure("Token no encontrado", HttpStatusCode.NotFound);

        var ipAddress = GetIpAddress();
        await _refreshTokenService.RevokeRefreshTokenAsync(
            request.RefreshToken,
            ipAddress,
            "Revocado manualmente");

        return Result<bool>.Success(true, HttpStatusCode.NoContent);
    }

    public async Task<Result<bool>> RevokeAllTokensAsync(int userId)
    {
        var activeTokens = await _refreshTokenService.GetActiveRefreshTokensAsync(userId);
        var count = activeTokens.Count;
    
        var ipAddress = GetIpAddress();
        await _refreshTokenService.RevokeAllRefreshTokensForUserAsync(
            userId,
            ipAddress,
            $"Revocados {count} tokens activos");

        return Result<bool>.Success(true, HttpStatusCode.NoContent);
    }

    public async Task<Result<List<ActiveRefreshTokenDto>>> GetActiveRefreshTokensAsync(int userId)
    {
        try
        {
            // Usar el servicio en lugar del repositorio directamente
            var tokens = await _refreshTokenService.GetActiveRefreshTokensAsync(userId);
        
            var dtos = tokens.Select(t => new ActiveRefreshTokenDto(
                t.Token,
                t.Created,
                t.Expires,
                t.CreatedByIp
            )).ToList();

            return Result<List<ActiveRefreshTokenDto>>.Success(dtos, HttpStatusCode.OK);
        }
        catch (Exception ex)
        {
            // Loggear el error si es necesario
            return Result<List<ActiveRefreshTokenDto>>.Failure(
                $"Error al obtener tokens activos: {ex.Message}", 
                HttpStatusCode.InternalServerError);
        }
    }

    private string GetIpAddress()
    {
        return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "IP desconocida";
    }
    
    public async Task<Result<bool>> ForgotPasswordAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || user.Status != UserStatus.Active)
            return Result<bool>.Success(true); // No revelar si el usuario existe

        var token = Guid.NewGuid().ToString();
        user.GeneratePasswordResetToken(token, DateTime.UtcNow.AddHours(1), _passwordHasher);
        await _userRepository.UpdateAsync(user);

        var resetLink = $"{_configuration["PasswordReset:BaseUrl"]}?token={token}&email={Uri.EscapeDataString(email)}";
        await _emailService.SendPasswordResetEmailAsync(email, user.Username, resetLink);

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> ResetPasswordAsync(ResetPasswordDto dto)
    {
        if (dto.NewPassword != dto.ConfirmPassword)
            return Result<bool>.Failure("Las contraseñas no coinciden", HttpStatusCode.BadRequest);

        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null)
            return Result<bool>.Failure("Operación inválida", HttpStatusCode.BadRequest);

        if (!user.VerifyPasswordResetToken(dto.Token, _passwordHasher))
            return Result<bool>.Failure("Token inválido o expirado", HttpStatusCode.BadRequest);

        if (_passwordHasher.VerifyPassword(user.PasswordHash, dto.NewPassword))
            return Result<bool>.Failure("No puedes usar la misma contraseña", HttpStatusCode.BadRequest);

        user.UpdatePassword(_passwordHasher.HashPassword(dto.NewPassword));
        await _userRepository.UpdateAsync(user);

        return Result<bool>.Success(true);
    }
}