using Chaski.Application.Dtos.Users;
using FluentValidation;

namespace Chaski.Application.Validators.Users;

public class UpdateUserDtoValidator : AbstractValidator<UserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El ID debe ser válido");
            
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Estado de usuario no válido");
        
        // Puedes añadir más reglas específicas para actualización
    }
}