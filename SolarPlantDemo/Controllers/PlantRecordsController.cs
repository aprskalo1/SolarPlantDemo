using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarPlantDemo.Models.Enum;
using SolarPlantDemo.Services;

namespace SolarPlantDemo.Controllers;

[Authorize]
[ApiController]
[Route("api/plants/{plantId:guid}/records")]
public class PlantRecordsController(IRecordService recordService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRecords(Guid plantId, DateTime start, DateTime end, int granularity, TimeseriesType timeseriesType)
    {
        var records = await recordService.GetRecordsByTimespanAsync(plantId, start, end, granularity, timeseriesType);
        return Ok(records);
    }
}