using Expenses.Application.Common;
using Expenses.Application.Services.Invoice.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Application.Services.Invoice
{
    public interface IInvoiceService
    {
        Task<Response<InvoiceResponse>> Create(CreateInvoiceRequest viewModel);

        Task<Response<InvoiceResponse>> Update(UpdateInvoiceRequest viewModel);

        Task<Response<InvoiceResponse>> GetById(string id);

        Task<Response<List<InvoiceResponse>>> GetList(GetInvoiceListRequest query);

        Task<Response<bool>> Delete(string id);
    }
}
