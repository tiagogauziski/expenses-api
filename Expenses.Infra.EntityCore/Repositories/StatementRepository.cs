using Expenses.Domain.Interfaces.Repositories;
using Expenses.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Expenses.Infra.EntityCore.Repositories
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

        public Statement GetById(Guid id)
        {
            return _expensesContext.Statements.Where(i => i.Id == id).FirstOrDefault();
        }

        public void Update(Statement model)
        {
            _expensesContext.Statements.Update(model);
            _expensesContext.SaveChanges();
        }
    }
}
