using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarPlantDemo.Models.DTOs.Request;
using SolarPlantDemo.Services;

namespace SolarPlantDemo.Controllers;

[Authorize]
[ApiController]
[Route("api/plants")]
public class PlantController(IPlantService plantService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPlants()
    {
        var plants = await plantService.GetPlantsAsync();
        return Ok(plants);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPlant(Guid id)
    {
        var plant = await plantService.GetPlantByIdAsync(id);
        return Ok(plant);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlant(PowerPlantRequestDto plantRequest)
    {
        var createdPlant = await plantService.CreatePlantAsync(plantRequest);
        return CreatedAtAction(nameof(GetPlant), new { id = createdPlant.Id }, createdPlant);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatePlant(Guid id, PowerPlantRequestDto plantRequest)
    {
        var updatedPlant = await plantService.UpdatePlantAsync(id, plantRequest);
        return Ok(updatedPlant);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePlant(Guid id)
    {
        await plantService.DeletePlantAsync(id);
        return NoContent();
    }
}