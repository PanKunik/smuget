using System;
using Domain.Users;

namespace Domain.Unit.Tests.Users.TestUtilities;

public static partial class Constants
{
    public static class User
    {
        public static readonly UserId Id = new(new Guid(0, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }));
        public static readonly Email Email = new("smuget-uer@example.com");
        public static readonly FirstName FirstName = new("Smuget");
        public const string SecuredPassword = "ABC123";
    }
}