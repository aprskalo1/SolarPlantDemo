namespace SolarPlantDemo.Models.DTOs.Response;

#pragma warning disable CS8618
public class UserResponseDto
{
    public Guid Id { get; init; }
    public string Username { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public DateTime CreatedAt { get; init; }
}