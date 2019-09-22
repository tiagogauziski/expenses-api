using AutoMapper;
using Expenses.API;
using Expenses.API.ViewModel;
using Expenses.Application.Invoice.ViewModel;
using Expenses.Infra.EntityCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Expenses.IntegrationTests.API.Controller
{
    public class InvoiceControllerTests
        : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public InvoiceControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;

            //https://github.com/AutoMapper/AutoMapper/issues/2607
            //Mapper.Reset();

            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    //replace any services for the testing
                    services.AddDbContext<ExpensesContext>(c =>
                        c.UseInMemoryDatabase("Expenses").UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
                });
            }).CreateClient();
        }

        [Fact]
        public async Task Post_SuccessResponse()
        {
            //arrange
            CreateInvoiceRequest model = new CreateInvoiceRequest()
            {
                Name = "Name",
                Description = "Description"
            };

            //act
            var response = await _client.PostAsync("/invoice",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<InvoiceResponse>>(content);

            //assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(model.Name, responseViewModel.Data.Name);
            Assert.Equal(model.Description, responseViewModel.Data.Description);
            Assert.NotEmpty(responseViewModel.Data.Id.ToString());
        }

        [Fact]
        public async Task Post_FailureResponse_FailedValidation()
        {
            //arrange
            CreateInvoiceRequest model = new CreateInvoiceRequest()
            {
                Description = "Description"
            };

            //act
            var response = await _client.PostAsync("/invoice",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<FailureResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(responseViewModel.Message);
        }

        [Fact]
        public async Task Put_FailureResponse_NotFound()
        {
            //arrange
            UpdateInvoiceRequest model = new UpdateInvoiceRequest()
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description"
            };

            //act
            var response = await _client.PutAsync($"/invoice/{model.Id.ToString()}",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<FailureResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull(responseViewModel.Message);
        }

        [Fact]
        public async Task Put_FailureResponse_FailedValidation()
        {
            //arrange
            UpdateInvoiceRequest model = new UpdateInvoiceRequest()
            {
                Id = Guid.NewGuid(),
                Name = null,
                Description = "Description"
            };

            //act
            var response = await _client.PutAsync($"/invoice/{model.Id.ToString()}",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<FailureResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(responseViewModel.Message);
        }

        [Fact]
        public async Task Put_SuccessResponse()
        {
            //arrange
            CreateInvoiceRequest createModel = new CreateInvoiceRequest()
            {
                Name = "Name",
                Description = "Description"
            };

            

            //act
            var createResponse = await _client.PostAsync($"/invoice/",
                new StringContent(JsonConvert.SerializeObject(createModel), Encoding.UTF8, "application/json"));
            if (!createResponse.IsSuccessStatusCode)
            {
                Assert.True(createResponse.IsSuccessStatusCode, "POST /invoice/ is failing with an error");
                return;
            }

            var createContent = await createResponse.Content.ReadAsStringAsync();
            var createViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<InvoiceResponse>>(createContent);

            UpdateInvoiceRequest updateModel = new UpdateInvoiceRequest()
            {
                Id = createViewModel.Data.Id,
                Name = "Name2",
                Description = "Description"
            };

            var response = await _client.PutAsync($"/invoice/{updateModel.Id.ToString()}",
                    new StringContent(JsonConvert.SerializeObject(updateModel), Encoding.UTF8, "application/json"));

            var updateContent = await response.Content.ReadAsStringAsync();
            var updateViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<InvoiceResponse>>(updateContent);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(updateViewModel.Data);
            Assert.Equal(updateModel.Id, updateViewModel.Data.Id);
            Assert.Equal(updateModel.Name, updateViewModel.Data.Name);
            Assert.Equal(updateModel.Description, updateViewModel.Data.Description);
        }
    }
}
