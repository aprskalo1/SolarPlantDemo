using AutoMapper;
using SolarPlantDemo.Exceptions;
using SolarPlantDemo.Integrations;
using SolarPlantDemo.Models.DTOs.Response;
using SolarPlantDemo.Models.Entities;
using SolarPlantDemo.Models.Enum;
using SolarPlantDemo.Repositories;

namespace SolarPlantDemo.Services;

public interface IRecordService
{
    Task<IEnumerable<RecordResponseDto>> GetRecordsByTimespanAsync(Guid plantId, DateTime start, DateTime end, int granularity, TimeseriesType timeseriesType);
}

internal class RecordService(
    IRecordRepository recordRepository,
    IMapper mapper,
    IWeatherForecastService weatherForecastService,
    IPlantRepository plantRepository) : IRecordService
{
    public async Task<IEnumerable<RecordResponseDto>> GetRecordsByTimespanAsync(Guid plantId, DateTime start, DateTime end, int granularity,
        TimeseriesType timeseriesType)
    {
        if (granularity % 15 != 0)
            throw new RecordGranularityException($"Granularity must be a multiple of 15. Provided granularity: {granularity}");

        switch (timeseriesType)
        {
            case TimeseriesType.Production:
            {
                var records = (await recordRepository.GetRecordsByTimespanAsync(plantId, start, end)).ToList();
                if (granularity == 15)
                    return mapper.Map<IEnumerable<RecordResponseDto>>(records);
                var recordsPerGroup = granularity / 15;
                var aggregatedRecords = records
                    .Select((record, index) => new { record, index })
                    .GroupBy(x => x.index / recordsPerGroup)
                    .Select(g => AggregateGroup(g.Select(x => x.record)))
                    .ToList();
                return mapper.Map<IEnumerable<RecordResponseDto>>(aggregatedRecords);
            }
            case TimeseriesType.Forecast:
            {
                var plant = await plantRepository.GetPlantByIdAsync(plantId);
                if (plant is null)
                    throw new PlantNotFoundException($"Plant with id {plantId} not found");

                var weatherForecast = await weatherForecastService.GetWeatherForecastAsync(plant.Latitude, plant.Longitude);
                var forecastRecords = new List<PlantRecord>();
                var weatherCodes = weatherForecast.WeatherData.ToList();
                for (var i = 0; i < weatherCodes.Count; i++)
                {
                    var forecastDate = weatherForecast.FromDate.AddDays(i);
                    var forecastedPower = ForecastPower(plant.InstalledPower, weatherCodes[i]);
                    forecastRecords.Add(new PlantRecord
                    {
                        Id = Guid.NewGuid(),
                        PowerPlantId = plant.Id,
                        RecordedAt = forecastDate,
                        PowerGenerated = forecastedPower
                    });
                }

                if (granularity == 15)
                    return mapper.Map<IEnumerable<RecordResponseDto>>(forecastRecords);
                var recordsPerGroup = granularity / 15;
                var aggregatedForecastRecords = forecastRecords
                    .Select((record, index) => new { record, index })
                    .GroupBy(x => x.index / recordsPerGroup)
                    .Select(g => AggregateGroup(g.Select(x => x.record)))
                    .ToList();
                return mapper.Map<IEnumerable<RecordResponseDto>>(aggregatedForecastRecords);
            }
            default:
                throw new ArgumentException("Invalid timeseries type");
        }
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

    private static double ForecastPower(double installedPower, int weatherCode)
    {
        var factor = weatherCode switch
        {
            0 => 1.0,
            1 => 0.9,
            2 => 0.9,
            3 => 0.9,
            _ => 0.8
        };
        return installedPower * factor;
    }
}