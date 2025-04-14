using Chaski.Application.Dtos.Users;
using Chaski.Domain.Repositories.Users;
using FluentValidation;

namespace Chaski.Application.Validators.Users;

public class UserDtoValidator: AbstractValidator<UserDto>
{
    private readonly IUserRepository _userRepository;
    public UserDtoValidator(
        IUserRepository userRepository
        )
    {
        _userRepository = userRepository;
        
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es requerido.")
            .MaximumLength(50).WithMessage("El nombre de usuario no puede superar los 50 caracteres.")
            .MustAsync(UsernameAvailable).WithMessage("El nombre de usuario ya está en uso.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El correo electrónico no es válido.");

        RuleFor(x => x.PasswordHash)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().When(x => x.Id == 0).WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).When(x => !string.IsNullOrWhiteSpace(x.PasswordHash)).WithMessage("Debe tener al menos 8 caracteres.")
            .Matches("[A-Z]").When(x => !string.IsNullOrWhiteSpace(x.PasswordHash)).WithMessage("Debe tener al menos una mayúscula.")
            .Matches("[a-z]").When(x => !string.IsNullOrWhiteSpace(x.PasswordHash)).WithMessage("Debe tener al menos una minúscula.")
            .Matches("[0-9]").When(x => !string.IsNullOrWhiteSpace(x.PasswordHash)).WithMessage("Debe tener al menos un número.")
            .Matches("[^a-zA-Z0-9]").When(x => !string.IsNullOrWhiteSpace(x.PasswordHash)).WithMessage("Debe tener al menos un carácter especial.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("El estado del usuario no es válido.");
    }
    private async Task<bool> UsernameAvailable(string username, CancellationToken cancellationToken)
    {
        var userEntity = await _userRepository.GetByUsernameAsync(username);
        return userEntity == null;
    }
}