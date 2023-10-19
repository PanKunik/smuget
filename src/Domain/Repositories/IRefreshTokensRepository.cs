using Domain.RefreshTokens;

namespace Domain.Repositories;

public interface IRefreshTokensRepository
{
    Task<RefreshToken?> Get(string token);
    Task<RefreshToken?> GetActiveByUserId(Guid id);
    Task Save(RefreshToken refreshToken);
}