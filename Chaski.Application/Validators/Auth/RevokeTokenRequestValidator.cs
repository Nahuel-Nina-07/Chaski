using Chaski.Application.Dtos.Auth;
using FluentValidation;

namespace Chaski.Application.Validators.Auth;

public class RevokeTokenRequestValidator: AbstractValidator<RevokeTokenRequestDto>
{
    public RevokeTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("El refresh token es requerido");
    }
}