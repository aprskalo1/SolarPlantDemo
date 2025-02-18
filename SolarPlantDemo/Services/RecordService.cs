using AutoMapper;
using SolarPlantDemo.Exceptions;
using SolarPlantDemo.Models.DTOs.Response;
using SolarPlantDemo.Models.Entities;
using SolarPlantDemo.Repositories;

namespace SolarPlantDemo.Services;

public interface IRecordService
{
    Task<IEnumerable<RecordResponseDto>> GetRecordsByTimespanAsync(Guid plantId, DateTime start, DateTime end, int granularity);
}

internal class RecordService(IRecordRepository recordRepository, IMapper mapper) : IRecordService
{
    public async Task<IEnumerable<RecordResponseDto>> GetRecordsByTimespanAsync(Guid plantId, DateTime start, DateTime end, int granularity)
    {
        if (granularity % 15 != 0)
        {
            throw new RecordGranularityException($"Granularity must be a multiple of 15. Provided granularity: {granularity}");
        }

        var records = (await recordRepository.GetRecordsByTimespanAsync(plantId, start, end)).ToList();
        if (granularity == 15)
        {
            return mapper.Map<IEnumerable<RecordResponseDto>>(records);
        }

        var recordsPerGroup = granularity / 15;
        var aggregatedRecords = records
            .Select((record, index) => new { record, index })
            .GroupBy(x => x.index / recordsPerGroup)
            .Select(g => AggregateGroup(g.Select(x => x.record)))
            .ToList();
        return mapper.Map<IEnumerable<RecordResponseDto>>(aggregatedRecords);
    }

    private static PlantRecord AggregateGroup(IEnumerable<PlantRecord> group)
    {
        var groupList = group.ToList();
        return new PlantRecord
        {
            Id = Guid.NewGuid(),
            PowerPlantId = groupList.First().PowerPlantId,
            PowerGenerated = groupList.Sum(r => r.PowerGenerated),
            RecordedAt = groupList.First().RecordedAt
        };
    }
}