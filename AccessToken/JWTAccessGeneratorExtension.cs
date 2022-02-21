using JWTGenerator.EntityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace JWTGenerator.AccessToken
{
    public static class JWTAccessGeneratorExtension
    {
        public static IServiceCollection AddJWTAccessGenerator(this IServiceCollection services, JWTConfiguration Configuration)
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
                return new JWTAccessGeneratorManager(Configuration);
            });
            return services;
        }
    }
}
