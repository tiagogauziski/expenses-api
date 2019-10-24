using Expenses.Domain.Interfaces.Models;
using Expenses.Domain.Interfaces.Repositories;
using Expenses.Domain.Models;
using Expenses.Domain.Queries.Invoice;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Infra.EntityCore.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ExpensesContext _expensesContext;

        public InvoiceRepository(ExpensesContext expensesContext)
        {
            _expensesContext = expensesContext;
        }

        public void Create(Invoice model)
        {
            _expensesContext.Invoices.Add(model);
            _expensesContext.SaveChanges();
        }

        public Invoice GetById(Guid id)
        {
            return _expensesContext.Invoices.Where(i => i.Id == id).FirstOrDefault();
        }

        public Invoice GetByName(string name)
        {
            return _expensesContext.Invoices.FirstOrDefault(i => i.Name == name);
        }

        public List<Invoice> GetList(GetInvoiceListQuery query)
        {
            var invoiceList = _expensesContext.Invoices.AsQueryable();

            if (!string.IsNullOrEmpty(query.Name))
                invoiceList = invoiceList.Where(i => i.Name.IndexOf(query.Name, StringComparison.OrdinalIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(query.Description))
                invoiceList = invoiceList.Where(i => i.Description.IndexOf(query.Description, StringComparison.OrdinalIgnoreCase) != -1);

            return invoiceList.ToList();
        }

        public void Update(Invoice model)
        {
            _expensesContext.Invoices.Update(model);
            _expensesContext.SaveChanges();
        }
    }
}
