using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Security;
using Application.Users;
using Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

internal sealed class Authenticator : IAuthenticator
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly TimeSpan _expiry;
    private readonly SigningCredentials _signingCredentials;

    public Authenticator(
        IOptions<AuthenticationOptions> authenticationOptions
    )
    {
        _issuer = authenticationOptions.Value.Issuer;
        _audience = authenticationOptions.Value.Audience;
        _expiry = authenticationOptions.Value.Expiry
            ?? TimeSpan.FromHours(1);

        _signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    authenticationOptions.Value.SigningKey
                )
            ),
            SecurityAlgorithms.HmacSha256
        );
    }

    public AuthenticationDTO CreateToken(Guid userId)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString())
        };

        var now = DateTime.Now;
        var expires = now.Add(_expiry);

        var jwt = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            DateTime.Now,
            expires,
            _signingCredentials
        );
        var token = new JwtSecurityTokenHandler()
            .WriteToken(jwt);

        return new AuthenticationDTO()
        {
            AccessToken = token,
            RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128)),
            Expires = expires
        };
    }
}