using System;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.TestUtilities;

public static partial class Constants
{
    public static class MonthlyBilling
    {
        public static readonly MonthlyBillingId Id = new(Guid.Parse("9209c3d9-9c5e-46cf-b355-0b498ca1ff0b"));
        public static readonly Year Year = new(2023);
        public static readonly Month Month = new(2);
        public static readonly Currency Currency = new("PLN");
        public static readonly State State = State.Open;
    }
}