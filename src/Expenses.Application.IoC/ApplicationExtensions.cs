using AutoMapper;
using Expenses.Application.AutoMapper;
using Expenses.Application.CommandHandlers;
using Expenses.Application.EventHandlers;
using Expenses.Application.Services.Invoice;
using Expenses.Application.Services.Statement;
using Expenses.Domain.Commands.Invoice;
using Expenses.Domain.Commands.Statement;
using Expenses.Domain.Events.Invoice;
using Expenses.Domain.Events.Statement;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Expenses.Application.IoC
{
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Application dependencies extension methods
        /// </summary>
        /// <param name="services"></param>
        public static void AddApplicationDependencies(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //Application Services
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IStatementService, StatementService>();

            // Application - Commands
            // Invoice
            services.AddScoped<IRequestHandler<CreateInvoiceCommand, bool>, InvoiceCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateInvoiceCommand, bool>, InvoiceCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteInvoiceCommand, bool>, InvoiceCommandHandler>();
            // Statement
            services.AddScoped<IRequestHandler<CreateStatementCommand, bool>, StatementCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateStatementCommand, bool>, StatementCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteStatementCommand, bool>, StatementCommandHandler>();
            services.AddScoped<IRequestHandler<DeleteStatementByInvoiceIdCommand, bool>, StatementCommandHandler>();

            // Application - Events
            // Invoice
            services.AddScoped<INotificationHandler<InvoiceCreatedEvent>, InvoiceEventHandler>();
            services.AddScoped<INotificationHandler<InvoiceUpdatedEvent>, InvoiceEventHandler>();
            services.AddScoped<INotificationHandler<InvoiceDeletedEvent>, InvoiceEventHandler>();
            // Statement
            services.AddScoped<INotificationHandler<StatementCreatedEvent>, StatementEventHandler>();
            services.AddScoped<INotificationHandler<StatementUpdatedEvent>, StatementEventHandler>();
            services.AddScoped<INotificationHandler<StatementDeletedEvent>, StatementEventHandler>();
            services.AddScoped<INotificationHandler<StatementBulkDeletedEvent>, StatementEventHandler>();
        }

        /// <summary>
        /// Add AutoMapper profiles
        /// </summary>
        /// <param name="services"></param>
        public static void AddApplicationAutoMapper(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(typeof(AutoMapperConfiguration));
        }


    }
}
