using Expenses.Domain.Models;
using Expenses.Domain.Queries.Statement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Domain.Interfaces.Repositories
{
    public interface IStatementRepository
    {
        void Create(Statement model);

        void Delete(Guid id);

        Statement GetById(Guid id);

        Statement GetByDate(Guid statementId, DateTime date);

        Task<IReadOnlyList<Statement>> GetListAsync(GetStatementListQuery query);

        void Update(Statement model);
    }
}
