using Chaski.Application.Dtos.Auth;
using Chaski.Application.Dtos.Users;
using Chaski.Application.Validators.Auth;
using Chaski.Application.Validators.Users;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Chaski.Infrastructure.DependencyInjection;

public static class ChaskiValidatorDependencyInjection
{
    public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
    {
        // Validadores específicos
        services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
        services.AddScoped<IValidator<UserDto>, UpdateUserDtoValidator>();
        services.AddScoped<IValidator<RefreshTokenRequestDto>, RefreshTokenRequestValidator>();
        services.AddScoped<IValidator<RevokeTokenRequestDto>, RevokeTokenRequestValidator>();
        
        // Configuración global de FluentValidation
        ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) => memberInfo?.Name;
        
        return services;
    }
}