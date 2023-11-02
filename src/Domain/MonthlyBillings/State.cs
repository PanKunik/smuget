using Domain.Abstractions;

namespace Domain.MonthlyBillings;

public sealed record State
    : Enumeration<State>
{
    public static State Open = new State(1, "Open");
    public static State Closed = new State(2, "Closed");

    private State(
        int id,
        string name
    )
        : base(
            id,
            name
        ) { }

    public override string ToString()
        => base.ToString();
}