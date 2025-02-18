using System.ComponentModel.DataAnnotations;

namespace SolarPlantDemo.Models.Entities;

public class PlantRecord
{
    [Key] public Guid Id { get; init; }

    public Guid PowerPlantId { get; init; }
    public PowerPlant PowerPlant { get; init; } = null!;

    public double PowerGenerated { get; init; }
    public DateTime RecordedAt { get; init; } = DateTime.Now;
}