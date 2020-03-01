using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Application.Services.Statement.ViewModel
{
    public class UpdateStatementRequest : CreateStatementRequest
    {
        public Guid Id { get; set; }
    }
}
