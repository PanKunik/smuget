using System.Text;
using Microsoft.IO;

namespace WebAPI.Middlewares;

public sealed class HTTPLoggingMiddleware : IMiddleware
{
    private readonly ILogger<HTTPLoggingMiddleware> _logger;
    private readonly RecyclableMemoryStreamManager _streamManager;

    public HTTPLoggingMiddleware(
        ILogger<HTTPLoggingMiddleware> logger
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _streamManager = new RecyclableMemoryStreamManager();
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var httpRequestId = Guid.NewGuid();
        context.Request.EnableBuffering();
        await LogRequest(context, httpRequestId);

        var originalBodyStream = context.Response.Body;
        await using var responseStream = _streamManager.GetStream("response");
        context.Response.Body = responseStream;

        await next.Invoke(context);

        await LogResponse(context, httpRequestId);
        await responseStream.CopyToAsync(originalBodyStream);
        context.Response.Body = originalBodyStream;
    }

    private async Task LogRequest(HttpContext context, Guid httpRequestId)
    {
        var requestLogContent = new StringBuilder("Request:\r\n");

        requestLogContent.AppendLine($"\tGuid: {httpRequestId}");
        requestLogContent.AppendLine($"\tDatetime: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffzzz")}");
        requestLogContent.AppendLine($"\tMethod: {context.Request.Method}");
        requestLogContent.AppendLine($"\tHost: {context.Request.Host}");
        requestLogContent.AppendLine($"\tPath: {context.Request.Path}");
        requestLogContent.AppendLine($"\tHeaders: {ConcatHeaders(context.Request.Headers)}");
        requestLogContent.AppendLine($"\tBody: {await ParseBody(new StreamReader(context.Request.Body))}");

        _logger.LogInformation(requestLogContent.ToString());

        context.Request.Body.Position = 0;
    }

    private string ConcatHeaders(IHeaderDictionary headers)
    {
        var concatenatedHeaders = new StringBuilder("{ ");
        concatenatedHeaders.Append(
            string.Join(", ", ParseHeaders(headers))
        );
        concatenatedHeaders.Append(" }");
        var headersContent = concatenatedHeaders.ToString();
        return string.IsNullOrWhiteSpace(headersContent) ? "<empty>" : headersContent;
    }

    private IEnumerable<string> ParseHeaders(IHeaderDictionary headers)
    {
        foreach (var header in headers)
        {
            yield return $"{header.Key}: {header.Value}";
        }
    }

    private async Task<string> ParseBody(StreamReader bodyReader)
    {
        var bodyContent = await bodyReader.ReadToEndAsync();
        return string.IsNullOrWhiteSpace(bodyContent)
            ? "<empty>"
            : bodyContent
                .Replace(" ", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace("\t", "");
    }

    private async Task LogResponse(HttpContext context, Guid httpRequestId)
    {
        var responseLogContent = new StringBuilder("Response:\r\n");

        responseLogContent.AppendLine($"\tGuid: {httpRequestId}");
        responseLogContent.AppendLine($"\tDatetime: {DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fffzzz")}");
        responseLogContent.AppendLine($"\tStatus code: {context.Response.StatusCode}");
        responseLogContent.AppendLine($"\tHeaders: {ConcatHeaders(context.Response.Headers)}");

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        responseLogContent.AppendLine($"\tBody: {await ParseBody(new StreamReader(context.Response.Body, leaveOpen: true))}");
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        _logger.LogInformation(responseLogContent.ToString());
    }
}