namespace Application.Identity;

public sealed class AuthenticationDTO
{
    public Guid Id { get; set; }
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime CreationDateTime { get; set; }
    public DateTime ExpirationDateTime { get; set; }
}