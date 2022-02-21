using JWTGenerator.EntityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JWTGenerator.TokenHandler
{
    public static class TokenHandlerExtension
    {
        public static IServiceCollection AddJWTTokenHandlerExtension(this IServiceCollection services, JWTConfiguration Configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Only in development to disable Https 
                options.SaveToken = false;
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
            });
            services.AddScoped(generatorManager =>
            {
                return new TokenHandlerManager(Configuration);
            });
            return services;
        }
    }
}
