using System.Text;

namespace WebAPI.Middlewares;

public sealed class HTTPLoggingMiddleware : IMiddleware
{
    private readonly ILogger<HTTPLoggingMiddleware> _logger;

    public HTTPLoggingMiddleware(
        ILogger<HTTPLoggingMiddleware> logger
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await LogRequest(context);
        await next.Invoke(context);

    }

    private async Task LogRequest(HttpContext context)
    {
        var requestLogContent = new StringBuilder("Request:\r\n");

        requestLogContent.AppendLine($"\tMethod: {context.Request.Method}");
        requestLogContent.AppendLine($"\tHost: {context.Request.Host}");
        requestLogContent.AppendLine($"\tPath: {context.Request.Path}");
        requestLogContent.AppendLine($"\tHeaders: {ConcatHeaders(context.Request.Headers)}");
        requestLogContent.AppendLine($"\tBody: {await ParseBody(context.Request)}");

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

    private async Task<string> ParseBody(HttpRequest request)
    {
        request.EnableBuffering();
        var requestReader = new StreamReader(request.Body);
        var bodyContent = await requestReader.ReadToEndAsync();
        return string.IsNullOrWhiteSpace(bodyContent)
            ? "<empty>"
            : bodyContent
                .Replace(" ", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace("\t", "");
    }
}