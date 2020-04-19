using Expenses.Application.Common;
using Expenses.Application.Services.Statement.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Application.Services.Statement
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStatementService
    {
        Task<Response<StatementResponse>> Create(CreateStatementRequest viewModel);

        Task<Response<StatementResponse>> Update(UpdateStatementRequest viewModel);

        Task<Response<StatementResponse>> UpdateAmount(UpdateStatementAmountRequest viewModel);

        Task<Response<StatementResponse>> GetById(string id);

        Task<Response<IReadOnlyList<StatementResponse>>> GetList(GetStatementListRequest query);

        Task<Response<bool>> Delete(string id);
    }
}
