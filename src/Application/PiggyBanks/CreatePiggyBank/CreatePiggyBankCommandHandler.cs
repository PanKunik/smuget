using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.PiggyBanks;
using Domain.Repositories;

namespace Application.PiggyBanks.CreatePiggyBank;

public sealed class CreatePiggyBankCommandHandler
    : ICommandHandler<CreatePiggyBankCommand>
{
    private readonly IPiggyBanksRepository _repository;

    public CreatePiggyBankCommandHandler(
        IPiggyBanksRepository repository
    )
    {
        _repository = repository;
    }

    public async Task HandleAsync(
        CreatePiggyBankCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var name = new Name(command.Name);

        var existingPiggyBankWithSameName = await _repository.GetByName(
            name,
            cancellationToken
        );

        if (existingPiggyBankWithSameName is not null)
        {
            throw new PiggyBankAlreadyExistsException(
                name
            );
        }

        var goal = new Goal(command.Goal);

        var newPiggyBank = new PiggyBank(
            new(Guid.NewGuid()),
            name,
            command.WithGoal,
            goal
        );

        await _repository.Save(newPiggyBank);
    }
}
