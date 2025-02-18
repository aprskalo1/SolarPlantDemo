using System.ComponentModel.DataAnnotations;

namespace SolarPlantDemo.Models.Entities;

public class PowerPlant
{
    [Key] public Guid Id { get; init; }
    [MaxLength(50)] public string? Name { get; init; }
    public required double InstalledPower { get; init; }
    public required DateTime InstallationDate { get; init; }
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.Now;
}