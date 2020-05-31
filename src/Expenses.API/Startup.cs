using Expenses.API.Authorization;
using Expenses.API.Middleware;
using Expenses.Application.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json.Serialization;
using OpenTelemetry.Trace.Configuration;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Samplers;

namespace Expenses.API
{
    public class Startup
    {
        private const string CORS_POLICY = "EXPENSES_CORS_POLICY"; 
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddCors(options =>
            {
                options.AddPolicy(CORS_POLICY,
                builder =>
                {
                    builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // Register the Swagger generator, defining one or more Swagger documents
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
                            AuthorizationUrl = new Uri($"{Configuration["Auth0:Domain"]}authorize?audience={Configuration["Auth0:Audience"]}", UriKind.Absolute),
                            TokenUrl = new Uri($"{Configuration["Auth0:Domain"]}/oauth/token", UriKind.Absolute)
                        }
                    }
                });
               c.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            // Application AutoMapper extension
            services.AddApplicationAutoMapper();

            // Application Dependencies
            services.AddApplicationDependencies();

            // Infrastructure Dependencies - Database
            services.AddInfrastructureDatabase();

            // Infrastructure Dependencies - Message Bus
            services.AddInfrastructureMessageBus();

            // Add Authentication Services
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration.GetValue<string>("Auth0:Domain");
                options.Audience = Configuration.GetValue<string>("Auth0:Audience");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier
                };
            });

            string domain = Configuration.GetValue<string>("Auth0:Domain");
            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:invoices", policy => policy.Requirements.Add(new HasPermissionRequirement("read:invoices", domain)));
                options.AddPolicy("create:invoices", policy => policy.Requirements.Add(new HasPermissionRequirement("create:invoices", domain)));
                options.AddPolicy("update:invoices", policy => policy.Requirements.Add(new HasPermissionRequirement("update:invoices", domain)));
                options.AddPolicy("delete:invoices", policy => policy.Requirements.Add(new HasPermissionRequirement("delete:invoices", domain)));
                options.AddPolicy("update:statements", policy => policy.Requirements.Add(new HasPermissionRequirement("update:statements", domain)));
                options.AddPolicy("create:statements", policy => policy.Requirements.Add(new HasPermissionRequirement("create:statements", domain)));
                options.AddPolicy("read:statements", policy => policy.Requirements.Add(new HasPermissionRequirement("read:statements", domain)));
                options.AddPolicy("delete:statements", policy => policy.Requirements.Add(new HasPermissionRequirement("delete:statements", domain)));
            });

            // register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasPermissionHandler>();

            if (Configuration.GetValue<bool>("Telemetry:Enabled"))
            {
                services.AddOpenTelemetry((sp, builder) =>
                {
                    if (Configuration.GetValue<bool>("Telemetry:Jaeger:Enabled"))
                    {
                        builder
                            .UseJaeger(options =>
                            {
                                options.ServiceName = Configuration.GetValue<string>("Telemetry:Jaeger:ServiceName");
                            });
                    }
                    else if (Configuration.GetValue<bool>("Telemetry:ApplicationInsights:Enabled"))
                    {
                        builder
                            .UseApplicationInsights(options =>
                            {
                                options.InstrumentationKey = Configuration.GetValue<string>("Telemetry:ApplicationInsights:InstrumenetationKey");
                            });
                    }
                    builder
                        .SetSampler(new AlwaysOnSampler())
                        .AddRequestAdapter()
                        .AddDependencyAdapter(config =>
                        {
                            config.SetHttpFlavor = true;
                        });
                });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
            if (env.IsDevelopment())
            {
                //uncomment for debug purposes
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.OAuthClientId(Configuration.GetValue<string>("Auth0:ClientId"));
                c.OAuthScopeSeparator(" ");
            }); 

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(CORS_POLICY);

            // Enable authentication middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
