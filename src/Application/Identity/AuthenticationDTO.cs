namespace Application.Identity;

public sealed class AuthenticationDTO
{
    public Guid Id { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime ExpirationDateTime { get; set; }
}