using Domain.Exceptions;
using Domain.MonthlyBillings;
using Domain.Piggybanks;

namespace Domain.PiggyBanks;

public sealed class PiggyBank
{
    public PiggyBankId Id { get; }
    public Name Name { get; }
    public bool WithGoal { get; }
    public Goal Goal { get; }

    public PiggyBank(
        PiggyBankId piggyBankId,
        Name name,
        bool withGoal,
        Goal goal
    )
    {
        ThrowIfPiggyBankIdIsNull(piggyBankId);
        ThrowIfNameIsNull(name);
        ThrowIfGoalIsNull(goal);

        Id = piggyBankId;
        Name = name;
        WithGoal = withGoal;
        ThrowIfGoalIsPassedWhenPiggyBankMarkedAsWithoutGoal(withGoal, goal);
        Goal = goal;
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
}