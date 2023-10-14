namespace Application.Unit.Tests.TestUtilities.Constants;

public static partial class Constants
{
    public static class RefreshToken
    {
        public static readonly Guid Id = new Guid(0, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 });
        public const string Token = "3FAn5gJc/RyQ6zPwH/+mFA==";
        public static readonly DateTime Expires = DateTime.Now.AddDays(1);
        public const bool WasUsed = false;
    }
}