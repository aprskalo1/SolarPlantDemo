using Microsoft.AspNetCore.Mvc;
using Moq;
using SolarPlantDemo.Controllers;
using SolarPlantDemo.Models.DTOs.Response;
using SolarPlantDemo.Models.Entities;
using SolarPlantDemo.Models.Enum;
using SolarPlantDemo.Services;

namespace SolarPlantTests
{
    public class PlantRecordsControllerTests
    {
        private readonly Mock<IRecordService> _mockRecordService;
        private readonly PlantRecordsController _controller;

        public PlantRecordsControllerTests()
        {
            _mockRecordService = new Mock<IRecordService>();
            _controller = new PlantRecordsController(_mockRecordService.Object);
        }

        [Fact]
        public async Task GetRecords_ReturnsOkResult_WithRecords()
        {
            var plantId = Guid.NewGuid();
            var start = DateTime.Now;
            var end = DateTime.Now.AddDays(1);
            const int granularity = 15;
            const TimeseriesType timeseriesType = TimeseriesType.Production;

            var records = new List<RecordResponseDto>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    PowerPlantId = plantId,
                    PowerGenerated = 100,
                    RecordedAt = start,
                    PowerPlant = new PowerPlant
                    {
                        Id = plantId,
                        Name = "Test Plant",
                        InstalledPower = 150,
                        InstallationDate = DateTime.Now,
                        Latitude = 45.0,
                        Longitude = 15.0,
                        CreatedAt = DateTime.Now
                    }
                }
            };

            _mockRecordService.Setup(rs => rs.GetRecordsByTimespanAsync(plantId, start, end, granularity, timeseriesType))
                .ReturnsAsync(records);

            var result = await _controller.GetRecords(plantId, start, end, granularity, timeseriesType);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnRecords = Assert.IsAssignableFrom<IEnumerable<RecordResponseDto>>(okResult.Value);
            Assert.Single(returnRecords);
        }
    }
}