namespace SolarPlantDemo.Models.Common;

public class TokensResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}