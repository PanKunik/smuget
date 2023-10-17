using Application.Abstractions.CQRS;

namespace Application.Identity.Register;

public sealed record RegisterCommand(
    string Email,
    string FirstName,
    string Password
) : ICommand;