using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Alumware.Tracklab.API.IAM.Infrastructure.Pipeline.Middleware.Components;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
        int statusCode = (exception.GetType().Namespace?.Contains("Domain.Model.Exceptions") ?? false) ? 400 : 500;
        var result = JsonSerializer.Serialize(new
        {
            error = exception.Message,
            exceptionType = exception.GetType().Name,
            traceId
        });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsync(result);
    }
} 