using Application.Abstractions.CQRS;

namespace Application.PiggyBanks.GetPiggyBanks;

public sealed record GetPiggyBanksQuery(
    Guid UserId
) : IQuery<IEnumerable<PiggyBankDTO?>>;