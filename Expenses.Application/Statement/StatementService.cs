using AutoMapper;
using Expenses.Application.Common;
using Expenses.Application.Statement.ViewModel;
using Expenses.Domain.Core.Bus;
using Expenses.Domain.Core.Events;
using Expenses.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Application.Statement
{
    public class StatementService : IStatementService
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMapper _mapper;
        private readonly IEventStore _eventStore;
        private readonly IStatementRepository _statementRepository;

        public StatementService(
            IMediatorHandler mediatorHandler,
            IMapper mapper,
            IEventStore eventStore,
            IStatementRepository statementRepository)
        {
            _mediatorHandler = mediatorHandler;
            _mapper = mapper;
            _eventStore = eventStore;
            _statementRepository = statementRepository;
        }

        public Task<Response<StatementResponse>> Create(CreateStatementRequest viewModel)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<StatementResponse>> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<StatementResponse>>> GetList(GetStatementListRequest query)
        {
            throw new NotImplementedException();
        }

        public Task<Response<StatementResponse>> Update(UpdateStatementRequest viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
