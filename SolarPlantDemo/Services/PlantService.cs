using AutoMapper;
using SolarPlantDemo.Exceptions;
using SolarPlantDemo.Models.DTOs.Request;
using SolarPlantDemo.Models.DTOs.Response;
using SolarPlantDemo.Models.Entities;
using SolarPlantDemo.Repositories;

namespace SolarPlantDemo.Services;

public interface IPlantService
{
    Task<PowerPlantResponseDto> CreatePlantAsync(PowerPlantRequestDto plantRequest);
    Task<PowerPlantResponseDto> GetPlantByIdAsync(Guid id);
    Task<IEnumerable<PowerPlantResponseDto>> GetPlantsAsync();
    Task<PowerPlantResponseDto> UpdatePlantAsync(Guid id, PowerPlantRequestDto plantRequest);
    Task DeletePlantAsync(Guid id);
}

internal class PlantService(IPlantRepository plantRepository, IMapper mapper) : IPlantService
{
    public async Task<PowerPlantResponseDto> CreatePlantAsync(PowerPlantRequestDto plantRequest)
    {
        var plant = mapper.Map<PowerPlant>(plantRequest);

        var createdPlant = await plantRepository.CreatePlantAsync(plant);
        await plantRepository.SaveChangesAsync();

        return mapper.Map<PowerPlantResponseDto>(createdPlant);
    }

    public async Task<PowerPlantResponseDto> GetPlantByIdAsync(Guid id)
    {
        var plant = await plantRepository.GetPlantByIdAsync(id);

        if (plant is null)
            throw new PlantNotFoundException($"Plant with id {id} not found.");

        return mapper.Map<PowerPlantResponseDto>(plant);
    }

    public async Task<IEnumerable<PowerPlantResponseDto>> GetPlantsAsync()
    {
        var plants = await plantRepository.GetPlantsAsync();

        if (plants is null)
            throw new PlantNotFoundException("No plants found.");

        return mapper.Map<IEnumerable<PowerPlantResponseDto>>(plants);
    }

    public async Task<PowerPlantResponseDto> UpdatePlantAsync(Guid id, PowerPlantRequestDto plantRequest)
    {
        var existingPlant = await plantRepository.GetPlantByIdAsync(id);
        if (existingPlant is null)
            throw new PlantNotFoundException($"Power plant with id {id} not found.");

        mapper.Map(plantRequest, existingPlant);
        await plantRepository.UpdatePlantAsync(existingPlant);

        return mapper.Map<PowerPlantResponseDto>(existingPlant);
    }

    public async Task DeletePlantAsync(Guid id)
    {
        var plant = await plantRepository.GetPlantByIdAsync(id);
        if (plant is null)
            throw new PlantNotFoundException($"Power plant with id {id} not found.");

        await plantRepository.DeletePlantAsync(plant);
    }
}