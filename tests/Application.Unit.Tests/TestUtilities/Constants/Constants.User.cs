namespace Application.Unit.Tests.TestUtilities.Constants;

public static partial class Constants
{
    public static class User
    {
        public static readonly Guid Id = new Guid(0, 0, 0, new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 });
        public const string Email = "regular-user@smuget.com";
        public const string FirstName = "Regular";
        public const string Password = "P@$sw0rd1";
        public const string SecuredPassword = "4a6fafec30def6bfe3ac4e8a538810ff";
    }
}