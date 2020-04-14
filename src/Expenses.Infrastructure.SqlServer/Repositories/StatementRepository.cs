using Expenses.Domain.Interfaces.Repositories;
using Expenses.Domain.Models;
using Expenses.Domain.Queries.Statement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.SqlServer.Repositories
{
    public class StatementRepository : IStatementRepository
    {
        private readonly ExpensesContext _expensesContext;

        public StatementRepository(ExpensesContext expensesContext)
        {
            _expensesContext = expensesContext;
        }

        public void Create(Statement model)
        {
            _expensesContext.Statements.Add(model);
            _expensesContext.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var model = _expensesContext.Statements.FirstOrDefault(i => i.Id == id);
            _expensesContext.Statements.Remove(model);
            _expensesContext.SaveChanges();
        }

        public async Task<IReadOnlyList<Statement>> DeleteByInvoiceIdAsync(Guid invoiceId)
        {
            var statements = _expensesContext.Statements.Where(i => i.InvoiceId == invoiceId).ToList();
            _expensesContext.Statements.RemoveRange(statements);
            await _expensesContext.SaveChangesAsync();

            return statements;
        }

        public Statement GetByDate(Guid statementId, DateTime date)
        {
            return
                _expensesContext.Statements
                .Where(s => s.InvoiceId == statementId && s.Date == date)
                .FirstOrDefault();
        }

        public Statement GetById(Guid id)
        {
            return _expensesContext.Statements
                .Include(s => s.Invoice)
                .Where(i => i.Id == id)
                .FirstOrDefault();
        }

        public async Task<IReadOnlyList<Statement>> GetListAsync(GetStatementListQuery query)
        {
            var statementList = _expensesContext.Statements.AsQueryable();

            statementList = statementList.Include(s => s.Invoice);

            if (query.InvoiceIdList.Any())
                statementList = statementList.Where(s => query.InvoiceIdList.Contains(s.InvoiceId));

            if (query.DateFrom != DateTime.MinValue)
                statementList = statementList.Where(s => s.Date >= query.DateFrom.Date);

            if (query.DateTo != DateTime.MinValue)
                statementList = statementList.Where(s => s.Date <= query.DateTo.Date);

            return await statementList.ToListAsync();
        }

        public void Update(Statement model)
        {
            _expensesContext.Statements.Update(model);
            _expensesContext.SaveChanges();
        }
    }
}
