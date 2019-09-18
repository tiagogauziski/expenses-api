using AutoMapper;
using Expenses.Application.Invoice.ViewModel;
using Expenses.Domain.Commands;
using Expenses.Domain.Interfaces.Models;
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
            CreateMap<CreateInvoiceCommand, Expenses.Domain.Models.Invoice>();
            CreateMap<Expenses.Domain.Models.Invoice, InvoiceResponse>();
        }
    }
}
