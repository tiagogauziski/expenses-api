using Expenses.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Domain.Interfaces.Repositories
{
    public interface IStatementRepository
    {
        void Create(Statement model);

        void Delete(Guid id);

        Statement GetById(Guid id);

        Statement GetByDate(Guid statementId, DateTime date);

        void Update(Statement model);
    }
}
