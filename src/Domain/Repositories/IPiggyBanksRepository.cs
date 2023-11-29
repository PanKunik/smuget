using Domain.MonthlyBillings;
using Domain.PiggyBanks;
using Domain.Users;

namespace Domain.Repositories;

public interface IPiggyBanksRepository
{
    Task<PiggyBank?> GetByName(
        Name name,
        UserId userId,
        CancellationToken token = default
    );

    Task<PiggyBank?> GetById(
        PiggyBankId piggyBankId,
        UserId userId,
        CancellationToken token = default
    );

    Task<IEnumerable<PiggyBank?>> GetAll(
        UserId userId,
        CancellationToken token = default
    );

    Task Save(PiggyBank piggyBank);
}