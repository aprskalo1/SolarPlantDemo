using Microsoft.AspNetCore.Mvc;
using Moq;
using SolarPlantDemo.Controllers;
using SolarPlantDemo.Models.DTOs.Request;
using SolarPlantDemo.Models.DTOs.Response;
using SolarPlantDemo.Services;

namespace SolarPlantTests;

public class PlantControllerTests
{
    private readonly Mock<IPlantService> _mockPlantService;
    private readonly PlantController _controller;

    public PlantControllerTests()
    {
        _mockPlantService = new Mock<IPlantService>();
        _controller = new PlantController(_mockPlantService.Object);
    }

    [Fact]
    public async Task GetPlants_ReturnsOkResult_WithPlants()
    {
        var plants = new List<PowerPlantResponseDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Plant 1",
                InstalledPower = 100,
                InstallationDate = DateTime.Now,
                Latitude = 45.0,
                Longitude = 15.0,
                CreatedAt = DateTime.Now
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Plant 2",
                InstalledPower = 200,
                InstallationDate = DateTime.Now,
                Latitude = 46.0,
                Longitude = 16.0,
                CreatedAt = DateTime.Now
            }
        };

        _mockPlantService.Setup(x => x.GetPlantsAsync()).ReturnsAsync(plants);

        var result = await _controller.GetPlants();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnPlants = Assert.IsAssignableFrom<IEnumerable<PowerPlantResponseDto>>(okResult.Value);
        Assert.Equal(2, returnPlants.Count());
    }

    [Fact]
    public async Task GetPlant_ReturnsOkResult_WithPlant()
    {
        var plantId = Guid.NewGuid();
        var plant = new PowerPlantResponseDto
        {
            Id = plantId,
            Name = "Test Plant",
            InstalledPower = 150,
            InstallationDate = DateTime.Now,
            Latitude = 45.0,
            Longitude = 15.0,
            CreatedAt = DateTime.Now
        };

        _mockPlantService.Setup(x => x.GetPlantByIdAsync(plantId)).ReturnsAsync(plant);

        var result = await _controller.GetPlant(plantId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnPlant = Assert.IsType<PowerPlantResponseDto>(okResult.Value);
        Assert.Equal(plantId, returnPlant.Id);
    }

    [Fact]
    public async Task CreatePlant_ReturnsCreatedAtActionResult_WithCreatedPlant()
    {
        var plantRequest = new PowerPlantRequestDto
        {
            Name = "New Plant",
            InstalledPower = 120,
            InstallationDate = DateTime.Now,
            Latitude = 45.5,
            Longitude = 15.5
        };

        var createdPlant = new PowerPlantResponseDto
        {
            Id = Guid.NewGuid(),
            Name = plantRequest.Name,
            InstalledPower = plantRequest.InstalledPower,
            InstallationDate = plantRequest.InstallationDate,
            Latitude = plantRequest.Latitude,
            Longitude = plantRequest.Longitude,
            CreatedAt = DateTime.Now
        };

        _mockPlantService.Setup(x => x.CreatePlantAsync(plantRequest)).ReturnsAsync(createdPlant);

        var result = await _controller.CreatePlant(plantRequest);

        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnPlant = Assert.IsType<PowerPlantResponseDto>(createdAtResult.Value);
        Assert.Equal(createdPlant.Id, returnPlant.Id);
    }

    [Fact]
    public async Task UpdatePlant_ReturnsOkResult_WithUpdatedPlant()
    {
        var plantId = Guid.NewGuid();
        var plantRequest = new PowerPlantRequestDto
        {
            Name = "Updated Plant",
            InstalledPower = 130,
            InstallationDate = DateTime.Now,
            Latitude = 45.5,
            Longitude = 15.5
        };

        var updatedPlant = new PowerPlantResponseDto
        {
            Id = plantId,
            Name = plantRequest.Name,
            InstalledPower = plantRequest.InstalledPower,
            InstallationDate = plantRequest.InstallationDate,
            Latitude = plantRequest.Latitude,
            Longitude = plantRequest.Longitude,
            CreatedAt = DateTime.Now
        };

        _mockPlantService.Setup(x => x.UpdatePlantAsync(plantId, plantRequest)).ReturnsAsync(updatedPlant);

        var result = await _controller.UpdatePlant(plantId, plantRequest);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnPlant = Assert.IsType<PowerPlantResponseDto>(okResult.Value);
        Assert.Equal(updatedPlant.Id, returnPlant.Id);
    }

    [Fact]
    public async Task DeletePlant_ReturnsNoContentResult()
    {
        var plantId = Guid.NewGuid();
        _mockPlantService.Setup(x => x.DeletePlantAsync(plantId)).Returns(Task.CompletedTask);

        var result = await _controller.DeletePlant(plantId);

        Assert.IsType<NoContentResult>(result);
    }
}