using Domain.PiggyBanks;

namespace Application.PiggyBanks.Mappings;

public static class PiggyBankMappingsExtensions
{
    public static PiggyBankDTO? ToDTO(this PiggyBank domain)
    {
        if (domain is null)
        {
            return null;
        }

        return new PiggyBankDTO()
        {
            Id = domain.Id.Value,
            Name = domain.Name.Value,
            WithGoal = domain.WithGoal,
            Goal = domain.Goal.Value,
            Transactions = domain.Transactions
                .Select(t => t.ToDTO())
                .ToList()
        };
    }
}