using Microsoft.EntityFrameworkCore;
using SolarPlantDemo.Data;
using SolarPlantDemo.Models.Entities;

namespace SolarPlantDemo.Repositories;

public interface IPlantRepository
{
    Task<PowerPlant> CreatePlantAsync(PowerPlant plant);
    Task<PowerPlant?> GetPlantByIdAsync(Guid id);
    Task<PowerPlant?> GetPlantByNameAsync(string plantName);
    Task<IEnumerable<PowerPlant>?> GetPlantsAsync();
    Task UpdatePlantAsync(PowerPlant plant);
    Task DeletePlantAsync(PowerPlant plant);
    Task SaveChangesAsync();
}

internal class PlantRepository(SolarPlantDbContext dbContext) : IPlantRepository
{
    public async Task<PowerPlant> CreatePlantAsync(PowerPlant plant)
    {
        await dbContext.PowerPlants.AddAsync(plant);
        return plant;
    }

    public async Task<PowerPlant?> GetPlantByIdAsync(Guid id)
    {
        return await dbContext.PowerPlants.FindAsync(id);
    }

    public async Task<PowerPlant?> GetPlantByNameAsync(string plantName)
    {
        return await dbContext.PowerPlants.FirstOrDefaultAsync(p => p.Name == plantName);
    }

    public async Task<IEnumerable<PowerPlant>?> GetPlantsAsync()
    {
        return await dbContext.PowerPlants.ToListAsync();
    }

    public async Task UpdatePlantAsync(PowerPlant plant)
    {
        dbContext.PowerPlants.Update(plant);
        await SaveChangesAsync();
    }

    public async Task DeletePlantAsync(PowerPlant plant)
    {
        dbContext.PowerPlants.Remove(plant);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}