using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SolarPlantDemo.Models.DTOs.Request;
using SolarPlantDemo.Services.Auth;

namespace SolarPlantDemo.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService, ITokenService tokenService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync(UserRegisterRequestDto userRegisterRequestDto)
    {
        var user = await authService.RegisterUserAsync(userRegisterRequestDto);
        return Created($"api/auth/{user.Id}", user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync(UserLoginRequestDto userLoginRequestDto)
    {
        var tokens = await authService.LoginUserAsync(userLoginRequestDto);
        return Ok(tokens);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAccessTokenAsync(string refreshToken)
    {
        var userId = GetUserIdFromClaims();
        if (userId == null)
        {
            return Unauthorized();
        }

        var tokens = await tokenService.RefreshAccessToken(refreshToken, userId.Value);
        return Ok(tokens);
    }

    private Guid? GetUserIdFromClaims()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return null;
        }

        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}