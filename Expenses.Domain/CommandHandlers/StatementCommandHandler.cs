using AutoMapper;
using Expenses.Domain.Commands.Statement;
using Expenses.Domain.Core.Bus;
using Expenses.Domain.Events;
using Expenses.Domain.Events.Statement;
using Expenses.Domain.Interfaces.Repositories;
using Expenses.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Expenses.Domain.CommandHandlers
{
    public class StatementCommandHandler : 
        IRequestHandler<CreateStatementCommand, bool>,
        IRequestHandler<UpdateStatementCommand, bool>,
        IRequestHandler<DeleteStatementCommand, bool>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMapper _mapper;
        private readonly IStatementRepository _statementRepository;

        public StatementCommandHandler(IMediatorHandler mediatorHandler,
            IMapper mapper,
            IStatementRepository statementRepository)
        {
            _mediatorHandler = mediatorHandler;
            _mapper = mapper;
            _statementRepository = statementRepository;
        }

        public async Task<bool> Handle(CreateStatementCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await _mediatorHandler.RaiseEvent(
                    new DomainValidationEvent(request.ValidationResult.ToString()));
                return false;
            }

            var getByDate = _statementRepository.GetByDate(request.InvoiceId, request.Date);
            if (getByDate != null)
            {
                await _mediatorHandler.RaiseEvent(
                    new DuplicatedRecordEvent(
                        "Multiple", 
                        "Statement", 
                        "A Stement with {0} and {1} is already present. Please select another value."));
                return false;
            }

            var model = _mapper.Map<Statement>(request);

            _statementRepository.Create(model);

            await _mediatorHandler.RaiseEvent(new StatementCreatedEvent()
            {
                New = model
            });

            return true;
        }

        public async Task<bool> Handle(UpdateStatementCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await _mediatorHandler.RaiseEvent(
                    new DomainValidationEvent(request.ValidationResult.ToString()));
                return false;
            }

            var oldStatement = _statementRepository.GetById(request.Id);
            if (oldStatement == null)
            {
                await _mediatorHandler.RaiseEvent(
                    new NotFoundEvent(request.Id, "Statement", "Statement not found"));
                return false;
            }

            var getByDate = _statementRepository.GetByDate(request.InvoiceId, request.Date);
            if (getByDate != null && getByDate.Id != request.Id)
            {
                await _mediatorHandler.RaiseEvent(
                    new DuplicatedRecordEvent(
                        "Name", 
                        "Statement",
                        "A Stement with {0} and {1} is already present. Please select another value."));
                return false;
            }

            var model = _mapper.Map<Statement>(request);

            _statementRepository.Update(model);

            await _mediatorHandler.RaiseEvent(new StatementUpdatedEvent()
            {
                Old = oldStatement,
                New = model
            });

            return true;
        }

        public async Task<bool> Handle(DeleteStatementCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                await _mediatorHandler.RaiseEvent(
                    new DomainValidationEvent(request.ValidationResult.ToString()));
                return false;
            }

            var oldStatement = _statementRepository.GetById(request.Id);
            if (oldStatement == null)
            {
                await _mediatorHandler.RaiseEvent(new NotFoundEvent(request.Id, "Statement", "Statement not found"));
                return false;
            }

            _statementRepository.Delete(request.Id);

            await _mediatorHandler.RaiseEvent(new StatementDeletedEvent()
            {
                Old = oldStatement
            });

            return true;
        }
    }
}
