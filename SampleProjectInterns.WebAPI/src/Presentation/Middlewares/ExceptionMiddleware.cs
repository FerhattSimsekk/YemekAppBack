using Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace SampleProjectInterns.WebAPI.Presentation.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex) when (ex is ValidationException)
        {
            await Response(context, HttpStatusCode.BadRequest, ex.Message, ex.Source);
        }
        catch (Exception ex) when (ex is NotFoundException)
        {
            await Response(context, HttpStatusCode.NotFound, ex.Message, ex.Source);
        }
        catch (Exception ex) when (ex is UnAuthorizedException)
        {
            await Response(context, HttpStatusCode.Unauthorized, ex.Message, ex.Source);
        }
        catch (Exception ex) when (ex is BusinessException)
        {
            await Response(context, HttpStatusCode.BadRequest, ex.Message, ex.Source);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await Response(context, HttpStatusCode.InternalServerError, "Internal server error");
        }
    }

    private static async Task Response(
        HttpContext context, HttpStatusCode httpStatusCode, string message, string? source = null)
    {
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = (int)httpStatusCode;

        await context.Response
            .WriteAsync(JsonSerializer.Serialize(new
            {
                errors = new Dictionary<string, string[]>() { { source ?? "System", new string[] { message } } }
            }));
    }
}