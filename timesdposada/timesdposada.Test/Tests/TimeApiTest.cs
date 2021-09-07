using employees.Test.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using timesdposada.Common.Models;
using timesdposada.Functions.Entities;
using timesdposada.Functions.Functions;
using timesdposada.Test.Helpers;
using Xunit;

namespace employees.Test.Tests
{
    public class TimeApiTest
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void CreateEntry_Should_Return_200()
        {
            // Arrenge
            MockCloudTableEntrys mockEntries = new MockCloudTableEntrys(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Time entryRequest = TestFactory.GetEntryRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(entryRequest);

            // Act
            IActionResult response = await TimeApi.CreateEntry(request, mockEntries, logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void UpdateTime_Should_Return_200()
        {
            // Arrenge
            MockCloudTableEntrys mockTodos = new MockCloudTableEntrys(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Time timeRequest = TestFactory.GetEntryRequest();
            Guid timeId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeId, timeRequest);

            // Act
            IActionResult response = await TimeApi.UpdateTime(request, mockTodos, timeId.ToString(), logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void GetAllTime_Should_Return_200()
        {

            // Arrange
            MockCloudTableEntrys mockTodos = new MockCloudTableEntrys(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            Time timeRequest = TestFactory.GetEntryRequest();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeRequest);

            // Act
            IActionResult response = await TimeApi.GetAllTimes(request, mockTodos, logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;

            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async void DeleteByIdTime_Should_Return_200()
        {

            // Arrange
            MockCloudTableEntrys mockTime = new MockCloudTableEntrys(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
            TimeEntity timeEntity = TestFactory.GetEntryEntity();
            Time timeRequest = TestFactory.GetEntryRequest();
            Guid timeId = Guid.NewGuid();
            DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeId, timeRequest);

            // Act
            IActionResult response = await TimeApi.DeleteTimeById(request, timeEntity, mockTime, timeId.ToString(), logger);

            // Assert
            OkObjectResult result = (OkObjectResult)response;
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        //[Fact]
        //public async void GetByIdTime_Should_Return_200()
        //{
        //    // Arrage
        //    MockCloudTableEntrys mockTime = new MockCloudTableEntrys(new Uri("http://127.0.0.1:10002/devstoreaccount1/reports"));
        //    TimeEntity timeEntity = TestFactory.GetEntryEntity();
        //    Time timeRequest = TestFactory.GetEntryRequest();
        //    Guid timeId = Guid.NewGuid();
        //    DefaultHttpRequest request = TestFactory.CreateHttpRequest(timeId, timeRequest);

        //    // Act
        //    IActionResult response = await TimeApi.GetEntryById(request, timeEntity, timeId.ToString(), logger);

        //    // Assert
        //    OkObjectResult result = (OkObjectResult)response;
        //    Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        //}
    }
}
