namespace SolarPlantDemo.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private readonly string _logFilePath = configuration["Logging:LogFilePath"] ?? "logs/log.txt";


    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;
        await next(context);
        var elapsedMs = (DateTime.UtcNow - startTime).TotalMilliseconds;
        var logMessage = $"{DateTime.UtcNow:O} | {context.Request.Method} {context.Request.Path} responded {context.Response.StatusCode} in {elapsedMs} ms";
        await LogToFileAsync(logMessage);
    }

    private async Task LogToFileAsync(string message)
    {
        var directory = Path.GetDirectoryName(_logFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.AppendAllTextAsync(_logFilePath, message + Environment.NewLine);
    }
}