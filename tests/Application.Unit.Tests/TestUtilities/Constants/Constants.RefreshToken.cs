namespace Application.Unit.Tests.TestUtilities.Constants;

public static partial class Constants
{
    public static class RefreshToken
    {
        public static readonly Guid Id = new Guid(0, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 });
        public const string Token = "3FAn5gJc/RyQ6zPwH/+mFA==";
        public static readonly Guid JwtId = new Guid(0, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 });
        public static readonly DateTime CreationDateTime = DateTime.Now;
        public static readonly DateTime ExpirationDateTime = DateTime.Now.AddDays(1);
        public const bool Used = false;
        public const bool Invalidated = false;
    }
}