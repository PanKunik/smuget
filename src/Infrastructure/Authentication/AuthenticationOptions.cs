namespace Infrastructure.Authentication;

public sealed class AuthenticationOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SigningKey { get; set; }
    public TimeSpan? TokenLifetime { get; set; }
    public TimeSpan? RefreshTokenLifetime { get; set; }
}