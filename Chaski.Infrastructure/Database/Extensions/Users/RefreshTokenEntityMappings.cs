using Chaski.Domain.Entities.Users;
using Chaski.Infrastructure.Database.Entities.Users;

namespace Chaski.Infrastructure.Database.Extensions.Users;

public static class RefreshTokenEntityMappings
{
    public static RefreshTokenEntity ToEntity(this RefreshToken refreshToken)
    {
        return new RefreshTokenEntity
        {
            Id = refreshToken.Id,
            Token = refreshToken.Token,
            Expires = refreshToken.Expires,
            Created = refreshToken.Created,
            CreatedByIp = refreshToken.CreatedByIp,
            Revoked = refreshToken.Revoked,
            RevokedByIp = refreshToken.RevokedByIp,
            ReplacedByToken = refreshToken.ReplacedByToken,
            ReasonRevoked = refreshToken.ReasonRevoked,
            UserId = refreshToken.UserId
        };
    }

    public static RefreshToken ToDomain(this RefreshTokenEntity entity)
    {
        return new RefreshToken(
            id: entity.Id,
            token: entity.Token,
            expires:entity.Expires,
            created:entity.Created,
            createdByIp: entity.CreatedByIp,
            revoked: entity.Revoked,
            revokedByIp: entity.RevokedByIp,
            replacedByToken: entity.ReplacedByToken,
            reasonRevoked: entity.ReasonRevoked,
            userId: entity.UserId
        );
    }
}