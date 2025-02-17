using System.ComponentModel.DataAnnotations;

namespace SolarPlantDemo.Models.DTOs.Request;

public class UserRegisterRequestDto
{
    [MaxLength(50)] public required string Username { get; init; }
    [MaxLength(50)] public string? FirstName { get; init; }
    [MaxLength(50)] public string? LastName { get; init; }
    [MaxLength(50)] [EmailAddress] public required string Email { get; init; }
    [MaxLength(50)] public string? PhoneNumber { get; init; }
    [MaxLength(250)] public required string Password { get; init; }
    [MaxLength(250)] public required string ConfirmPassword { get; init; }
}