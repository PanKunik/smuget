namespace WebAPI.Identity;

public sealed record AuthenticationResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiry
);