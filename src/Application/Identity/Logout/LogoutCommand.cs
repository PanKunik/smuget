using Application.Abstractions.CQRS;

namespace Application.Identity.Logout;

public sealed record LogoutCommand(
    string RefreshToken,
    Guid UserId
) : ICommand;