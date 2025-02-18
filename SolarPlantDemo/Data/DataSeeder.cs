using SolarPlantDemo.Models.Entities;

namespace SolarPlantDemo.Data;

public class DataSeeder
{
    public static async Task SeedDataAsync(SolarPlantDbContext context)
    {
        if (context.PowerPlants.Any())
        {
            return;
        }

        var random = new Random();
        var now = DateTime.Now;

        for (var i = 1; i <= 3; i++)
        {
            var powerPlant = new PowerPlant
            {
                Id = Guid.NewGuid(),
                Name = $"Solar Plant {i}",
                InstalledPower = random.NextDouble() * 100 + 50,
                InstallationDate = now.AddDays(-random.Next(100, 500)),
                Latitude = random.NextDouble() * 180 - 90,
                Longitude = random.NextDouble() * 360 - 180,
                CreatedAt = now
            };

            context.PowerPlants.Add(powerPlant);

            for (var j = 0; j < 25; j++)
            {
                var record = new PlantRecord
                {
                    Id = Guid.NewGuid(),
                    PowerPlantId = powerPlant.Id,
                    PowerPlant = powerPlant,
                    PowerGenerated = random.NextDouble() * powerPlant.InstalledPower,
                    RecordedAt = now.AddMinutes(-j * 15)
                };

                context.PlantRecords.Add(record);
            }
        }

        await context.SaveChangesAsync();
    }
}