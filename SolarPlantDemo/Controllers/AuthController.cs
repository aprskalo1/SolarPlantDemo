using Microsoft.AspNetCore.Mvc;
using SolarPlantDemo.Models.DTOs.Request;
using SolarPlantDemo.Services.Auth;

namespace SolarPlantDemo.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
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
}