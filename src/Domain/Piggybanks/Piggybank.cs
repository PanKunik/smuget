using Domain.Exceptions;
using Domain.MonthlyBillings;
using Domain.Users;

namespace Domain.PiggyBanks;

public sealed class PiggyBank
{
    private readonly List<Transaction> _transactions;

    public PiggyBankId Id { get; }
    public Name Name { get; }
    public bool WithGoal { get; }
    public Goal Goal { get; }
    public UserId UserId { get; }

    public IReadOnlyCollection<Transaction> Transactions
        => _transactions.AsReadOnly();

    public PiggyBank(
        PiggyBankId piggyBankId,
        Name name,
        bool withGoal,
        Goal goal,
        UserId userId,
        List<Transaction> transactions = null
    )
    {
        ThrowIfPiggyBankIdIsNull(piggyBankId);
        ThrowIfNameIsNull(name);
        ThrowIfGoalIsNull(goal);
        ThrowIfUserIdIsNull(userId);

        Id = piggyBankId;
        Name = name;
        WithGoal = withGoal;
        ThrowIfGoalIsPassedWhenPiggyBankMarkedAsWithoutGoal(withGoal, goal);
        Goal = goal;
        UserId = userId;

        _transactions = transactions ?? new List<Transaction>();
    }

    private void ThrowIfPiggyBankIdIsNull(PiggyBankId piggyBankId)
    {
        if (piggyBankId is null)
        {
            throw new PiggyBankIdIsNullException();
        }
    }

    private void ThrowIfNameIsNull(Name name)
    {
        if (name is null)
        {
            throw new NameIsNullException();
        }
    }

    private void ThrowIfGoalIsNull(Goal goal)
    {
        if (goal is null)
        {
            throw new GoalIsNullException();
        }
    }

    private void ThrowIfUserIdIsNull(UserId userId)
    {
        if (userId is null)
        {
            throw new UserIdIsNullException();
        }
    }

    private void ThrowIfGoalIsPassedWhenPiggyBankMarkedAsWithoutGoal(
        bool withGoal,
        Goal goal
    )
    {
        if (withGoal ^ goal.Value > 0)
        {
            throw new InvalidPiggyBankGoalValueException();
        }
    }

    public void AddTransaction(Transaction transaction)
    {
        ThrowIfTransactionAlreadyExists(transaction);
        _transactions.Add(transaction);
    }

    private void ThrowIfTransactionAlreadyExists(Transaction transaction)
    {
        if (_transactions.Any(t => t.Id == transaction.Id))
        {
            throw new TransactionAlreadyExistsException(transaction.Id);
        }
    }

    public void RemoveTransaction(TransactionId transactionId)
    {
        var transactionToRemove = _transactions.Find(t => t.Id == transactionId);

        if (transactionToRemove is null)
        {
            throw new TransactionNotFoundException(transactionId);
        }

        _transactions.Remove(transactionToRemove);
    }
}