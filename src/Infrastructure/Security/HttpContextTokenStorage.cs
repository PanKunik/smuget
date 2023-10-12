using Application.Abstractions.Security;
using Microsoft.AspNetCore.Http;

internal sealed class HttpContextTokenStorage : ITokenStorage
{
    private const string TokenKey = "jwt";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTokenStorage(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Store(string token)
        => _httpContextAccessor.HttpContext?.Items.TryAdd(
                TokenKey,
                token
            );

    public string Get()
        => _httpContextAccessor.HttpContext.Items.TryGetValue(
                TokenKey,
                out var jwt) ? (string)jwt : null;
}