namespace Application.Abstractions.Security;

public interface ITokenStorage
{
    void Store(string token);
    string Get();
}