namespace Application.Abstractions.Security;

public interface IAuthenticator
{
    string CreateToken(Guid userId);
}