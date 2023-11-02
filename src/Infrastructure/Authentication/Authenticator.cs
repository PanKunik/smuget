using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Authentication;
using Application.Exceptions;
using Application.Identity;
using Domain.Users;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

internal sealed class Authenticator
    : IAuthenticator
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly TimeSpan _tokenExpiry;
    private readonly TimeSpan _refreshTokenExpiry;
    private readonly SigningCredentials _signingCredentials;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public Authenticator(
        IOptions<AuthenticationOptions> authenticationOptions,
        TokenValidationParameters tokenValidationParameters
    )
    {
        _issuer = authenticationOptions.Value.Issuer;
        _audience = authenticationOptions.Value.Audience;
        _tokenExpiry = authenticationOptions.Value.TokenLifetime
            ?? TimeSpan.FromMinutes(1);
        _refreshTokenExpiry = authenticationOptions.Value.RefreshTokenLifetime
            ?? TimeSpan.FromDays(1);

        _signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    authenticationOptions.Value.SigningKey
                )
            ),
            SecurityAlgorithms.HmacSha256
        );
        _tokenValidationParameters = tokenValidationParameters;
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

        var now = DateTime.Now;
        var expires = now.Add(_tokenExpiry);

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

        var refreshTokenExpires = now.Add(_refreshTokenExpiry);
        return new AuthenticationDTO()
        {
            Id = jti,
            AccessToken = token,
            RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            CreationDateTime = now,
            ExpirationDateTime = refreshTokenExpires
        };
    }

    public AuthenticationDTO RefreshToken(
        User user,
        string accessToken,
        Guid refreshTokenJwtId
    )
    {
        var validatedToken = GetPrincipalFromToken(accessToken);

        var expiryDateUnix = long.Parse(
            validatedToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Exp).Value
        );

        var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);

        if (expiryDateTimeUtc > DateTime.UtcNow)
        {
            throw new InvalidAccessTokenException("Access token hasn't expired yet.");
        }

        var jti = validatedToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
        if (refreshTokenJwtId != Guid.Parse(jti))
        {
            throw new InvalidRefreshTokenException("Refresh token isn't for passed access token.");
        }

        return CreateToken(user);
    }

    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = _tokenValidationParameters.Clone();
        tokenValidationParameters.ValidateLifetime = false;

        ClaimsPrincipal principal;
        SecurityToken validatedToken;

        try
        {
            principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out validatedToken
            );
        }
        catch
        {
            throw new InvalidAccessTokenException("Access token is invalid.");
        }

        if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
        {
            throw new InvalidAccessTokenException("Access token is invalid.");
        }

        return principal
            ?? throw new InvalidAccessTokenException("Access token is invalid.");
    }

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return validatedToken is JwtSecurityToken jwtSecurityToken
            && jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase
            );
    }
}