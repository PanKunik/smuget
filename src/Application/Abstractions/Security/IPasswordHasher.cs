namespace Application.Abstractions.Security;

public interface IPasswordHasher
{
    string Secure(string password);
}