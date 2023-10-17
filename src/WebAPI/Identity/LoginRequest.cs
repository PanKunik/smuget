namespace WebAPI.Identity;

/// <summary>
/// Users credentials for login.
/// </summary>
/// <param name="Email">Email address for login.</param>
/// <param name="Password">Users password.</param>
public sealed record LoginRequest(
    string Email,
    string Password
);