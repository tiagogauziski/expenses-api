using AutoMapper;
using Expenses.Application.AutoMapper;
using Expenses.Application.Invoice;
using Expenses.Application.Invoice.ViewModel;
using Expenses.Domain.Commands;
using Expenses.Domain.Core.Bus;
using Expenses.Domain.Core.Events;
using Expenses.Domain.Events;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Expenses.UnitTests.Application.Invoice
{
    public class InvoiceServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly InvoiceService _invoiceService;

        public InvoiceServiceTests()
        {
            _mocker = new AutoMocker();

            var mapper = AutoMapperConfiguration.RegisterMappings().CreateMapper();
            _mocker.Use(mapper);

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.SendCommand(It.IsAny<CreateInvoiceCommand>()))
                .ReturnsAsync(true);

            _invoiceService = _mocker.CreateInstance<InvoiceService>();
        }

        [Fact]
        public async void Create_SuccessfulResult()
        {
            //arrange
            _mocker.GetMock<IEventStore>()
                .Setup(m => m.GetEvent<InvoiceCreatedEvent>())
                .Returns(new InvoiceCreatedEvent());

            //act
            var result = await _invoiceService.Create(new InvoiceRequest());

            //assess
            Assert.NotNull(result);
        }

        [Fact]
        public async void Create_FailureResult()
        {
            //arrange
            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.SendCommand(It.IsAny<CreateInvoiceCommand>()))
                .ReturnsAsync(false);

            //act
            var result = await _invoiceService.Create(new InvoiceRequest());

            //assess
            Assert.Null(result);
        }
    }
}
