using Application.Abstractions.CQRS;

namespace Application.Identity.Login;

public sealed record LoginCommand(
    string Email,
    string Password
) : ICommand;