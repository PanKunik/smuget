using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

internal static class AuthenticationExtensions
{
    private const string OptionsSectionName = "Authentication";

    public static IServiceCollection AddAuthenticationWithJwt(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var options = configuration.GetOptions<AuthenticationOptions>(OptionsSectionName);
        services.Configure<AuthenticationOptions>(configuration.GetRequiredSection(OptionsSectionName));

        services
            .AddAuthentication(
                o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer(
                o =>
                {
                    o.Audience = options.Audience;
                    o.IncludeErrorDetails = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = options.Issuer,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey))
                    };
                }
            );

        services.AddAuthorization();

        return services;
    }
}