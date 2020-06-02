using Expenses.API.Extensions.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Reflection;

namespace Expenses.API.Extensions.Swagger
{
    internal static class SwaggerExtensions
    {
        internal static void AddSwagger(this IServiceCollection services, AuthOptions authOptions)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "JSONDiffer API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // Define the OAuth2.0 scheme that's in use (i.e. Implicit Flow)
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{authOptions.Domain}authorize?audience={authOptions.Audience}", UriKind.Absolute),
                            TokenUrl = new Uri($"{authOptions.Domain}/oauth/token", UriKind.Absolute)
                        }
                    }
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

    }
}
