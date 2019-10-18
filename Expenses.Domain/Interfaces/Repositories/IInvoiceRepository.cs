using Expenses.Domain.Interfaces.Models;
using Expenses.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Domain.Interfaces.Repositories
{
    public interface IInvoiceRepository
    {
        void Create(Invoice model);

        void Update(Invoice model);

        Task<Invoice> GetById(Guid id);
      
        Invoice GetByName(string name);
    }
}
