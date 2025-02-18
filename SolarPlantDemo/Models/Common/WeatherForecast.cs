namespace SolarPlantDemo.Models.Common;

public class WeatherForecast
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public required IEnumerable<int> WeatherData { get; set; }
}