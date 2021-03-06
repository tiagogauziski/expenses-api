﻿using Expenses.API;
using Expenses.API.ViewModel;
using Expenses.Application.Services.Invoice.ViewModel;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Expenses.IntegrationTests.API.Controller
{
    public class InvoiceControllerTests
        : IClassFixture<CustomWebApplicationFactorySqlite<TestStartup>>
    {
        private readonly CustomWebApplicationFactorySqlite<TestStartup> _factory;
        private readonly HttpClient _client;

        public InvoiceControllerTests(CustomWebApplicationFactorySqlite<TestStartup> factory)
        {
            _factory = new CustomWebApplicationFactorySqlite<TestStartup>();

            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false });
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test", "true");
        }

        [Fact]
        public async Task Post_SuccessResponse()
        {
            //arrange
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
            var responseViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<InvoiceResponse>>(content);

            //assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(model.Name, responseViewModel.Data.Name);
            Assert.Equal(model.Description, responseViewModel.Data.Description);
            Assert.Equal(model.Recurrence.RecurrenceType, responseViewModel.Data.Recurrence.RecurrenceType);
            Assert.Equal(model.Recurrence.Start, responseViewModel.Data.Recurrence.Start);
            Assert.Equal(model.Recurrence.Times, responseViewModel.Data.Recurrence.Times);
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
        public async Task Post_FailureResponse_DuplicateValidation()
        {
            //arrange
            CreateInvoiceRequest model = new CreateInvoiceRequest()
            {
                Name = "Name",
                Description = "Description"
            };

            //act
            //call it once
            var response = await _client.PostAsync("/invoice",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
                Assert.True(response.IsSuccessStatusCode, "First call failed, should succeed. Verify");

            //call it twice
            response = await _client.PostAsync("/invoice",
                new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<FailureResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            Assert.NotNull(responseViewModel.Message);
        }

        [Fact]
        public async Task Post_FailureResponse_RecurrenceCustomTimes()
        {
            //arrange
            CreateInvoiceRequest model = new CreateInvoiceRequest()
            {
                Name = "Name",
                Description = "Description",
                Recurrence = new InvoiceRecurrence()
                {
                    RecurrenceType = Domain.Models.RecurrenceType.Custom
                }
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
        public async Task Post_FailureResponse_RecurrenceCustomStart()
        {
            //arrange
            CreateInvoiceRequest model = new CreateInvoiceRequest()
            {
                Name = "Name",
                Description = "Description",
                Recurrence = new InvoiceRecurrence()
                {
                    RecurrenceType = Domain.Models.RecurrenceType.Custom,
                    Times = 3
                }
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
        public async Task Put_FailureResponse_DuplicateValidation()
        {
            //arrange
            CreateInvoiceRequest createModel = new CreateInvoiceRequest()
            {
                Name = "Name",
                Description = "Description"
            };

            CreateInvoiceRequest createModelSecond = new CreateInvoiceRequest()
            {
                Name = "NameSecond",
                Description = "Description"
            };


            //act
            var createResponse = await _client.PostAsync($"/invoice/",
                new StringContent(JsonConvert.SerializeObject(createModelSecond), Encoding.UTF8, "application/json"));
            if (!createResponse.IsSuccessStatusCode)
                Assert.True(createResponse.IsSuccessStatusCode, "POST /invoice/ is failing with an error. Verify");

            createResponse = await _client.PostAsync($"/invoice/",
                new StringContent(JsonConvert.SerializeObject(createModel), Encoding.UTF8, "application/json"));
            if (!createResponse.IsSuccessStatusCode)
                Assert.True(createResponse.IsSuccessStatusCode, "POST /invoice/ is failing with an error. Verify");


            var createContent = await createResponse.Content.ReadAsStringAsync();
            var createViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<InvoiceResponse>>(createContent);

            UpdateInvoiceRequest updateModel = new UpdateInvoiceRequest()
            {
                Id = createViewModel.Data.Id,
                Name = createModelSecond.Name,
                Description = "Description"
            };

            var response = await _client.PutAsync($"/invoice/{updateModel.Id.ToString()}",
                    new StringContent(JsonConvert.SerializeObject(updateModel), Encoding.UTF8, "application/json"));

            var updateContent = await response.Content.ReadAsStringAsync();
            var updateViewModel = JsonConvert.DeserializeObject<FailureResponse>(updateContent);

            //assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            Assert.NotNull(updateViewModel.Message);
        }

        [Fact]
        public async Task Put_SuccessResponse()
        {
            //arrange
            CreateInvoiceRequest createModel = new CreateInvoiceRequest()
            {
                Name = "Name",
                Description = "Description",
                Recurrence = new InvoiceRecurrence()
                {
                    RecurrenceType = Domain.Models.RecurrenceType.Monthly
                }
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
                Description = "Description",
                Recurrence = new InvoiceRecurrence()
                {
                    RecurrenceType = Domain.Models.RecurrenceType.Custom,
                    Start = DateTime.UtcNow,
                    Times = 3
                }
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
            Assert.Equal(updateModel.Recurrence.RecurrenceType, updateViewModel.Data.Recurrence.RecurrenceType);
            Assert.Equal(updateModel.Recurrence.Start, updateViewModel.Data.Recurrence.Start);
            Assert.Equal(updateModel.Recurrence.Times, updateViewModel.Data.Recurrence.Times);
        }

        [Fact]
        public async Task GetById_FailureResponse_NotFound()
        {
            //arrange
            Guid randomGuid = Guid.NewGuid();

            //act
            var response = await _client.GetAsync($"/invoice/{randomGuid.ToString()}");
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<FailureResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.NotNull(responseViewModel.Message);
        }

        [Fact]
        public async Task GetById_FailureResponse_FailedValidation()
        {
            //arrange
            string invalidGuid = "INVALID_GUID";

            //act
            var response = await _client.GetAsync($"/invoice/{invalidGuid.ToString()}");
            var content = await response.Content.ReadAsStringAsync();
            var responseViewModel = JsonConvert.DeserializeObject<FailureResponse>(content);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(responseViewModel.Message);
        }

        [Fact]
        public async Task GetById_SuccessResponse()
        {
            //arrange
            CreateInvoiceRequest createModel = new CreateInvoiceRequest()
            {
                Name = "Name",
                Description = "Description",
                Recurrence = new InvoiceRecurrence()
                {
                    RecurrenceType = Domain.Models.RecurrenceType.Custom,
                    Start = DateTime.UtcNow,
                    Times = 3
                }
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

            var response = await _client.GetAsync($"/invoice/{createViewModel.Data.Id.ToString()}");

            var getContent = await response.Content.ReadAsStringAsync();
            var getViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<InvoiceResponse>>(getContent);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(getViewModel.Data);
            Assert.Equal(createViewModel.Data.Id, getViewModel.Data.Id);
            Assert.Equal(createViewModel.Data.Name, getViewModel.Data.Name);
            Assert.Equal(createViewModel.Data.Description, getViewModel.Data.Description);
            Assert.Equal(createViewModel.Data.Recurrence.RecurrenceType, getViewModel.Data.Recurrence.RecurrenceType);
            Assert.Equal(createViewModel.Data.Recurrence.Start, getViewModel.Data.Recurrence.Start);
            Assert.Equal(createViewModel.Data.Recurrence.Times, getViewModel.Data.Recurrence.Times);

        }

        [Fact]
        public async Task GetList_ReturnList()
        {
            //arrange
            CreateInvoiceRequest createModel = new CreateInvoiceRequest()
            {
                Name = "Name",
                Description = "Description",
                Recurrence = new InvoiceRecurrence()
                {
                    RecurrenceType = Domain.Models.RecurrenceType.Custom,
                    Start = DateTime.UtcNow,
                    Times = 3
                }
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

            var response = await _client.GetAsync($"/invoice/");

            var getContent = await response.Content.ReadAsStringAsync();
            var getViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<List<InvoiceResponse>>>(getContent);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(getViewModel.Data);
            Assert.NotEmpty(getViewModel.Data);
            Assert.Equal(createViewModel.Data.Id, getViewModel.Data[0].Id);
            Assert.Equal(createViewModel.Data.Name, getViewModel.Data[0].Name);
            Assert.Equal(createViewModel.Data.Description, getViewModel.Data[0].Description);
            Assert.Equal(createViewModel.Data.Recurrence.RecurrenceType, getViewModel.Data[0].Recurrence.RecurrenceType);
            Assert.Equal(createViewModel.Data.Recurrence.Start, getViewModel.Data[0].Recurrence.Start);
            Assert.Equal(createViewModel.Data.Recurrence.Times, getViewModel.Data[0].Recurrence.Times);
        }

        [Fact]
        public async Task GetList_ReturnNone()
        {
            //arrange
            CreateInvoiceRequest createModel = new CreateInvoiceRequest()
            {
                Name = "Name",
                Description = "Description"
            };
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryString["name"] = "name1";
            queryString["description"] = "description1";

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

            var response = await _client.GetAsync($"/invoice?{queryString.ToString()}");

            var getContent = await response.Content.ReadAsStringAsync();
            var getViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<List<InvoiceResponse>>>(getContent);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(getViewModel.Data);
            Assert.Empty(getViewModel.Data);
        }

        [Fact]
        public async Task GetList_QueryName()
        {
            //arrange
            CreateInvoiceRequest createModel = new CreateInvoiceRequest()
            {
                Name = "Name",
                Description = "Description"
            };
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryString["name"] = "name";

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

            var response = await _client.GetAsync($"/invoice?{queryString.ToString()}");

            var getContent = await response.Content.ReadAsStringAsync();
            var getViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<List<InvoiceResponse>>>(getContent);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(getViewModel.Data);
            Assert.NotEmpty(getViewModel.Data);
            Assert.Equal(createViewModel.Data.Id, getViewModel.Data[0].Id);
            Assert.Equal(createViewModel.Data.Name, getViewModel.Data[0].Name);
            Assert.Equal(createViewModel.Data.Description, getViewModel.Data[0].Description);
        }

        [Fact]
        public async Task GetList_QueryDescription()
        {
            //arrange
            CreateInvoiceRequest createModel = new CreateInvoiceRequest()
            {
                Name = "Name",
                Description = "Description"
            };
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryString["description"] = "description";

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

            var response = await _client.GetAsync($"/invoice?{queryString.ToString()}");

            var getContent = await response.Content.ReadAsStringAsync();
            var getViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<List<InvoiceResponse>>>(getContent);

            //assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(getViewModel.Data);
            Assert.NotEmpty(getViewModel.Data);
            Assert.Equal(createViewModel.Data.Id, getViewModel.Data[0].Id);
            Assert.Equal(createViewModel.Data.Name, getViewModel.Data[0].Name);
            Assert.Equal(createViewModel.Data.Description, getViewModel.Data[0].Description);
        }

        [Fact]
        public async Task Delete_FailureResponse_InvalidGuid()
        {
            //arrange
            string invalidGuid = "INVALID_GUID";

            //act
            var deleteResponse = await _client.DeleteAsync($"/invoice/{invalidGuid}");

            var deleteContent = await deleteResponse.Content.ReadAsStringAsync();
            var deleteViewModel = JsonConvert.DeserializeObject<FailureResponse>(deleteContent);

            //assert
            Assert.Equal(HttpStatusCode.BadRequest, deleteResponse.StatusCode);
            Assert.NotEmpty(deleteViewModel.Message);
        }

        [Fact]
        public async Task Delete_FailureResponse_NotFound()
        {
            //arrange
            string validGuid = Guid.NewGuid().ToString();

            //act
            var deleteResponse = await _client.DeleteAsync($"/invoice/{validGuid}");

            var deleteContent = await deleteResponse.Content.ReadAsStringAsync();
            var deleteViewModel = JsonConvert.DeserializeObject<FailureResponse>(deleteContent);

            //assert
            Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
            Assert.NotEmpty(deleteViewModel.Message);
        }

        [Fact]
        public async Task Delete_SuccessResponse()
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

            var deleteResponse = await _client.DeleteAsync($"/invoice/{createViewModel.Data.Id}");

            var deleteContent = await deleteResponse.Content.ReadAsStringAsync();
            var deleteViewModel = JsonConvert.DeserializeObject<SuccessfulResponse<bool>>(deleteContent);

            //assert
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
            Assert.True(deleteViewModel.Data);
        }
    }
}
