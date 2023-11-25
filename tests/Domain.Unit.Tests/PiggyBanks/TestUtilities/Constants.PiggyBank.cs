using System;
using Domain.MonthlyBillings;
using Domain.PiggyBanks;

namespace Domain.Unit.Tests.PiggyBanks.TestUtilities;

public static partial class Constants
{
    public static class PiggyBank
    {
        public static readonly PiggyBankId Id = new(new Guid(0, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }));
        public static readonly Name Name = new("New car");
        public const bool WithGoal = true;
        public static readonly Goal Goal = new(2500m);
    }
}