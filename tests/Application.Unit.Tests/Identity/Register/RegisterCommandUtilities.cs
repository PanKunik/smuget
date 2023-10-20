using Application.Unit.Tests.TestUtilities.Constants;
using Application.Identity.Register;

namespace Application.Unit.Tests.Identity.Register;

public static class RegisterCommandUtilities
{
    public static RegisterCommand CreateCommand()
        => new(
            "regular-user2@smuget.com",
            Constants.User.FirstName,
            Constants.User.Password
        );
}