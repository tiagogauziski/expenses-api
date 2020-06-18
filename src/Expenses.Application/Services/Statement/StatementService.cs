using AutoMapper;
using Expenses.Application.Common;
using Expenses.Application.Services.Statement.ViewModel;
using Expenses.Domain.Commands.Statement;
using Expenses.Domain.Events;
using Expenses.Domain.Events.Statement;
using Expenses.Domain.Interfaces.Repositories;
using Expenses.Domain.Queries.Statement;
using Expenses.Infrastructure.EventBus;
using Expenses.Infrastructure.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Expenses.Application.Services.Statement
{
    public class StatementService : BaseService, IStatementService
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

        public async Task<Response<StatementResponse>> Create(CreateStatementRequest viewModel)
        {
            var command = _mapper.Map<CreateStatementCommand>(viewModel);

            var result = await _mediatorHandler.SendCommand(command);

            if (result)
            {
                var statementEvent = _eventStore.GetEvent<StatementCreatedEvent>();

                var data = _mapper.Map<Expenses.Domain.Models.Statement, StatementResponse>(statementEvent.New);

                return SuccessfulResponse(data, statementEvent);
            }
            else
            {
                var validationEvent = _eventStore.GetEvent<DomainValidationEvent>();

                return FailureResponse<StatementResponse>(validationEvent);
            }
        }

        public async Task<Response<bool>> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
                return FailureResponse<bool>(
                    new Error("Invalid Guid"), 
                    System.Net.HttpStatusCode.BadRequest);

            var command = new DeleteStatementCommand();
            command.Id = guid;

            var result = await _mediatorHandler.SendCommand(command);

            if (result)
            {
                var deletedEvent = _eventStore.GetEvent<StatementDeletedEvent>();

                return SuccessfulResponse(true, deletedEvent);
            }
            else
            {
                var validationEvent = _eventStore.GetEvent<DomainValidationEvent>();

                return FailureResponse<bool>(validationEvent);
            }
        }

        public async Task<Response<StatementResponse>> GetById(string id)
        {
            if (!Guid.TryParse(id, out Guid guid))
                return FailureResponse<StatementResponse>(
                    new Error("Invalid Guid"),
                    System.Net.HttpStatusCode.BadRequest);

            var result = _statementRepository.GetById(guid);

            if (result == null)
                return FailureResponse<StatementResponse>(
                    new Error("Statement not found"),
                    System.Net.HttpStatusCode.NotFound);

            var data = _mapper.Map<Expenses.Domain.Models.Statement, StatementResponse>(result);

            return SuccessfulResponse(data);
        }

        public async Task<Response<IReadOnlyList<StatementResponse>>> GetList(GetStatementListRequest request)
        {
            var query = _mapper.Map<GetStatementListQuery>(request);

            var result = await _statementRepository.GetListAsync(query);

            var data = _mapper.Map<IReadOnlyList<Expenses.Domain.Models.Statement>, IReadOnlyList<StatementResponse>>(result.ToList());

            return SuccessfulResponse(data);
        }

        public async Task<Response<StatementResponse>> Update(UpdateStatementRequest viewModel)
        {
            var command = _mapper.Map<UpdateStatementCommand>(viewModel);

            var result = await _mediatorHandler.SendCommand(command);

            if (result)
            {
                var updateEvent = _eventStore.GetEvent<StatementUpdatedEvent>();

                var data = _mapper.Map<Expenses.Domain.Models.Statement, StatementResponse>(updateEvent.New);

                return SuccessfulResponse(data, updateEvent);
            }
            else
            {
                var validationEvent = _eventStore.GetEvent<DomainValidationEvent>();

                return FailureResponse<StatementResponse>(validationEvent);
            }
        }

        public async Task<Response<StatementResponse>> UpdateAmount(UpdateStatementAmountRequest viewModel)
        {
            var command = _mapper.Map<UpdateStatementAmountCommand>(viewModel);

            var result = await _mediatorHandler.SendCommand(command);

            if (result)
            {
                var updateEvent = _eventStore.GetEvent<StatementUpdatedEvent>();

                var data = _mapper.Map<Expenses.Domain.Models.Statement, StatementResponse>(updateEvent.New);

                return SuccessfulResponse(data, updateEvent);
            }
            else
            {
                var validationEvent = _eventStore.GetEvent<DomainValidationEvent>();

                return FailureResponse<StatementResponse>(validationEvent);
            }
        }
    }
}
