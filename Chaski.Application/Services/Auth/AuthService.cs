using System.Net;
using Chaski.Application.Common;
using Chaski.Application.Common.Helpers;
using Chaski.Application.Dtos.Auth;
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
            return Result<string>.Failure("Credenciales inválidas", HttpStatusCode.Unauthorized);
        }
        
        var roles = new List<string> { "Admin" };
        
        var token = _tokenService.GenerateToken(user, roles);
        return Result<string>.Success(token, HttpStatusCode.OK);
    }
}