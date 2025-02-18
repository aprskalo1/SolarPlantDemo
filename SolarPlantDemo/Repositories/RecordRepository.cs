using Microsoft.EntityFrameworkCore;
using SolarPlantDemo.Data;
using SolarPlantDemo.Models.Entities;

namespace SolarPlantDemo.Repositories;

public interface IRecordRepository
{
    Task<IEnumerable<PlantRecord>> GetRecordsByTimespanAsync(Guid plantId, DateTime start, DateTime end);
}

internal class RecordRepository(SolarPlantDbContext dbContext) : IRecordRepository
{
    public async Task<IEnumerable<PlantRecord>> GetRecordsByTimespanAsync(Guid plantId, DateTime start, DateTime end)
    {
        return await dbContext.PlantRecords
            .Where(record => record.PowerPlantId == plantId && record.RecordedAt >= start && record.RecordedAt <= end)
            .OrderBy(record => record.RecordedAt)
            .ToListAsync();
    }
}