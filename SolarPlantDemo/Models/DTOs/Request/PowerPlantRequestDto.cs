using System.ComponentModel.DataAnnotations;

namespace SolarPlantDemo.Models.DTOs.Request;

public class PowerPlantRequestDto
{
    [MaxLength(50)] public string? Name { get; init; }
    public required double InstalledPower { get; init; }
    public required DateTime InstallationDate { get; init; }
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
}