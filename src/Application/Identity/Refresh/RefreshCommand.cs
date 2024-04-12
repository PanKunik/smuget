using Application.Abstractions.CQRS;

namespace Application.Identity.Refresh;

public sealed record RefreshCommand(
    string AccessToken,
    string RefreshToken,
    string IpAddess
) : ICommand;