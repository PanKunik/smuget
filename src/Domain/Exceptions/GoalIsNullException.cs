using Domain.PiggyBanks;

namespace Domain.Exceptions;

public sealed class GoalIsNullException
    : RequiredFieldException
{
    public GoalIsNullException()
        : base(nameof(Goal)) { }
}
