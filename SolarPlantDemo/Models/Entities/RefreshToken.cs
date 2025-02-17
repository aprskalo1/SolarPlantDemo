using System.ComponentModel.DataAnnotations;

namespace SolarPlantDemo.Models.Entities;

public class RefreshToken
{
    [Key] public Guid Id { get; init; }

    public Guid UserId { get; init; }
    public User User { get; init; } = null!;

    [MaxLength(500)] public required string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.Now;
}