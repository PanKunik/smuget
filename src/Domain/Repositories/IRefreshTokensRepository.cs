using Domain.RefreshTokens;

namespace Domain.Repositories;

public interface IRefreshTokensRepository
{
    Task<RefreshToken?> Get(string token);
    Task<RefreshToken?> GetActiveByUserId(Guid id);
    Task<RefreshToken?> GetByRefreshedBy(Guid refreshedBy);
    Task Save(RefreshToken refreshToken);
}