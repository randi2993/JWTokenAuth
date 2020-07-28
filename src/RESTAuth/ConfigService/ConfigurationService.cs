using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Auth;

namespace RESTAuth.ConfigService
{
    public static class ConfigurationService
    {
        public static void AddScopedGroup(this IServiceCollection services)
        {
            services.AddScoped<JWTokenAuthService>();
        }

        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc(configuration["Swagger:versionDoc"], new OpenApiInfo { Title = configuration["Swagger:title"], Version = configuration["Swagger:version"] });

                o.AddSecurityDefinition("bearertoken", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = configuration["Swagger:SecurityDescription"],
                    Name = configuration["Swagger:SecurityName"],
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "bearertoken"
                       }
                      }, new string[] { }
                    }
                  });
            });
        }

        public static void AddJWToken(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Auth:JWToken:issuer"],
                    ValidAudience = configuration["Auth:JWToken:issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth:JWToken:key"])),
                };
                config.Events = new JwtBearerEvents
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
        }
    }
}
