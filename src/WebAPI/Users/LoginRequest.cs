namespace WebAPI.Users;

/// <summary>
/// USers credentials for login.
/// </summary>
/// <param name="Email">Email address for login.</param>
/// <param name="Password">Users password.</param>
public sealed record LoginRequest(
    string Email,
    string Password
);