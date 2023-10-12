namespace Application.Abstractions.Security;

public interface IPasswordHasher
{
    string Secure(string password);
    bool Validate(string password, string securedPassword);
}