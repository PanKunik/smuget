using Domain.Piggybanks;

namespace Domain.Exceptions;

public sealed class GoalIsNullException
    : RequiredFieldException
{
    public GoalIsNullException()
        : base(nameof(Goal)) { }
}
