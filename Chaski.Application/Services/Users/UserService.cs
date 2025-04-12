using System.Net;
using Chaski.Application.Common;
using Chaski.Application.Dtos;
using Chaski.Application.Extensions.Users;
using Chaski.Domain.Entities.Users;
using Chaski.Domain.Repositories.Users;
using FluentValidation;

namespace Chaski.Application.Services.Users;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<UserDto> _validator;

    public UserService(IUserRepository userRepository, IValidator<UserDto> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<Result<UserDto>> CreateUserAsync(UserDto userDto)
    {
        var validateModel = await _validator.ValidateAsync(userDto);
        if (!validateModel.IsValid)
            return Result<UserDto>.Failure(validateModel.Errors.Select(e => e.ErrorMessage).ToList(), HttpStatusCode.BadRequest);

        var user = new User(
            userDto.Id,
            userDto.Username,
            userDto.Email,
            userDto.PasswordHash,
            userDto.Status
        );

        var createdUser = await _userRepository.CreateAsync(user);

        return Result<UserDto>.Success(createdUser.ToDto(), HttpStatusCode.Created);
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
        var validateModel = await _validator.ValidateAsync(userDto);
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