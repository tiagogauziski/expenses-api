﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Expenses.Application.Statement.ViewModel
{
    public class UpdateStatementRequest : CreateStatementRequest
    {
        public Guid Id { get; set; }
    }
}
