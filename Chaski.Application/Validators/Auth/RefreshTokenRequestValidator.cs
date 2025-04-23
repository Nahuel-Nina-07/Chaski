using Chaski.Application.Dtos.Auth;
using FluentValidation;

namespace Chaski.Application.Validators.Auth;

public class RefreshTokenRequestValidator: AbstractValidator<RefreshTokenRequestDto>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("El token de acceso es requerido");
            
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("El refresh token es requerido");
    }
}