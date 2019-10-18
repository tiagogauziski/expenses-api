using Expenses.Application.AutoMapper;
using Expenses.Domain.CommandHandlers;
using Expenses.Domain.Commands;
using Expenses.Domain.Core.Bus;
using Expenses.Domain.Events;
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

namespace Expenses.UnitTests.Domain.CommandHandlers
{
    public class InvoiceCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly InvoiceCommandHandler _invoiceCommandHandler;

        public InvoiceCommandHandlerTests()
        {
            _mocker = new AutoMocker();

            var mapper = AutoMapperConfiguration.RegisterMappings().CreateMapper();
            _mocker.Use(mapper);

            _invoiceCommandHandler = _mocker.CreateInstance<InvoiceCommandHandler>();
        }

        [Fact]
        public async Task Handle_CreateInvoiceCommand_Return_True()
        {
            //arrange
            var command = new CreateInvoiceCommand()
            {
                Name = "Name",
                Description = "Description"
            };

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.Create(It.IsAny<Invoice>()))
                .Verifiable("IInvoiceRepository.Create should have been called");

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetByName(It.IsAny<string>()))
                .Returns(value: null)
                .Verifiable("IInvoiceRepository.GetByName should have been called");

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.RaiseEvent(It.IsAny<InvoiceCreatedEvent>()))
                .Verifiable("An event InvoiceCreatedEvent should have been raised");

            //act
            var result = await _invoiceCommandHandler.Handle(command, new CancellationToken());

            //assert
            _mocker.GetMock<IInvoiceRepository>().Verify();
            _mocker.GetMock<IMediatorHandler>().Verify();
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_CreateInvoiceCommand_Return_False()
        {
            //arrange
            var command = new CreateInvoiceCommand()
            {
                Name = null, //invalid name = failed validation
                Description = "Description"
            };

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.RaiseEvent(It.IsAny<DomainValidationEvent>()))
                .Verifiable("An event DomainValidationEvent should have been raised");

            //act
            var result = await _invoiceCommandHandler.Handle(command, new CancellationToken());

            //assert
            _mocker.GetMock<IMediatorHandler>().Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_CreateInvoiceCommand_Failed_DuplicateInvoice()
        {
            //arrange
            var command = new CreateInvoiceCommand()
            {
                Name = "Name",
                Description = "Description"
            };

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetByName(It.IsAny<string>()))
                .Returns(new Invoice())
                .Verifiable("IInvoiceRepository.GetByName should have been called");

            //act
            var result = await _invoiceCommandHandler.Handle(command, new CancellationToken());

            //assert
            _mocker.GetMock<IInvoiceRepository>().Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_UpdateInvoiceCommand_Return_True()
        {
            //arrange
            var command = new UpdateInvoiceCommand()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description"
            };

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(new Invoice())
                .Verifiable("IInvoiceRepository.GetById should have been called");

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetByName(It.IsAny<string>()))
                .Returns(new Invoice() { Id = command.Id, Name = command.Name })
                .Verifiable("IInvoiceRepository.GetByName should have been called");

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.Update(It.IsAny<Invoice>()))
                .Verifiable("IInvoiceRepository.Update should have been called");

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.RaiseEvent(It.IsAny<InvoiceUpdatedEvent>()))
                .Verifiable("An event InvoiceUpdatedEvent should have been raised");

            //act
            var result = await _invoiceCommandHandler.Handle(command, new CancellationToken());

            //assert
            _mocker.GetMock<IInvoiceRepository>().Verify();
            _mocker.GetMock<IMediatorHandler>().Verify();
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_UpdateInvoiceCommand_Failed_Validation()
        {
            //arrange
            var command = new UpdateInvoiceCommand()
            {
                Id = Guid.Empty, // invalid value, should failed validation
                Name = "Name",
                Description = "Description"
            };

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.RaiseEvent(It.IsAny<DomainValidationEvent>()))
                .Verifiable("An event InvoiceUpdatedEvent should have been raised");

            //act
            var result = await _invoiceCommandHandler.Handle(command, new CancellationToken());

            //assert
            _mocker.GetMock<IMediatorHandler>().Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_UpdateInvoiceCommand_Failed_DuplicateInvoice()
        {
            //arrange
            var command = new UpdateInvoiceCommand()
            {
                Id = Guid.NewGuid(), 
                Name = "Name",
                Description = "Description"
            };

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetById(It.IsAny<Guid>()))
                .Returns(new Invoice())
                .Verifiable("IInvoiceRepository.GetById should have been called");

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetByName(It.IsAny<string>()))
                .Returns(new Invoice() { Id = Guid.NewGuid(), Name = command.Name })
                .Verifiable("IInvoiceRepository.GetByName should have been called");

            //act
            var result = await _invoiceCommandHandler.Handle(command, new CancellationToken());

            //assert
            _mocker.GetMock<IInvoiceRepository>().Verify();
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_UpdateInvoiceCommand_InvoiceNotFound()
        {
            //arrange
            var command = new UpdateInvoiceCommand()
            {
                Id = Guid.NewGuid(), 
                Name = "Name",
                Description = "Description"
            };

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetById(It.IsAny<Guid>()))
                .Returns(value: null)
                .Verifiable("IInvoiceRepository.GetById should have been called");

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.RaiseEvent(It.IsAny<NotFoundEvent>()))
                .Verifiable("An event NotFoundEvent should have been raised");

            //act
            var result = await _invoiceCommandHandler.Handle(command, new CancellationToken());

            //assert
            _mocker.GetMock<IMediatorHandler>().Verify();
            Assert.False(result);
        }
    }
}
