using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Security;
using Application.Identity;
using Domain.Repositories;
using Domain.Users;
using Infrastructure.Authentication;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

internal sealed class Authenticator : IAuthenticator
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly TimeSpan _tokenExpiry;
    private readonly TimeSpan _refreshTokenExpiry;
    private readonly SigningCredentials _signingCredentials;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IRefreshTokensRepository _repository;

    // TODO: Extract Identity service (for handling logic of using Authenticator and saving tokens to DB)
    public Authenticator(
        IOptions<AuthenticationOptions> authenticationOptions,
        TokenValidationParameters tokenValidationParameters,
        IRefreshTokensRepository repository
    )
    {
        _issuer = authenticationOptions.Value.Issuer;
        _audience = authenticationOptions.Value.Audience;
        _tokenExpiry = authenticationOptions.Value.TokenLifetime
            ?? TimeSpan.FromMinutes(2);
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
        _repository = repository;
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
        var refreshTokenExpires = now.Add(_refreshTokenExpiry);

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
            ExpirationDateTime = refreshTokenExpires
        };
    }

    public async Task<AuthenticationDTO> RefreshToken(
        User user,
        string accessToken,
        string refreshToken
    )
    {
        var validatedToken = GetPrincipalFromToken(accessToken);

        if (validatedToken is null)
        {
            throw new InvalidAccessTokenException();
        }

        var expiryDateUnix = long.Parse(
            validatedToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Exp).Value
        );

        var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);

        if (expiryDateTimeUtc > DateTime.UtcNow)
        {
            throw new InvalidAccessTokenException();
        }

        var jti = validatedToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
        var storedRefreshToken = await _repository.Get(refreshToken)
            ?? throw new InvalidRefreshTokenException();

        if (DateTime.Now > storedRefreshToken.ExpirationDateTime)
        {
            throw new InvalidRefreshTokenException();
        }

        if (storedRefreshToken.Used || storedRefreshToken.Invalidated)
        {
            throw new InvalidRefreshTokenException();
        }

        if (storedRefreshToken.JwtId != Guid.Parse(jti))
        {
            throw new InvalidRefreshTokenException();
        }

        storedRefreshToken.Use();
        await _repository.Save(storedRefreshToken);
        return CreateToken(user);
    }

    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = _tokenValidationParameters.Clone();
        tokenValidationParameters.ValidateLifetime = false;

        try
        {
            var principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out var validatedToken
            );

            if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
            {
                return null;
            }

            return principal;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken)
            && jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase
            );
    }
}