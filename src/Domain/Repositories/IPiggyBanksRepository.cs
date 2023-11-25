using Domain.MonthlyBillings;
using Domain.PiggyBanks;

namespace Domain.Repositories;

public interface IPiggyBanksRepository
{
    Task<PiggyBank?> GetByName(
        Name name,
        CancellationToken token = default
    );

    Task Save(PiggyBank piggyBank);
}