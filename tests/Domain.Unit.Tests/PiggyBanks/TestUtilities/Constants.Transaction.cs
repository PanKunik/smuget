using System;
using Domain.PiggyBanks;

namespace Domain.Unit.Tests.PiggyBanks.TestUtilities;

public static partial class Constants
{
    public static class Transaction
    {
        public static readonly TransactionId Id = new(new Guid(0, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }));
        public const decimal Value = 215.90m;
        public static readonly DateOnly Date = new DateOnly(2023, 11, 11);
        public const bool Active = true;
    }
}