namespace Application.Identity;

public sealed class AuthenticationDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expires { get; set; }
}