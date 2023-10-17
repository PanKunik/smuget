namespace WebAPI.Identity;

/// <summary>
/// Users tokens for refresh.
/// </summary>
/// <param name="AccessToken">Users expired JWT access token.</param>
/// <param name="RefreshToken">Users refresh token.</param>
public sealed record RefreshRequest(
    string AccessToken,
    string RefreshToken
);