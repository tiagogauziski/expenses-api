using Expenses.Infrastructure.EventBus;
using Expenses.OpenTelemetry.Extensions;
using Expenses.OpenTelemetry.Options;
using History.Worker.Expenses.HostedServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace History.Worker.Expenses
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Deserialize telemetry options into object
            var telemetryOptions = new TelemetryOptions();
            Configuration.GetSection("Telemetry").Bind(telemetryOptions);

            // Infrastructure Dependencies - Message Bus
            services.AddInfrastructureMessageBus();

            // Configure telemetry.
            services.AddTelemetry(telemetryOptions);

            services.AddHostedService<ExpensesHistoryHostedService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
