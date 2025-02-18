using System.Text.Json;
using SolarPlantDemo.Models.Common;

namespace SolarPlantDemo.Integrations;

public interface IWeatherForecastService
{
    Task<WeatherForecast> GetWeatherForecastAsync(double latitude, double longitude);
}

internal class WeatherForecastService(HttpClient httpClient) : IWeatherForecastService
{
    public async Task<WeatherForecast> GetWeatherForecastAsync(double latitude, double longitude)
    {
        var url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&daily=weathercode&timezone=auto";

        using var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var openMeteoResponse = JsonSerializer.Deserialize<OpenMeteoResponse>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        if (openMeteoResponse?.Daily.Time == null ||
            openMeteoResponse.Daily.WeatherCode == null ||
            openMeteoResponse.Daily.Time.Length == 0)
        {
            throw new Exception("Invalid or empty response received from the weather API.");
        }

        var fromDate = DateTime.Parse(openMeteoResponse.Daily.Time.First());
        var toDate = DateTime.Parse(openMeteoResponse.Daily.Time.Last());

        var forecast = new WeatherForecast
        {
            FromDate = fromDate,
            ToDate = toDate,
            WeatherData = openMeteoResponse.Daily.WeatherCode
        };

        return forecast;
    }

    private class OpenMeteoResponse
    {
        public required DailyForecast Daily { get; init; }
    }

    private class DailyForecast
    {
        public required string[] Time { get; set; }
        public required int[] WeatherCode { get; set; }
    }
}