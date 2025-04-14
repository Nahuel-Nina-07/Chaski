using System.Net;
using Chaski.Application.Common;
using Chaski.Application.Common.Constants;
using Chaski.Application.Common.Helpers;
using Chaski.Application.Dtos.Auth;
using Chaski.Domain.Enums;
using Chaski.Domain.Repositories.Users;
using Chaski.Domain.Security;

namespace Chaski.Application.Services.Auth;

public class AuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepo, IPasswordHasher hasher, ITokenService tokenService)
    {
        _userRepo = userRepo;
        _hasher = hasher;
        _tokenService = tokenService;
    }
    
    public async Task<Result<string>> LoginAsync(LoginDto dto)
    {
        var user = await _userRepo.GetByUsernameAsync(dto.Username);
    
        if (user == null || !_hasher.VerifyPassword(user.PasswordHash, dto.Password))
        {
            return Result<string>.Failure(
                AuthErrorMessages.InvalidCredentials, 
                HttpStatusCode.Unauthorized);
        }
    
        if (!user.IsEmailConfirmed)
        {
            return Result<string>.Failure(
                AuthErrorMessages.EmailNotConfirmed, 
                HttpStatusCode.Forbidden);
        }
    
        if (user.Status != UserStatus.Active)
        {
            return Result<string>.Failure(
                AuthErrorMessages.AccountNotActive, 
                HttpStatusCode.Forbidden);
        }
    
        var roles = new List<string> { "Admin" }; // Obtener roles reales de la base de datos
        var token = _tokenService.GenerateToken(user, roles);
    
        return Result<string>.Success(token, HttpStatusCode.OK);
    }
}