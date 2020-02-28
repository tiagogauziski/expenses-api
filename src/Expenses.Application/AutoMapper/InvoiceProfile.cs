using AutoMapper;
using Expenses.Application.Services.Invoice.ViewModel;
using Expenses.Domain.Commands.Invoice;
using Expenses.Domain.Interfaces.Models;
using Expenses.Domain.Queries.Invoice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Application.AutoMapper
{
    /// <summary>
    /// Invoice AutoMapper Profile configuration
    /// </summary>
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<CreateInvoiceRequest, CreateInvoiceCommand>();
            CreateMap<UpdateInvoiceRequest, UpdateInvoiceCommand>();
            CreateMap<CreateInvoiceCommand, Expenses.Domain.Models.Invoice>();
            CreateMap<UpdateInvoiceCommand, Expenses.Domain.Models.Invoice>();
            CreateMap<GetInvoiceListRequest, GetInvoiceListQuery> ();
            CreateMap<Expenses.Domain.Models.Invoice, InvoiceResponse>();
            CreateMap<Expenses.Domain.Models.Recurrence, InvoiceRecurrence>();
            CreateMap<InvoiceRecurrence, Expenses.Domain.Models.Recurrence>();
        }
    }
}
