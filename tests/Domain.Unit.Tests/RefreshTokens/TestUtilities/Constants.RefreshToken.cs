using System;
using Domain.RefreshTokens;
using Domain.Users;

namespace Domain.Unit.Tests.RefreshTokens.TestUtilities;

public static partial class Constants
{
    public static class RefreshToken
    {
        public static readonly RefreshTokenId Id = new(new Guid(0, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }));
        public const string Token = "3FAn5gJc/RyQ6zPwH/+mFA==";
        public static readonly Guid JwtId = new Guid(0, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 });
        public static readonly DateTime CreationDateTime = DateTime.Now;
        public static readonly DateTime ExpirationDateTime = DateTime.Now.AddDays(1);
        public const string IssuedFrom = "127.0.0.1";
        public const bool Used = false;
        public const bool Invalidated = false;
        public static readonly UserId UserId = new(new Guid(0, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }));
    }
}