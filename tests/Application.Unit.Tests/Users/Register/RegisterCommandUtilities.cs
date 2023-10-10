using Application.Unit.Tests.TestUtilities.Constants;
using Application.Users.Register;

namespace Application.Unit.Tests.Users.Register;

public static class RegisterCommandUtilities
{
    public static RegisterCommand CreateCommand()
        => new(
            "regular-user2@smuget.com",
            Constants.User.FirstName,
            Constants.User.Password
        );
}