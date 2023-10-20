namespace WebAPI.Identity;

/// <summary>
/// Users valid refresh token for logout.
/// </summary>
/// <param name="RefreshToken">Valid refresh token.</param>
public sealed record LogoutRequest(
    string RefreshToken
);