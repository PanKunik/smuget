namespace WebAPI.Identity;

public sealed record LogoutRequest(
    string RefreshToken
);