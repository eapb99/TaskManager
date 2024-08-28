using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System.Net.Http.Json;
using System.Net;
using TaskManagerMVC.Controllers;
using TaskManagerMVC.Models;
using TaskManagerMVC.Services;
namespace TaskManagerTests
{

    public class TasksControllerTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly ApiService _apiService;
        private readonly TaskController _controller;

        public TasksControllerTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:5000/api/")
            };

            _apiService = new ApiService(httpClient);

            _controller = new TaskController(_apiService);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfTasks()
        {
            // Arrange
            var tasks = new List<PendingTask>
            {
                new PendingTask { ID = 1, Title = "Task 1", Description = "Test Task 1" },
                new PendingTask { ID = 2, Title = "Task 2", Description = "Test Task 2" }
            };
                _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri == new Uri("https://localhost:5000/api/PendingTask")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(tasks)
                });

                var result = await _controller.Index() as ViewResult;

                // Assert
                Assert.NotNull(result);
                var model = Assert.IsAssignableFrom<IEnumerable<PendingTask>>(result.Model);
                Assert.Equal(2, model.Count());
        }
    }
}