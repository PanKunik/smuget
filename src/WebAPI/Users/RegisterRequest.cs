namespace WebAPI.Users;

/// <summary>
/// Informations about new user.
/// </summary>
/// <param name="Email">Email address, for log in.</param>
/// <param name="FirstName">How we can call a user.</param>
/// <param name="Password">Strong password.</param>
public sealed record RegisterRequest(
    string Email,
    string FirstName,
    string Password
);