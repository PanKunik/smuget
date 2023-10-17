using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Security;
using Application.Identity;
using Domain.Users;
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

    public AuthenticationDTO CreateToken(User user)
    {
        var jti = Guid.NewGuid();

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jti.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email.Value),
            new Claim("id", user.Id.Value.ToString())
        };

        var now = DateTime.UtcNow;
        var expires = now.Add(_expiry);

        var jwt = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            now,
            expires,
            _signingCredentials
        );
        var token = new JwtSecurityTokenHandler()
            .WriteToken(jwt);

        return new AuthenticationDTO()
        {
            Id = jti,
            AccessToken = token,
            RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            CreationDateTime = now,
            ExpirationDateTime = expires
        };
    }
}