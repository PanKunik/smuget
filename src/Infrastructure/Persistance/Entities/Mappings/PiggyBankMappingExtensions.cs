using Domain.PiggyBanks;

namespace Infrastructure.Persistance.Entities.Mappings;

internal static class PiggyBankMappingExtensions
{
    public static PiggyBank ToDomain(this PiggyBankEntity entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new PiggyBank(
            new(entity.Id),
            new(entity.Name),
            entity.WithGoal,
            new(entity.Goal),
            new(entity.UserId),
            entity.Transactions
                .ConvertAll(t => t.ToDomain())
        );
    }

    public static PiggyBankEntity ToEntity(this PiggyBank domain)
    {
        return new PiggyBankEntity()
        {
            Id = domain.Id.Value,
            Name = domain.Name.Value,
            WithGoal = domain.WithGoal,
            Goal = domain.Goal.Value,
            UserId = domain.UserId.Value,
            Transactions = domain.Transactions
                .Select(t => t.ToEntity(domain.Id.Value))
                .ToList()
        };
    }
}