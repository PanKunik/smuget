using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.PiggyBanks;
using Domain.Repositories;
using Domain.Users;

namespace Application.PiggyBanks.UpdateTransaction;

public sealed class UpdateTransactionCommandHandler : ICommandHandler<UpdateTransactionCommand>
{
	private readonly IPiggyBanksRepository _repository;

	public UpdateTransactionCommandHandler(IPiggyBanksRepository repository)
	{
		_repository = repository;
	}

	public async Task HandleAsync(
		UpdateTransactionCommand command,
		CancellationToken cancellationToken = default
	)
	{
		var piggyBankId = new PiggyBankId(command.PiggyBankId);
		var transactionId = new TransactionId(command.TransactionId);
		var userId = new UserId(command.UserId);

		var entity = await _repository.GetById(
			piggyBankId,
			userId,
			cancellationToken
		) ?? throw new PiggyBankNotFoundException(piggyBankId);

		entity.UpdateTransaction(
			transactionId,
			command.Value,
			command.Date
		);

		await _repository
			.Save(entity);
	}
}
