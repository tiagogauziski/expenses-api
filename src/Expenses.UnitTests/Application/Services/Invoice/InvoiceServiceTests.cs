using AutoMapper;
using Expenses.Application.AutoMapper;
using Expenses.Application.Services.Invoice;
using Expenses.Application.Services.Invoice.ViewModel;
using Expenses.Domain.Commands.Invoice;
using Expenses.Domain.Core.Bus;
using Expenses.Domain.Core.Events;
using Expenses.Domain.Events;
using Expenses.Domain.Events.Invoice;
using Expenses.Domain.Interfaces.Repositories;
using Expenses.Domain.Queries.Invoice;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Expenses.UnitTests.Application.Sevices.Invoice
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

            

            _invoiceService = _mocker.CreateInstance<InvoiceService>();
        }

        [Fact]
        public async void Create_SuccessfulResult()
        {
            //arrange
            _mocker.GetMock<IEventStore>()
                .Setup(m => m.GetEvent<InvoiceCreatedEvent>())
                .Returns(new InvoiceCreatedEvent() { New = new Expenses.Domain.Models.Invoice() });

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.SendCommand(It.IsAny<CreateInvoiceCommand>()))
                .ReturnsAsync(true);

            //act
            var result = await _invoiceService.Create(new CreateInvoiceRequest());

            //assess
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Null(result.Error);
            Assert.True(result.Successful);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        }

        [Fact]
        public async void Create_FailureResult()
        {
            //arrange
            const string ERROR_MESSAGE = "ErrorMessage";
            const string ERROR_CODE = "ErrorCode";

            _mocker.GetMock<IEventStore>()
                .Setup(m => m.GetEvent<DomainValidationEvent>())
                .Returns(new DomainValidationEvent(ERROR_MESSAGE, ERROR_CODE));

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.SendCommand(It.IsAny<CreateInvoiceCommand>()))
                .ReturnsAsync(false);

            //act
            var result = await _invoiceService.Create(new CreateInvoiceRequest());

            //assess
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal(ERROR_MESSAGE, result.Error.Message);
            Assert.Equal(ERROR_CODE, result.Error.ErrorCode);
            Assert.False(result.Successful);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async void Update_SuccessfulResult()
        {
            //arrange
            const string NEW_INVOICE = "New";
            const string OLD_INVOICE = "Old";

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.SendCommand(It.IsAny<UpdateInvoiceCommand>()))
                .ReturnsAsync(true);

            _mocker.GetMock<IEventStore>()
                .Setup(m => m.GetEvent<InvoiceUpdatedEvent>())
                .Returns(new InvoiceUpdatedEvent() {
                    New = new Expenses.Domain.Models.Invoice() { Name = NEW_INVOICE },
                    Old = new Expenses.Domain.Models.Invoice() { Name = OLD_INVOICE }
                });

            //act
            var result = await _invoiceService.Update(new UpdateInvoiceRequest());

            //assess
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(NEW_INVOICE, result.Data.Name);
            Assert.Null(result.Error);
            Assert.True(result.Successful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async void Update_FailureResult()
        {
            //arrange
            const string ERROR_MESSAGE = "ErrorMessage";
            const string ERROR_CODE = "ErrorCode";

            _mocker.GetMock<IEventStore>()
                .Setup(m => m.GetEvent<DomainValidationEvent>())
                .Returns(new DomainValidationEvent(ERROR_MESSAGE, ERROR_CODE));

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.SendCommand(It.IsAny<UpdateInvoiceCommand>()))
                .ReturnsAsync(false);

            //act
            var result = await _invoiceService.Create(new UpdateInvoiceRequest());

            //assess
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal(ERROR_MESSAGE, result.Error.Message);
            Assert.Equal(ERROR_CODE, result.Error.ErrorCode);
            Assert.False(result.Successful);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async void GetById_InvalidGuid_Validation()
        {
            //arrange
            const string INVALID_GUID = "123";

            //act
            var result = await _invoiceService.GetById(INVALID_GUID);

            //assess
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal("Invalid Guid", result.Error.Message);
            Assert.False(result.Successful);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async void GetById_NotFound_Validation()
        {
            //arrange
            string VALID_ID = Guid.NewGuid().ToString();

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetById(It.IsAny<Guid>()))
                .Returns(value: null);

            //act
            var result = await _invoiceService.GetById(VALID_ID);

            //assess
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal("Invoice not found", result.Error.Message);
            Assert.False(result.Successful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async void GetById_Success()
        {
            //arrange
            Guid VALID_ID = Guid.NewGuid();
            var invoice = new Expenses.Domain.Models.Invoice()
            {
                Id = VALID_ID,
                Name = "Name",
                Description = "Description"
            };

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetById(It.IsAny<Guid>()))
                .Returns(value: invoice);

            //act
            var result = await _invoiceService.GetById(VALID_ID.ToString());

            //assess
            Assert.NotNull(result.Data);
            Assert.Equal(invoice.Description, result.Data.Description);
            Assert.Equal(invoice.Name, result.Data.Name);
            Assert.Equal(invoice.Id, result.Data.Id);
        }

        [Fact]
        public async void GetList_Success()
        {
            //arrange
            Guid VALID_ID = Guid.NewGuid();
            var invoice = new Expenses.Domain.Models.Invoice()
            {
                Id = VALID_ID,
                Name = "Name",
                Description = "Description"
            };

            _mocker.GetMock<IInvoiceRepository>()
                .Setup(m => m.GetList(It.IsAny<GetInvoiceListQuery>()))
                .Returns(value: new List<Expenses.Domain.Models.Invoice>() { invoice });

            //act
            var result = await _invoiceService.GetList(new GetInvoiceListRequest());

            //assess
            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);
            Assert.Equal(invoice.Description, result.Data[0].Description);
            Assert.Equal(invoice.Name, result.Data[0].Name);
            Assert.Equal(invoice.Id, result.Data[0].Id);
        }

        [Fact]
        public async void Delete_SuccessfulResult()
        {
            //arrange
            _mocker.GetMock<IEventStore>()
                .Setup(m => m.GetEvent<InvoiceDeletedEvent>())
                .Returns(new InvoiceDeletedEvent() { Old = new Expenses.Domain.Models.Invoice() });

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.SendCommand(It.IsAny<DeleteInvoiceCommand>()))
                .ReturnsAsync(true);

            //act
            var result = await _invoiceService.Delete(Guid.NewGuid().ToString());

            //assess
            Assert.NotNull(result);
            Assert.True(result.Successful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async void Delete_FailureResult()
        {
            //arrange
            const string ERROR_MESSAGE = "ErrorMessage";
            const string ERROR_CODE = "ErrorCode";

            _mocker.GetMock<IEventStore>()
                .Setup(m => m.GetEvent<DomainValidationEvent>())
                .Returns(new DomainValidationEvent(ERROR_MESSAGE, ERROR_CODE));

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.SendCommand(It.IsAny<DeleteInvoiceCommand>()))
                .ReturnsAsync(false);

            //act
            var result = await _invoiceService.Delete(Guid.NewGuid().ToString());

            //assess
            Assert.NotNull(result);
            Assert.NotNull(result.Error);
            Assert.Equal(ERROR_MESSAGE, result.Error.Message);
            Assert.Equal(ERROR_CODE, result.Error.ErrorCode);
            Assert.False(result.Successful);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
