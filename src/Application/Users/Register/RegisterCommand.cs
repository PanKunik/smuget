using Application.Abstractions.CQRS;

namespace Application.Users.Register;

public sealed record RegisterCommand(
    string Email,
    string FirstName,
    string Password
) : ICommand;