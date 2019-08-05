using Expenses.Application.Invoice.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Application.Invoice
{
    public interface IInvoiceService
    {
        Task<InvoiceResponse> Create(InvoiceRequest viewModel);
    }
}
