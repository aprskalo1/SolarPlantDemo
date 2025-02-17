using AutoMapper;
using SolarPlantDemo.Exceptions;
using SolarPlantDemo.Models.Common;
using SolarPlantDemo.Models.DTOs.Request;
using SolarPlantDemo.Models.DTOs.Response;
using SolarPlantDemo.Models.Entities;
using SolarPlantDemo.Repositories;
using SolarPlantDemo.Utils;

namespace SolarPlantDemo.Services.Auth;

public interface IAuthService
{
    Task<UserResponseDto> RegisterUserAsync(UserRegisterRequestDto userRegisterRequestDto);
    Task<TokensResponse> LoginUserAsync(UserLoginRequestDto userLoginRequestDto);
}

internal class AuthService(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher, ITokenService tokenService) : IAuthService
{
    public async Task<UserResponseDto> RegisterUserAsync(UserRegisterRequestDto userRegisterRequestDto)
    {
        if (userRegisterRequestDto.Password != userRegisterRequestDto.ConfirmPassword)
            throw new UserCreationException("Passwords do not match.");

        if (await userRepository.GetUserByUsernameAsync(userRegisterRequestDto.Username) != null)
            throw new UserCreationException($"User with username {userRegisterRequestDto.Username} already exists.");

        var user = mapper.Map<User>(userRegisterRequestDto);
        user.PasswordHash = passwordHasher.HashPassword(userRegisterRequestDto.Password);

        await userRepository.CreateUserAsync(user);
        await userRepository.SaveChangesAsync();

        return mapper.Map<UserResponseDto>(user);
    }

    public async Task<TokensResponse> LoginUserAsync(UserLoginRequestDto userLoginRequestDto)
    {
        var user = await userRepository.GetUserByUsernameAsync(userLoginRequestDto.Username);

        if (user == null)
            throw new UserLoginException($"User with username {userLoginRequestDto.Username} does not exist.");

        if (!passwordHasher.VerifyPassword(userLoginRequestDto.Password, user.PasswordHash))
            throw new UserLoginException("Invalid password.");

        var accessToken = tokenService.GenerateAccessToken(user);
        var refreshToken = await tokenService.GenerateRefreshToken(user.Id);

        return new TokensResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}