using System.Net;
using SolarPlantDemo.Exceptions;
using SolarPlantDemo.Models.Common;

namespace SolarPlantDemo.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (SolarPlantCustomException ex)
        {
            await HandleCustomExceptionAsync(context, ex);
        }
        catch (Exception)
        {
            await HandleGeneralExceptionAsync(context);
        }
    }

    private static Task HandleCustomExceptionAsync(HttpContext context, SolarPlantCustomException exception)
    {
        var statusCode = exception switch
        {
            TokenNotFoundException => HttpStatusCode.NotFound,
            UnauthorizedException => HttpStatusCode.Unauthorized,
            UserCreationException => HttpStatusCode.BadRequest,
            UserNotFoundException => HttpStatusCode.NotFound,
            UserLoginException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.BadRequest
        };

        var errorResponse = new ErrorResponse
        {
            Message = exception.Message,
            StatusCode = ((int)statusCode).ToString(),
            TimeStamp = DateTime.Now
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsJsonAsync(errorResponse);
    }

    private static Task HandleGeneralExceptionAsync(HttpContext context)
    {
        var errorResponse = new ErrorResponse
        {
            Message = "An unexpected error occurred.",
            StatusCode = "500",
            TimeStamp = DateTime.Now
        };

        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        return context.Response.WriteAsJsonAsync(errorResponse);
    }
}