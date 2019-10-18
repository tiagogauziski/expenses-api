using Expenses.Domain.Interfaces.Models;
using Expenses.Domain.Interfaces.Repositories;
using Expenses.Domain.Models;
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

        public async Task<Invoice> GetById(Guid id)
        {
            return await _expensesContext.Invoices.Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Invoice GetByName(string name)
        {
            return _expensesContext.Invoices.FirstOrDefault(i => i.Name == name);
        }

        public void Update(Invoice model)
        {
            _expensesContext.Invoices.Update(model);
            _expensesContext.SaveChanges();
        }
    }
}
