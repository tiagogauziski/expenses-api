using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Expenses.API.Authorization
{
    internal static class AuthExtensions
    {
        internal static void AddCustomAuthorization(this IServiceCollection services, AuthOptions auth0Options)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:invoices", policy => policy.Requirements.Add(new HasPermissionRequirement("read:invoices", auth0Options.Domain)));
                options.AddPolicy("create:invoices", policy => policy.Requirements.Add(new HasPermissionRequirement("create:invoices", auth0Options.Domain)));
                options.AddPolicy("update:invoices", policy => policy.Requirements.Add(new HasPermissionRequirement("update:invoices", auth0Options.Domain)));
                options.AddPolicy("delete:invoices", policy => policy.Requirements.Add(new HasPermissionRequirement("delete:invoices", auth0Options.Domain)));
                options.AddPolicy("update:statements", policy => policy.Requirements.Add(new HasPermissionRequirement("update:statements", auth0Options.Domain)));
                options.AddPolicy("create:statements", policy => policy.Requirements.Add(new HasPermissionRequirement("create:statements", auth0Options.Domain)));
                options.AddPolicy("read:statements", policy => policy.Requirements.Add(new HasPermissionRequirement("read:statements", auth0Options.Domain)));
                options.AddPolicy("delete:statements", policy => policy.Requirements.Add(new HasPermissionRequirement("delete:statements", auth0Options.Domain)));
            });

            // register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasPermissionHandler>();
        }

        internal static void AddCustomAuthentication(this IServiceCollection services, AuthOptions auth0Options)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = auth0Options.Domain;
                options.Audience = auth0Options.Audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });
        }
    }
}
