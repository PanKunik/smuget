namespace WebAPI.Services.Users;

public interface IUserService
{
    Guid UserId { get; }
    string? IpAddress { get; }
}