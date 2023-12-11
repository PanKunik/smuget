using Application.Abstractions.CQRS;
using Application.Exceptions;
using Application.PiggyBanks.UpdateTransaction;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.PiggyBanks;
using Domain.Repositories;
using Domain.Users;

namespace Application.Unit.Tests.PiggyBanks.UpdateTransaction;

public sealed class UpdateTransactionCommandHandlerTests
{
	private readonly ICommandHandler<UpdateTransactionCommand> _cut;

	private readonly IPiggyBanksRepository _mockRepository;

	public UpdateTransactionCommandHandlerTests()
	{
		_mockRepository = Substitute.For<IPiggyBanksRepository>();

		_mockRepository
			.GetById(
				Arg.Is(new PiggyBankId(Constants.PiggyBank.Id)),
				Arg.Is(new UserId(Constants.User.Id))
			)
			.Returns(
				PiggyBanksUtilities.CreatePiggyBank(
					new List<Transaction>()
					{
						new Transaction(
							new(Constants.Transaction.Id),
							Constants.Transaction.Value,
							Constants.Transaction.Date
						)
					}
				)
			);

		_cut = new UpdateTransactionCommandHandler(
			_mockRepository
		);
	}

	[Fact]
	public async Task HandleAsync_WhenCalled_ShouldCallGetByIdOnRepositoryOnce()
	{
		// Arrange
		var command = UpdateTransactionCommandUtilities.CreateCommand();

		// Act
		await _cut.HandleAsync(
			command,
			default
		);

		// Assert
		await _mockRepository
			.Received(1)
			.GetById(
				Arg.Is<PiggyBankId>(
					p => p.Value == Constants.PiggyBank.Id
				),
				Arg.Is<UserId>(
					u => u.Value == Constants.User.Id
				)
			);
	}

	[Fact]
	public async Task HandleAsync_WhenPiggyBankNotFound_ShouldThrowPiggyBankNotFoundException()
	{
		// Arrange
		var command = new UpdateTransactionCommand(
			Guid.NewGuid(),
			Constants.Transaction.Id,
			Constants.User.Id,
			Constants.Transaction.Value,
			Constants.Transaction.Date
		);

		var updateTransaction = async () => await _cut.HandleAsync(
			command,
			default
		);

		// Act & Assert
		await updateTransaction
			.Should()
			.ThrowExactlyAsync<PiggyBankNotFoundException>();
	}

	[Fact]
	public async Task HandleAsync_OnSuccess_ShouldCallSaveOnRepositoryOnce()
	{
		// Arrange
		var command = UpdateTransactionCommandUtilities.CreateCommand();
		PiggyBank passedArgument = null;
		await _mockRepository
			.Save(
				Arg.Do<PiggyBank>(
					p => passedArgument = p
				)
			);

		// Act
		await _cut.HandleAsync(
			command,
			default
		);

		// Assert
		passedArgument
			.Should()
			.NotBeNull();

		passedArgument?.Transactions
			.Should()
			.BeEquivalentTo(new List<Transaction>
			{
				new Transaction(
					new(Constants.Transaction.Id),
					777M,
					new DateOnly(2019, 12, 19)
				)
			});
	}
}
