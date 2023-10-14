namespace Infrastructure.Persistance.Entities;

internal sealed class RefreshTokenEntity
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public bool WasUsed { get; set; }
    public Guid UserId { get; set; }
}