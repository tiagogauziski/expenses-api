using Expenses.Application.AutoMapper;
using Expenses.Application.CommandHandlers;
using Expenses.Domain.Commands.Statement;
using Expenses.Domain.Core.Bus;
using Expenses.Domain.Events;
using Expenses.Domain.Events.Statement;
using Expenses.Domain.Interfaces.Repositories;
using Expenses.Domain.Models;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Expenses.UnitTests.Application.CommandHandlers
{
    public class StatementCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly StatementCommandHandler _statementCommandHandler;

        public StatementCommandHandlerTests()
        {
            _mocker = new AutoMocker();

            var mapper = AutoMapperConfiguration.RegisterMappings().CreateMapper();
            _mocker.Use(mapper);

            _statementCommandHandler = _mocker.CreateInstance<StatementCommandHandler>();
        }

        [Fact]
        public async Task Handle_CreateStatementCommand_Return_True()
        {
            //arrange
            Guid INVOICE_ID = Guid.NewGuid(); 
            var command = new CreateStatementCommand()
            {
                Value = 123.45,
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                InvoiceId = INVOICE_ID
            };

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetById(It.Is<Guid>(id => id == INVOICE_ID)))
                .Returns(value: new Invoice())
                .Verifiable("IInvoiceRepository.GetById should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.Create(It.IsAny<Statement>()))
                .Verifiable("IStatementRepository.Create should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.GetByDate(It.IsAny<Guid>(), It.IsAny<DateTime>()))
                .Returns(value: null)
                .Verifiable("IStatementRepository.GetByName should have been called");

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.RaiseEvent(It.IsAny<StatementCreatedEvent>()))
                .Verifiable("An event StatementCreatedEvent should have been raised");

            //act
            var result = await _statementCommandHandler.Handle(command, CancellationToken.None);

            //assert
            _mocker.GetMock<IStatementRepository>().Verify();
            _mocker.GetMock<IInvoiceRepository>().Verify();
            _mocker.GetMock<IMediatorHandler>().Verify();
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_CreateStatementCommand_InvalidInvoiceId()
        {
            //arrange
            Guid INVALID_INVOICE_ID = Guid.NewGuid();
            var command = new CreateStatementCommand()
            {
                Value = 123.45,
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                InvoiceId = INVALID_INVOICE_ID
            };

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetById(It.Is<Guid>(id => id == INVALID_INVOICE_ID)))
                .Returns(value: null)
                .Verifiable("IInvoiceRepository.GetById should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.Create(It.IsAny<Statement>()))
                .Verifiable("IStatementRepository.Create should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.GetByDate(It.IsAny<Guid>(), It.IsAny<DateTime>()))
                .Returns(value: null)
                .Verifiable("IStatementRepository.GetByName should have been called");

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.RaiseEvent(It.IsAny<DomainValidationEvent>()))
                .Verifiable("An event DomainValidationEvent should have been raised");

            //act
            var result = await _statementCommandHandler.Handle(command, CancellationToken.None);

            //assert
            _mocker.GetMock<IInvoiceRepository>().Verify();
            _mocker.GetMock<IMediatorHandler>().Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_CreateStatementCommand_Duplicate()
        {
            //arrange
            Guid INVALID_INVOICE_ID = Guid.NewGuid();
            var command = new CreateStatementCommand()
            {
                Value = 123.45,
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                InvoiceId = INVALID_INVOICE_ID
            };

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetById(It.Is<Guid>(id => id == INVALID_INVOICE_ID)))
                .Returns(value: null)
                .Verifiable("IInvoiceRepository.GetById should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.Create(It.IsAny<Statement>()))
                .Verifiable("IStatementRepository.Create should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.GetByDate(It.IsAny<Guid>(), It.IsAny<DateTime>()))
                .Returns(value: new Statement())
                .Verifiable("IStatementRepository.GetByName should have been called");

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.RaiseEvent(It.IsAny<DomainValidationEvent>()))
                .Verifiable("An event DomainValidationEvent should have been raised");

            //act
            var result = await _statementCommandHandler.Handle(command, CancellationToken.None);

            //assert
            _mocker.GetMock<IInvoiceRepository>().Verify();
            _mocker.GetMock<IMediatorHandler>().Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_UpdateStatementCommand_Return_True()
        {
            //arrange
            Guid INVOICE_ID = Guid.NewGuid();
            var command = new UpdateStatementCommand()
            {
                Id = Guid.NewGuid(),
                Value = 123.45,
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                InvoiceId = INVOICE_ID
            };

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetById(It.Is<Guid>(id => id == INVOICE_ID)))
                .Returns(value: new Invoice())
                .Verifiable("IInvoiceRepository.GetById should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.GetById(It.IsAny<Guid>()))
                .Returns(value: new Statement())
                .Verifiable("IStatementRepository.GetById should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.GetByDate(It.IsAny<Guid>(), It.IsAny<DateTime>()))
                .Returns(value: null)
                .Verifiable("IStatementRepository.GetByName should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.Update(It.IsAny<Statement>()))
                .Verifiable("IStatementRepository.Update should have been called");

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.RaiseEvent(It.IsAny<StatementUpdatedEvent>()))
                .Verifiable("An event StatementUpdatedEvent should have been raised");

            //act
            var result = await _statementCommandHandler.Handle(command, CancellationToken.None);

            //assert
            _mocker.GetMock<IStatementRepository>().Verify();
            _mocker.GetMock<IInvoiceRepository>().Verify();
            _mocker.GetMock<IMediatorHandler>().Verify();
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_UpdateStatementCommand_InvalidInvoiceId()
        {
            //arrange
            Guid INVALID_INVOICE_ID = Guid.NewGuid();
            var command = new UpdateStatementCommand()
            {
                Id = Guid.NewGuid(),
                Value = 123.45,
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                InvoiceId = INVALID_INVOICE_ID
            };

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetById(It.Is<Guid>(id => id == INVALID_INVOICE_ID)))
                .Returns(value: null)
                .Verifiable("IInvoiceRepository.GetById should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.GetById(It.IsAny<Guid>()))
                .Returns(value: new Statement())
                .Verifiable("IStatementRepository.GetById should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.GetByDate(It.IsAny<Guid>(), It.IsAny<DateTime>()))
                .Returns(value: null)
                .Verifiable("IStatementRepository.GetByName should have been called");

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.RaiseEvent(It.IsAny<DomainValidationEvent>()))
                .Verifiable("An event InvoiceCreatedEvent should have been raised");

            //act
            var result = await _statementCommandHandler.Handle(command, CancellationToken.None);

            //assert
            _mocker.GetMock<IInvoiceRepository>().Verify();
            _mocker.GetMock<IMediatorHandler>().Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_UpdateStatementCommand_InvalidStatementId()
        {
            //arrange
            Guid INVALID_INVOICE_ID = Guid.NewGuid();
            var command = new UpdateStatementCommand()
            {
                Id = Guid.NewGuid(),
                Value = 123.45,
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                InvoiceId = INVALID_INVOICE_ID
            };

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetById(It.Is<Guid>(id => id == INVALID_INVOICE_ID)))
                .Returns(value: null)
                .Verifiable("IInvoiceRepository.GetById should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.GetByDate(It.IsAny<Guid>(), It.IsAny<DateTime>()))
                .Returns(value: null)
                .Verifiable("IStatementRepository.GetByName should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.GetById(It.IsAny<Guid>()))
                .Returns(value: new Statement())
                .Verifiable("IStatementRepository.GetById should have been called");

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.RaiseEvent(It.IsAny<DomainValidationEvent>()))
                .Verifiable("An event InvoiceCreatedEvent should have been raised");

            //act
            var result = await _statementCommandHandler.Handle(command, CancellationToken.None);

            //assert
            _mocker.GetMock<IInvoiceRepository>().Verify();
            _mocker.GetMock<IMediatorHandler>().Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_UpdateStatementCommand_Duplicate()
        {
            //arrange
            Guid INVALID_INVOICE_ID = Guid.NewGuid();
            var command = new UpdateStatementCommand()
            {
                Id = Guid.NewGuid(),
                Value = 123.45,
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                InvoiceId = INVALID_INVOICE_ID
            };

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetById(It.Is<Guid>(id => id == INVALID_INVOICE_ID)))
                .Returns(value: new Invoice())
                .Verifiable("IInvoiceRepository.GetById should have been called");

            _mocker.GetMock<IStatementRepository>()
               .Setup(m => m.GetById(It.IsAny<Guid>()))
               .Returns(value: new Statement())
               .Verifiable("IStatementRepository.GetById should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.GetByDate(It.IsAny<Guid>(), It.IsAny<DateTime>()))
                .Returns(value: new Statement())
                .Verifiable("IStatementRepository.GetByName should have been called");

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.RaiseEvent(It.IsAny<DomainValidationEvent>()))
                .Verifiable("An event DomainValidationEvent should have been raised");

            //act
            var result = await _statementCommandHandler.Handle(command, CancellationToken.None);

            //assert
            _mocker.GetMock<IStatementRepository>().Verify();
            _mocker.GetMock<IMediatorHandler>().Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_DeletetatementCommand_InvalidId()
        {
            //arrange
            var command = new DeleteStatementCommand()
            {
                Id = Guid.NewGuid()
            };

            _mocker.GetMock<IStatementRepository>()
               .Setup(m => m.GetById(It.IsAny<Guid>()))
               .Returns(value: null)
               .Verifiable("IStatementRepository.GetById should have been called");
            
            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.RaiseEvent(It.IsAny<DomainValidationEvent>()))
                .Verifiable("An event DomainValidationEvent should have been raised");

            //act
            var result = await _statementCommandHandler.Handle(command, CancellationToken.None);

            //assert
            _mocker.GetMock<IStatementRepository>().Verify();
            _mocker.GetMock<IMediatorHandler>().Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_DeleteStatementCommand_Return_True()
        {
            //arrange
            var command = new DeleteStatementCommand()
            {
                Id = Guid.NewGuid()
            };

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.GetById(It.IsAny<Guid>()))
                .Returns(value: new Statement())
                .Verifiable("IStatementRepository.GetById should have been called");

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.Delete(It.IsAny<Guid>()))
                .Verifiable("IStatementRepository.Update should have been called");

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.RaiseEvent(It.IsAny<StatementDeletedEvent>()))
                .Verifiable("An event StatementDeletedEvent should have been raised");

            //act
            var result = await _statementCommandHandler.Handle(command, CancellationToken.None);

            //assert
            _mocker.GetMock<IStatementRepository>().Verify();
            _mocker.GetMock<IMediatorHandler>().Verify();
            Assert.True(result);
        }
    }
}
