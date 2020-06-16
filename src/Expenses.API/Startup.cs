using Expenses.API.Extensions.Authorization;
using Expenses.API.Extensions.Swagger;
using Expenses.API.Middleware;
using Expenses.Application.IoC;
using Expenses.OpenTelemetry.Options;
using Expenses.OpenTelemetry.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

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
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
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
            // Deserialize auth options into object
            var authOptions = new AuthOptions();
            Configuration.GetSection("Auth0").Bind(authOptions);

            // Deserialize telemetry options into object
            var telemetryOptions = new TelemetryOptions();
            Configuration.GetSection("Telemetry").Bind(telemetryOptions);

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwagger(authOptions);

            // Application AutoMapper extension
            services.AddApplicationAutoMapper();

            // Application Dependencies
            services.AddApplicationDependencies();

            // Infrastructure Dependencies - Database
            services.AddInfrastructureDatabase();

            // Infrastructure Dependencies - Message Bus
            services.AddInfrastructureMessageBus();

            // Add Authentication Services
            services.AddCustomAuthentication(authOptions);

            // Configure authorization.
            services.AddCustomAuthorization(authOptions);

            // Configure telemetry.
            services.AddTelemetry(telemetryOptions);
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
