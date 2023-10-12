using Application.Abstractions.CQRS;

namespace Application.Users.Login;

public sealed record LoginCommand(
    string Email,
    string Password
) : ICommand;