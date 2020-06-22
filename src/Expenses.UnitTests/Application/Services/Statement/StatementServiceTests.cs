using Expenses.Application.AutoMapper;
using Expenses.Application.Services.Statement;
using Expenses.Application.Services.Statement.ViewModel;
using Expenses.Domain.Commands.Statement;
using Expenses.Domain.Events;
using Expenses.Domain.Events.Statement;
using Expenses.Domain.Interfaces.Repositories;
using Expenses.Infrastructure.EventBus;
using Expenses.Infrastructure.EventBus.Events;
using Moq;
using Moq.AutoMock;
using System;
using System.Net;
using Xunit;

namespace Expenses.UnitTests.Application.Sevices.Statement
{
    public class StatementServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly StatementService _statementService;

        public StatementServiceTests()
        {
            _mocker = new AutoMocker();

            var mapper = AutoMapperConfiguration.RegisterMappings().CreateMapper();
            _mocker.Use(mapper);

            _statementService = _mocker.CreateInstance<StatementService>();
        }

        [Fact]
        public async void Create_SuccessfulResult()
        {
            //arrange
            _mocker.GetMock<IEventStore>()
                .Setup(m => m.GetEvent<StatementCreatedEvent>())
                .Returns(new StatementCreatedEvent() { New = new Expenses.Domain.Models.Statement() });

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.SendCommand(It.IsAny<CreateStatementCommand>()))
                .ReturnsAsync(true);

            //act
            var result = await _statementService.Create(new CreateStatementRequest());

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
                .Setup(m => m.SendCommand(It.IsAny<CreateStatementCommand>()))
                .ReturnsAsync(false);

            //act
            var result = await _statementService.Create(new CreateStatementRequest());

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
            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.SendCommand(It.IsAny<UpdateStatementCommand>()))
                .ReturnsAsync(true);

            _mocker.GetMock<IEventStore>()
                .Setup(m => m.GetEvent<StatementUpdatedEvent>())
                .Returns(new StatementUpdatedEvent()
                {
                    New = new Expenses.Domain.Models.Statement() { },
                    Old = new Expenses.Domain.Models.Statement() { }
                });

            //act
            var result = await _statementService.Update(new UpdateStatementRequest());

            //assess
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
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
                .Setup(m => m.SendCommand(It.IsAny<UpdateStatementCommand>()))
                .ReturnsAsync(false);

            //act
            var result = await _statementService.Create(new UpdateStatementRequest());

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
            var result = await _statementService.GetById(INVALID_GUID);

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

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.GetById(It.IsAny<Guid>()))
                .Returns(value: null);

            //act
            var result = await _statementService.GetById(VALID_ID);

            //assess
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal("Statement not found", result.Error.Message);
            Assert.False(result.Successful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async void GetById_Success()
        {
            //arrange
            Guid VALID_ID = Guid.NewGuid();
            var statement = new Expenses.Domain.Models.Statement()
            {
                Id = VALID_ID,
                Date = DateTime.UtcNow,
                InvoiceId = Guid.NewGuid(),
                Notes = "NOTES",
                Amount = 123
            };

            _mocker.GetMock<IStatementRepository>()
                .Setup(m => m.GetById(It.IsAny<Guid>()))
                .Returns(value: statement);

            //act
            var result = await _statementService.GetById(VALID_ID.ToString());

            //assess
            Assert.NotNull(result.Data);
            Assert.Equal(statement.Date, result.Data.Date);
            Assert.Equal(statement.Amount, result.Data.Amount);
            Assert.Equal(statement.InvoiceId, result.Data.InvoiceId);
            Assert.Equal(statement.Notes, result.Data.Notes);
        }

        [Fact]
        public async void Delete_SuccessfulResult()
        {
            //arrange
            _mocker.GetMock<IEventStore>()
                .Setup(m => m.GetEvent<StatementDeletedEvent>())
                .Returns(new StatementDeletedEvent() { Old = new Expenses.Domain.Models.Statement() });

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.SendCommand(It.IsAny<DeleteStatementCommand>()))
                .ReturnsAsync(true);

            //act
            var result = await _statementService.Delete(Guid.NewGuid().ToString());

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
                .Setup(m => m.SendCommand(It.IsAny<DeleteStatementCommand>()))
                .ReturnsAsync(false);

            //act
            var result = await _statementService.Delete(Guid.NewGuid().ToString());

            //assess
            Assert.NotNull(result);
            Assert.NotNull(result.Error);
            Assert.Equal(ERROR_MESSAGE, result.Error.Message);
            Assert.Equal(ERROR_CODE, result.Error.ErrorCode);
            Assert.False(result.Successful);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async void UpdateAmount_SuccessfulResult()
        {
            //arrange
            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.SendCommand(It.IsAny<UpdateStatementAmountCommand>()))
                .ReturnsAsync(true);

            _mocker.GetMock<IEventStore>()
                .Setup(m => m.GetEvent<StatementUpdatedEvent>())
                .Returns(new StatementUpdatedEvent()
                {
                    New = new Expenses.Domain.Models.Statement() { },
                    Old = new Expenses.Domain.Models.Statement() { }
                });

            //act
            var result = await _statementService.UpdateAmount(new UpdateStatementAmountRequest());

            //assess
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Null(result.Error);
            Assert.True(result.Successful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async void UpdateAmount_FailureResult()
        {
            //arrange
            const string ERROR_MESSAGE = "ErrorMessage";
            const string ERROR_CODE = "ErrorCode";

            _mocker.GetMock<IEventStore>()
                .Setup(m => m.GetEvent<DomainValidationEvent>())
                .Returns(new DomainValidationEvent(ERROR_MESSAGE, ERROR_CODE));

            _mocker.GetMock<IMediatorHandler>()
                .Setup(m => m.SendCommand(It.IsAny<UpdateStatementAmountCommand>()))
                .ReturnsAsync(false);

            //act
            var result = await _statementService.UpdateAmount(new UpdateStatementAmountRequest());

            //assess
            Assert.NotNull(result);
            Assert.Null(result.Data);
            Assert.NotNull(result.Error);
            Assert.Equal(ERROR_MESSAGE, result.Error.Message);
            Assert.Equal(ERROR_CODE, result.Error.ErrorCode);
            Assert.False(result.Successful);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
