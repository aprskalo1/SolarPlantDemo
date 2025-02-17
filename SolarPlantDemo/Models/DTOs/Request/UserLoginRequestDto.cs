namespace SolarPlantDemo.Models.DTOs.Request;

public class UserLoginRequestDto
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}