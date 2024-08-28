using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManagerMVC.Services;
using TaskManagerMVC.Models;
using Moq.Protected;
using System.Threading;

namespace TaskManagerTests
{
    public class ApiServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        public ApiServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:5000/api/")
            };
            _apiService = new ApiService(_httpClient);
        }


        [Fact]
        public async Task GetTasksAsync_ReturnsTasks()
        {

            var tokenValido = "sample-token";
            _apiService.SetAuthorizationHeader(tokenValido);

            var expectedTasks = new List<PendingTask>
            {
                new PendingTask { ID = 1, Title = "Task 1", Description = "Test Task 1" },
                new PendingTask { ID = 2, Title = "Task 2", Description = "Test Task 2" }
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedTasks)
            };


            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri == new Uri("https://localhost:5000/api/PendingTask") &&
                        req.Headers.Authorization != null &&
                        req.Headers.Authorization.Scheme == "Bearer" &&
                        req.Headers.Authorization.Parameter == tokenValido),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var tasks = await _apiService.GetTasksAsync();

            Assert.NotNull(tasks);
            Assert.Equal(expectedTasks.Count, tasks.Count());
            Assert.Equal(expectedTasks[0].Title, tasks.First().Title);
        }


        [Fact]
        public async Task GetTaskByIdAsync_ReturnsTask_WhenIdIsValid()
        {
            var tokenValido = "sample-token";
            _apiService.SetAuthorizationHeader(tokenValido);
            int taskId = 1;


            var expectedTask = new PendingTask { ID = taskId, Title = "Task 1", Description = "Test Task 1" };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(expectedTask)
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri == new Uri($"https://localhost:5000/api/PendingTask/{taskId}") &&
                        req.Headers.Authorization != null &&
                        req.Headers.Authorization.Scheme == "Bearer" &&
                        req.Headers.Authorization.Parameter == "sample-token"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);


            var task = await _apiService.GetTaskByIdAsync(taskId);

            Assert.NotNull(task);
            Assert.Equal(expectedTask.ID, task.ID);
            Assert.Equal(expectedTask.Title, task.Title);
            Assert.Equal(expectedTask.Description, task.Description);
        }

    }
}
