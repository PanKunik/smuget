using Domain.RefreshTokens;

namespace Domain.Repositories;

public interface IRefreshTokensRepository
{
    Task<RefreshToken?> Get(string token);
    Task Save(RefreshToken refreshToken);
}