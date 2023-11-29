using Application.Abstractions.CQRS;

namespace Application.PiggyBanks.GetPiggyBankById;

public sealed record GetPiggyBankByIdQuery(
    Guid PiggyBankId,
    Guid UserId
) : IQuery<PiggyBankDTO>;