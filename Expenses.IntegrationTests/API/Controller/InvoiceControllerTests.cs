using AutoMapper;
using Expenses.API;
using Expenses.Application.Invoice.ViewModel;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
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
                });
            }).CreateClient();
        }

        [Fact]
        public async Task Post_SuccessResponse()
        {
            //arrange
            InvoiceRequest model = new InvoiceRequest()
            {
                Name = "Name",
                Description = "Description"
            };

            //act
            var response = await _client.PostAsync("/invoice", 
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<InvoiceResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(model.Name, responseViewModel.Name);
            Assert.Equal(model.Description, responseViewModel.Description);
            Assert.NotEmpty(responseViewModel.Id.ToString());
        }
    }
}
