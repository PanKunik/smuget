using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Domain.PiggyBanks;
using Domain.Repositories;
using Domain.Users;

namespace Application.PiggyBanks.CreatePiggyBank;

public sealed class CreatePiggyBankCommandHandler
    : ICommandHandler<CreatePiggyBankCommand>
{
    private readonly IPiggyBanksRepository _repository;

    public CreatePiggyBankCommandHandler(
        IPiggyBanksRepository repository
    )
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task HandleAsync(
        CreatePiggyBankCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var name = new Name(command.Name);
        var userId = new UserId(command.UserId);

        var existingPiggyBankWithSameName = await _repository.GetByName(
            name,
            userId,
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
            goal,
            userId
        );

        await _repository.Save(newPiggyBank);
    }
}
