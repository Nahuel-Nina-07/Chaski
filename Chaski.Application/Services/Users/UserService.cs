using System.Net;
using Chaski.Application.Common;
using Chaski.Application.Common.Constants;
using Chaski.Application.Common.Helpers;
using Chaski.Application.Dtos.Users;
using Chaski.Application.Extensions.Users;
using Chaski.Application.Services.Email;
using Chaski.Domain.Entities.Users;
using Chaski.Domain.Enums;
using Chaski.Domain.Repositories.Users;
using Chaski.Domain.Security;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace Chaski.Application.Services.Users;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<CreateUserDto> _validator;
    private readonly IValidator<UserDto> _updateValidator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IRoleRepository _roleRepository;


    public UserService(
        IUserRepository userRepository, 
        IValidator<CreateUserDto> validator, 
        IValidator<UserDto> updateValidator,
        IPasswordHasher passwordHasher, 
        IEmailService emailService, 
        IConfiguration configuration,
        IUserRoleRepository userRoleRepository,
        IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _validator = validator;
        _updateValidator = updateValidator;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
        _emailService = emailService;
        _userRoleRepository = userRoleRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Result<UserDto>> CreateUserAsync(CreateUserDto createDto)
    {
        var validateModel = await _validator.ValidateAsync(createDto);
        if (!validateModel.IsValid)
            return Result<UserDto>.Failure(validateModel.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
        if (await _userRepository.GetByEmailAsync(createDto.Email) != null)
            return Result<UserDto>.Failure("El correo electrónico ya está registrado", HttpStatusCode.BadRequest);
        var passwordHash = _passwordHasher.HashPassword(createDto.PasswordHash);
        var user = new User(
            0,
            createDto.Username,
            createDto.Email,
            passwordHash,
            UserStatus.PendingEmailConfirmation
        );
        var token = Guid.NewGuid().ToString();
        var expiryDate = DateTime.UtcNow.AddHours(
            _configuration.GetValue<int>("EmailConfirmation:TokenExpiryInHours", 24));
        user.GenerateEmailConfirmationToken(token, expiryDate);
        var createdUser = await _userRepository.CreateAsync(user);

        var userRoleId = await _roleRepository.GetRoleIdByNameAsync("User");
        if (userRoleId.HasValue)
            await _userRoleRepository.AssignRoleAsync(createdUser.Id, userRoleId.Value);
        
        var confirmationBaseUrl = _configuration["EmailConfirmation:BaseUrl"];
        var confirmationLink = $"{confirmationBaseUrl}?token={token}&email={Uri.EscapeDataString(user.Email)}";
        await _emailService.SendEmailConfirmationAsync(
            user.Email, 
            user.Username, 
            confirmationLink);
        return Result<UserDto>.Success(createdUser.ToDto(), HttpStatusCode.Created);
    }
    public async Task<Result<bool>> ConfirmEmailAsync(string token, string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            return Result<bool>.Failure("Usuario no encontrado", HttpStatusCode.NotFound);
        if (user.IsEmailConfirmed)
            return Result<bool>.Success(true, HttpStatusCode.OK);
        if (user.EmailConfirmationToken != token)
        {
            return Result<bool>.Failure(
                AuthErrorMessages.InvalidToken, 
                HttpStatusCode.BadRequest);
        }
        if (user.EmailConfirmationTokenExpiry < DateTime.UtcNow)
        {
            return Result<bool>.Failure(
                AuthErrorMessages.TokenExpired, 
                HttpStatusCode.BadRequest);
        }
        user.ConfirmEmail();
        await _userRepository.UpdateAsync(user);
        return Result<bool>.Success(true, HttpStatusCode.OK);
    }
    public async Task<Result<UserDto>> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return Result<UserDto>.Failure("Usuario no encontrado", HttpStatusCode.NotFound);
        return Result<UserDto>.Success(user.ToDto(), HttpStatusCode.OK);
    }

    public async Task<Result<List<UserDto>>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return Result<List<UserDto>>.Success(users.Select(u => u.ToDto()).ToList(), HttpStatusCode.OK);
    }

    public async Task<Result<UserDto>> UpdateUserAsync(UserDto userDto)
    {
        var validateModel = await _updateValidator.ValidateAsync(userDto);
        if (!validateModel.IsValid)
            return Result<UserDto>.Failure(validateModel.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);
        var user = await _userRepository.GetByIdAsync(userDto.Id);
        if (user == null)
            return Result<UserDto>.Failure("Usuario no encontrado", HttpStatusCode.NotFound);
        user.UpdateStatus(userDto.Status);
        var updatedUser = await _userRepository.UpdateAsync(user);
        return Result<UserDto>.Success(updatedUser.ToDto(), HttpStatusCode.OK);
    }
    
    public async Task<Result<UserDto>> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
            return Result<UserDto>.Failure("Usuario no encontrado", HttpStatusCode.NotFound);
        return Result<UserDto>.Success(user.ToDto(), HttpStatusCode.OK);
    }

    public async Task<Result<UserDto>> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            return Result<UserDto>.Failure("Usuario no encontrado", HttpStatusCode.NotFound);
        return Result<UserDto>.Success(user.ToDto(), HttpStatusCode.OK);
    }
}