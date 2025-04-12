using Chaski.Application.Dtos;
using Chaski.Domain.Repositories.Users;
using FluentValidation;

namespace Chaski.Application.Validators.Users;

public class UserDtoValidator: AbstractValidator<UserDto>
{
    private readonly IUserRepository _userRepository;
    public UserDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es requerido.")
            .MaximumLength(50).WithMessage("El nombre de usuario no puede superar los 50 caracteres.")
            .MustAsync(UsernameAvailable).WithMessage("El nombre de usuario ya está en uso.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El correo electrónico no es válido.");

        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("El estado del usuario no es válido.");
    }
    private async Task<bool> UsernameAvailable(string username, CancellationToken cancellationToken)
    {
        var userEntity = await _userRepository.GetByUsernameAsync(username);
        return userEntity == null;
    }
}