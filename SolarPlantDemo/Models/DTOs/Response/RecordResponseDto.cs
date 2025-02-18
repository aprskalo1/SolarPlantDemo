using SolarPlantDemo.Models.Entities;

namespace SolarPlantDemo.Models.DTOs.Response;

#pragma warning disable CS8618
public class RecordResponseDto
{
    public Guid Id { get; init; }

    public Guid PowerPlantId { get; init; }
    public PowerPlant PowerPlant { get; init; }

    public double PowerGenerated { get; init; }
    public DateTime RecordedAt { get; init; }
}