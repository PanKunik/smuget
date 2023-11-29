using Domain.PiggyBanks;

namespace Application.PiggyBanks.Mappings;

public static class TransactionMappingsExtensions
{
    public static TransactionDTO ToDTO(this Transaction domain)
    {
        if (domain is null)
        {
            return null;
        }

        return new TransactionDTO()
        {
            Id = domain.Id.Value,
            Value = domain.Value,
            Date = domain.Date
        };
    }
}