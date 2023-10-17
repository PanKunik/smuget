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
                    o.IncludeErrorDetails = true;

                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = options.Audience,
                        ValidateAudience = true,
                        ValidIssuer = options.Issuer,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey)),
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    o.TokenValidationParameters = tokenValidationParameters;

                    services.AddSingleton(tokenValidationParameters);
                }
            );

        services.AddAuthorization();

        return services;
    }
}