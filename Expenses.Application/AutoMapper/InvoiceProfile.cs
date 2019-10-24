using AutoMapper;
using Expenses.Application.Invoice.ViewModel;
using Expenses.Domain.Commands;
using Expenses.Domain.Interfaces.Models;
using Expenses.Domain.Queries.Invoice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Application.AutoMapper
{
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
        }
    }
}
