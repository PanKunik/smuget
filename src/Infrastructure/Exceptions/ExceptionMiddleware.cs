using Application.Exceptions;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Exceptions;

internal sealed class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        ILogger<ExceptionMiddleware> logger
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            await HandleExceptionAsync(exception, context);
        }
    }

    private async Task HandleExceptionAsync(Exception exception, HttpContext context)
    {
        var (statusCode, error) = exception switch
        {
            InvalidRefreshTokenException => (401,
                new Error(
                    "Invalid refresh token.",
                    "InvalidRefreshToken",
                    context.Request.Path
                )
            ),
            InvalidAccessTokenException => (401,
                new Error(
                    "Invalid access token.",
                    "InvalidAccessToken",
                    context.Request.Path
                )
            ),
            IdentityException => (401,
                new Error(
                    exception.Message,
                    exception.GetType().Name?.Replace("Exception", "") ?? "Invalid identity",
                    context.Request.Path
                )
            ),
            SmugetException => (StatusCodes.Status400BadRequest,
                new Error(
                    exception.Message,
                    exception.GetType().Name?.Replace("Exception", "") ?? "Exception",
                    context.Request.Path
                )
            ),
            _ => (StatusCodes.Status500InternalServerError,
                new Error(
                    "Internal server error has occured. Please contact with administrator.",
                    "Exception",
                    context.Request.Path
                )
            )
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(error);
    }
}