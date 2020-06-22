using Expenses.API;
using Expenses.API.ViewModel;
using Expenses.Application.Services.Invoice.ViewModel;
using Expenses.Application.Services.Statement.ViewModel;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Expenses.IntegrationTests.API.Controller
{
    public class StatementControllerTests
        : IClassFixture<CustomWebApplicationFactorySqlite<TestStartup>>
    {
        private readonly CustomWebApplicationFactorySqlite<TestStartup> _factory;
        private readonly HttpClient _client;

        public StatementControllerTests(CustomWebApplicationFactorySqlite<TestStartup> factory)
        {
            _factory = new CustomWebApplicationFactorySqlite<TestStartup>();

            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false });
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", "true");
        }

        [Fact]
        public async Task Post_SuccessResponse()
        {
            //arrange
            var invoice = await CreateInvoiceAsync();

            CreateStatementRequest model = new CreateStatementRequest()
            {
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                Amount = 123,
                InvoiceId = invoice.Id
            };

            //act
            var response = await _client.PostAsync("/statement",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<StatementResponse>>(content);

            //assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(model.Date.Day, responseViewModel.Data.Date.Day);
            Assert.Equal(model.Date.Month, responseViewModel.Data.Date.Month);
            Assert.Equal(model.Date.Year, responseViewModel.Data.Date.Year);
            Assert.Equal(0, responseViewModel.Data.Date.Hour);
            Assert.Equal(0, responseViewModel.Data.Date.Minute);
            Assert.Equal(0, responseViewModel.Data.Date.Second);
            Assert.Equal(model.Notes, responseViewModel.Data.Notes);
            Assert.Equal(model.Amount, responseViewModel.Data.Amount);
            Assert.Equal(model.InvoiceId, responseViewModel.Data.InvoiceId);
            Assert.NotEmpty(responseViewModel.Data.Id.ToString());
        }

        [Fact]
        public async Task Post_FailureResponse_InvalidDate()
        {
            //arrange
            var invoice = await CreateInvoiceAsync();

            CreateStatementRequest model = new CreateStatementRequest()
            {
                Date = DateTime.MinValue,
                Notes = "NOTES",
                Amount = 123,
                InvoiceId = invoice.Id
            };

            //act
            var response = await _client.PostAsync("/statement",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<FailureResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("The Statement Date must be a valid date.", responseViewModel.Message);
        }

        [Fact]
        public async Task Post_FailureResponse_InvalidAmount()
        {
            //arrange
            var invoice = await CreateInvoiceAsync();

            CreateStatementRequest model = new CreateStatementRequest()
            {
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                Amount = -1,
                InvoiceId = invoice.Id
            };

            //act
            var response = await _client.PostAsync("/statement",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<FailureResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("The Statement Amount must be a positive number.", responseViewModel.Message);
        }

        [Fact]
        public async Task Post_FailureResponse_InvalidInvoiceId()
        {
            //arrange
            CreateStatementRequest model = new CreateStatementRequest()
            {
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                Amount = 123,
                InvoiceId = Guid.Empty
            };

            //act
            var response = await _client.PostAsync("/statement",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<FailureResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("The Statement Invoice Id must be a valid Guid.", responseViewModel.Message);
        }

        [Fact]
        public async Task Post_FailureResponse_InvoiceNotFound()
        {
            //arrange
            CreateStatementRequest model = new CreateStatementRequest()
            {
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                Amount = 123,
                InvoiceId = Guid.NewGuid()
            };

            //act
            var response = await _client.PostAsync("/statement",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<FailureResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Statement Invoice ID does not exist. Please select a valid value.", responseViewModel.Message);
        }

        [Fact]
        public async Task Put_SuccessResponse()
        {
            //arrange
            var invoice = await CreateInvoiceAsync();

            CreateStatementRequest createModel = new CreateStatementRequest()
            {
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                Amount = 1,
                InvoiceId = invoice.Id
            };

            var createViewModel = await CreateStatementAsync(createModel);

            var updateModel = new UpdateStatementRequest()
            {
                Id = createViewModel.Id,
                Date = DateTime.UtcNow.AddDays(1).AddMonths(1).AddYears(1),
                Notes = "NOTES2",
                Amount = 2,
                InvoiceId = invoice.Id
            };

            //act
            var response = await _client.PutAsync($"/statement/{updateModel.Id}",
                new StringContent(JsonConvert.SerializeObject(updateModel), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<StatementResponse>>(content);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updateModel.Date.Day, responseViewModel.Data.Date.Day);
            Assert.Equal(updateModel.Date.Month, responseViewModel.Data.Date.Month);
            Assert.Equal(updateModel.Date.Year, responseViewModel.Data.Date.Year);
            Assert.Equal(0, responseViewModel.Data.Date.Hour);
            Assert.Equal(0, responseViewModel.Data.Date.Minute);
            Assert.Equal(0, responseViewModel.Data.Date.Second);
            Assert.Equal(updateModel.Notes, responseViewModel.Data.Notes);
            Assert.Equal(updateModel.Amount, responseViewModel.Data.Amount);
            Assert.Equal(updateModel.InvoiceId, responseViewModel.Data.InvoiceId);
            Assert.NotEmpty(responseViewModel.Data.Id.ToString());
        }

        [Fact]
        public async Task Put_FailureResponse_DuplicateStatement()
        {
            //arrange
            var invoice = await CreateInvoiceAsync();

            CreateStatementRequest createModelFirst = new CreateStatementRequest()
            {
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                Amount = 1,
                InvoiceId = invoice.Id
            };

            var createViewModelFirst = await CreateStatementAsync(createModelFirst);

            CreateStatementRequest createModelSecond = new CreateStatementRequest()
            {
                Date = DateTime.UtcNow.AddDays(1),
                Notes = "NOTES",
                Amount = 1,
                InvoiceId = invoice.Id
            };

            var createViewModelSecond = await CreateStatementAsync(createModelSecond);

            var updateModel = new UpdateStatementRequest()
            {
                Id = createViewModelSecond.Id,
                Date = createViewModelFirst.Date,
                Notes = createViewModelSecond.Notes,
                Amount = createViewModelSecond.Amount,
                InvoiceId = createViewModelSecond.InvoiceId
            };

            //act
            var response = await _client.PutAsync($"/statement/{updateModel.Id}",
                new StringContent(JsonConvert.SerializeObject(updateModel), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<FailureResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            Assert.NotNull(responseViewModel.Message);
        }

        [Fact]
        public async Task Put_FailureResponse_NotFound()
        {
            //arrange
            var invoice = await CreateInvoiceAsync();

            CreateStatementRequest createModel = new CreateStatementRequest()
            {
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                Amount = 1,
                InvoiceId = invoice.Id
            };

            var createViewModel = await CreateStatementAsync(createModel);

            var updateModel = new UpdateStatementRequest()
            {
                Id = Guid.NewGuid(),
                Date = createViewModel.Date,
                Notes = createViewModel.Notes,
                Amount = createViewModel.Amount,
                InvoiceId = createViewModel.InvoiceId
            };

            //act
            var response = await _client.PutAsync($"/statement/{updateModel.Id}",
                new StringContent(JsonConvert.SerializeObject(updateModel), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<FailureResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull(responseViewModel.Message);
        }

        [Fact]
        public async Task GetById_SuccessResponse()
        {
            //arrange
            var invoice = await CreateInvoiceAsync();

            CreateStatementRequest createModel = new CreateStatementRequest()
            {
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                Amount = 1,
                InvoiceId = invoice.Id
            };

            var createViewModel = await CreateStatementAsync(createModel);

            //act
            var response = await _client.GetAsync($"/statement/{createViewModel.Id}");
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<StatementResponse>>(content);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(createViewModel.Date.Day, responseViewModel.Data.Date.Day);
            Assert.Equal(createViewModel.Date.Month, responseViewModel.Data.Date.Month);
            Assert.Equal(createViewModel.Date.Year, responseViewModel.Data.Date.Year);
            Assert.Equal(0, responseViewModel.Data.Date.Hour);
            Assert.Equal(0, responseViewModel.Data.Date.Minute);
            Assert.Equal(0, responseViewModel.Data.Date.Second);
            Assert.Equal(createViewModel.Notes, responseViewModel.Data.Notes);
            Assert.Equal(createViewModel.Amount, responseViewModel.Data.Amount);
            Assert.Equal(createViewModel.InvoiceId, responseViewModel.Data.InvoiceId);
            Assert.NotEmpty(responseViewModel.Data.Id.ToString());
        }

        [Fact]
        public async Task GetById_NotFound()
        {
            //arrange

            //act
            var response = await _client.GetAsync($"/statement/{Guid.NewGuid()}");
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<FailureResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull(responseViewModel.Message);
        }

        [Fact]
        public async Task Delete_SuccessResponse()
        {
            //arrange
            var invoice = await CreateInvoiceAsync();

            CreateStatementRequest createModel = new CreateStatementRequest()
            {
                Date = DateTime.UtcNow,
                Notes = "NOTES",
                Amount = 1,
                InvoiceId = invoice.Id
            };

            var createViewModel = await CreateStatementAsync(createModel);

            //act
            var response = await _client.DeleteAsync($"/statement/{createViewModel.Id}");
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<string>>(content);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_NotFound()
        {
            //arrange

            //act
            var response = await _client.DeleteAsync($"/statement/{Guid.NewGuid()}");
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<FailureResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull(responseViewModel.Message);
        }

        private async Task<InvoiceResponse> CreateInvoiceAsync()
        {
            // arrange
            CreateInvoiceRequest model = new CreateInvoiceRequest()
            {
                Name = "Name",
                Description = "Description",
                Recurrence = new InvoiceRecurrence()
                {
                    RecurrenceType = Domain.Models.RecurrenceType.Custom,
                    Start = DateTime.UtcNow,
                    Times = 1
                }
            };

            //act
            var response = await _client.PostAsync("/invoice",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                Assert.True(response.IsSuccessStatusCode, "POST /invoice/ is failing with an error. Verify");
            return JsonConvert.DeserializeObject<SuccessfulResponse<InvoiceResponse>>(content).Data;
        }

        private async Task<StatementResponse> CreateStatementAsync(CreateStatementRequest model)
        {
            var response = await _client.PostAsync("/statement",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                Assert.True(response.IsSuccessStatusCode, "POST /statement/ is failing with an error. Verify");
            return JsonConvert.DeserializeObject<SuccessfulResponse<StatementResponse>>(content).Data;
        }
    }
}
