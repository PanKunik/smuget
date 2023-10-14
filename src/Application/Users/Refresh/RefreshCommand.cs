using Application.Abstractions.CQRS;

namespace Application.Users.Refresh;

public sealed record RefreshCommand(
    string RefreshToken
) : ICommand;