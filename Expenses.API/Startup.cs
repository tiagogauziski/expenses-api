using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Expenses.Application.Invoice;
using Expenses.Domain.CommandHandlers;
using Expenses.Domain.Commands;
using Expenses.Domain.Core.Bus;
using Expenses.Domain.Core.Events;
using Expenses.Domain.EventHandlers;
using Expenses.Domain.Events;
using Expenses.Infrastructure.Bus;
using Expenses.Infrastructure.EventStore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using Expenses.API.Extensions;
using Expenses.Domain.Interfaces.Repositories;
using Expenses.Infra.EntityCore.Repositories;
using Expenses.Infra.EntityCore;
using Expenses.API.Middleware;

namespace Expenses.API
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "JSONDiffer API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //AutoMapper extension
            services.AddAutoMapperExtension();

            //Application Services
            services.AddScoped<IInvoiceService, InvoiceService>();

            // Domain Bus (Mediator)
            services.AddMediatR(typeof(Startup));
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            services.AddScoped<IEventStore, InMemoryEventStore>();

            // Domain - Commands
            services.AddScoped<IRequestHandler<CreateInvoiceCommand, bool>, InvoiceCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateInvoiceCommand, bool>, InvoiceCommandHandler>();

            // Domain - Events
            services.AddScoped<INotificationHandler<InvoiceCreatedEvent>, InvoiceEventHandler>();
            services.AddScoped<INotificationHandler<InvoiceUpdatedEvent>, InvoiceEventHandler>();

            // Infrastructure - Repositories
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();

            // Infrastructre - DbContext Configuration
            services.AddDbContext<ExpensesContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            }); 

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
