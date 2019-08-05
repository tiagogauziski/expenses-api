using AutoMapper;
using Expenses.Application.Invoice.ViewModel;
using Expenses.Domain.Commands;
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
        }
    }
}
