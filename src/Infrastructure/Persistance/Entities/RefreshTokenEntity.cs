namespace Infrastructure.Persistance.Entities;

internal sealed class RefreshTokenEntity
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public Guid JwtId { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime ExpirationDateTime { get; set; }
    public string IssuedFrom { get; set; }
    public bool Used { get; set; }
    public bool Invalidated { get; set; }
    public Guid UserId { get; set; }
    public Guid? RefreshedBy { get; set; }
}