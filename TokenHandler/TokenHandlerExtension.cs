using JWTGenerator.JWTModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
namespace JWTGenerator.TokenHandler;
public static class TokenHandlerExtension
{
    public static IServiceCollection AddJWTTokenHandlerExtension(this IServiceCollection services, JWTConfiguration Configuration, bool requireHttpsMetadata = false, bool saveTokenInAuthProperties = false)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = requireHttpsMetadata;
            options.SaveToken = saveTokenInAuthProperties;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = Configuration.Audience,
                ValidIssuer = Configuration.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(Configuration.Key)),
                ClockSkew = TimeSpan.Zero // once is expired, it is not valid.
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                }
            };
        });
        services.AddSingleton(generatorManager =>
        {
            return new TokenHandlerManager(Configuration);
        });
        return services;
    }
}

