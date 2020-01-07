﻿using Expenses.Application.Common;
using Expenses.Application.Statement.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Application.Statement
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStatementService
    {
        Task<Response<StatementResponse>> Create(CreateStatementRequest viewModel);

        Task<Response<StatementResponse>> Update(UpdateStatementRequest viewModel);

        Task<Response<StatementResponse>> GetById(string id);

        Task<Response<List<StatementResponse>>> GetList(GetStatementListRequest query);

        Task<Response<bool>> Delete(string id);
    }
}
