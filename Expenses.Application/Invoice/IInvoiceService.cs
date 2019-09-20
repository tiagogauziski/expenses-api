using Expenses.Application.Common;
using Expenses.Application.Invoice.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Application.Invoice
{
    public interface IInvoiceService
    {
        Task<Response<InvoiceResponse>> Create(CreateInvoiceRequest viewModel);
    }
}
