namespace SolarPlantDemo.Models.DTOs.Response;

#pragma warning disable CS8618
public class PowerPlantResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public double InstalledPower { get; init; }
    public DateTime InstallationDate { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public DateTime CreatedAt { get; init; }
}