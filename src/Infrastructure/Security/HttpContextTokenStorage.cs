using Application.Abstractions.Security;
using Application.Users;
using Microsoft.AspNetCore.Http;

internal sealed class HttpContextTokenStorage : ITokenStorage
{
    private const string TokenKey = "jwt";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTokenStorage(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Store(AuthenticationDTO token)
        => _httpContextAccessor.HttpContext?.Items.TryAdd(
                TokenKey,
                token
            );

    public AuthenticationDTO Get()
    {
        object? token = null;
        _httpContextAccessor.HttpContext?.Items.TryGetValue(
            TokenKey,
            out token
        );
        return token as AuthenticationDTO;
    }
}