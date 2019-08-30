using AutoMapper;
using Expenses.Application.Invoice.ViewModel;
using Expenses.Domain.Commands;
using Expenses.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Application.AutoMapper
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<InvoiceRequest, CreateInvoiceCommand>();
            CreateMap<InvoiceCreatedEvent, InvoiceResponse>();
        }
    }
}
