using Domain.PiggyBanks;

namespace Infrastructure.Persistance.Entities.Mappings;

internal static class TransactionMappingExtensions
{
    public static Transaction ToDomain(this TransactionEntity entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new Transaction(
            new(entity.Id),
            entity.Value,
            entity.Date
        );
    }

    public static TransactionEntity ToEntity(
        this Transaction domain,
        Guid piggyBankId
    )
    {
        return new TransactionEntity()
        {
            Id = domain.Id.Value,
            Value = domain.Value,
            Date = domain.Date,
            PiggyBankId = piggyBankId
        };
    }
}