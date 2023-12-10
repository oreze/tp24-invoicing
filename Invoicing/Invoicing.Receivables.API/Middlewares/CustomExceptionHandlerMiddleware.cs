using System.Net;
using System.Text.Json;
using Identity.Receivables.ApplicationContracts.DTOs;
using Invoicing.Receivables.Domain.Exceptions;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Invoicing.Receivables.API.Middlewares;

public class CustomExceptionHandlerMiddleware
{
    private static readonly ILogger Logger = Log.ForContext<CustomExceptionHandlerMiddleware>();

    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            // log the error
            Logger.Error(exception, "error during executing {Context}", context.Request.Path.Value);
            var response = context.Response;
            response.ContentType = "application/json";

            // get the response code and message
            var (status, message) = GetResponse(exception);
            response.StatusCode = (int)status;
            await response.WriteAsync(message);
        }
    }

    public (HttpStatusCode code, string message) GetResponse(Exception exception)
    {
        HttpStatusCode code;
        switch (exception)
        {
            case NotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case UnauthorizedAccessException:
                code = HttpStatusCode.Unauthorized;
                break;
            case InputException
                or InputNullException:
                code = HttpStatusCode.BadRequest;
                break;
            default:
                code = HttpStatusCode.InternalServerError;
                break;
        }

        var errorResponse = new ErrorResponse { Message = exception.Message };
        return (code, JsonSerializer.Serialize(errorResponse));
    }
}
