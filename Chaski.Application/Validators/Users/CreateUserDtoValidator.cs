using Chaski.Application.Dtos.Users;
using Chaski.Domain.Repositories.Users;
using FluentValidation;

namespace Chaski.Application.Validators.Users;

public class CreateUserDtoValidator: AbstractValidator<CreateUserDto>
{
    private readonly IUserRepository _userRepository;
    
    public CreateUserDtoValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es requerido.")
            .MaximumLength(50).WithMessage("El nombre de usuario no puede superar los 50 caracteres.")
            .MustAsync(UsernameAvailable).WithMessage("El nombre de usuario ya está en uso.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El correo electrónico no es válido.")
            .MustAsync(EmailAvailable).WithMessage("El correo electrónico ya está registrado.");

        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("Debe tener al menos 8 caracteres.")
            .Matches("[A-Z]").WithMessage("Debe tener al menos una mayúscula.")
            .Matches("[a-z]").WithMessage("Debe tener al menos una minúscula.")
            .Matches("[0-9]").WithMessage("Debe tener al menos un número.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Debe tener al menos un carácter especial.");
    }

    private async Task<bool> UsernameAvailable(string username, CancellationToken cancellationToken)
    {
        var userEntity = await _userRepository.GetByUsernameAsync(username);
        return userEntity == null;
    }

    private async Task<bool> EmailAvailable(string email, CancellationToken cancellationToken)
    {
        var userEntity = await _userRepository.GetByEmailAsync(email);
        return userEntity == null;
    }
}