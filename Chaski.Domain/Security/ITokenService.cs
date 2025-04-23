using System.Security.Claims;
using Chaski.Domain.Entities.Users;

namespace Chaski.Domain.Security;

public interface ITokenService
{
    string GenerateAccessToken(User user, IList<string> roles);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}