using System.Security.Claims;
using Chaski.Domain.Entities.Users;

namespace Chaski.Domain.Security;

public interface ITokenService
{
    string GenerateToken(User user, IList<string> roles);

}