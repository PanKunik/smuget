namespace WebAPI.Services.Users;

public sealed class UserService : IUserService
{
    private readonly HttpContext _httpContext;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public Guid UserId
        => Guid.Parse(_httpContext.User.Identity?.Name
            ?? throw new UserIdentityNotFoundException());
}