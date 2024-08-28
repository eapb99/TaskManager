using System.Net.Http.Headers;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.Services
{

    public class TokenResponse
    {
        public string Token { get; set; }
    }
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:5000/api/");
        }

        // Method to authenticate and get a JWT token
        public async Task<string> LoginAsync(string email, string password)
        {
            var loginModel = new { Email = email, Password = password };
            var response = await _httpClient.PostAsJsonAsync("Auth/login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
                return result?.Token;
            }

            return null;
        }

        // Method to set the JWT token in the request headers
        public void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IEnumerable<PendingTask>> GetTasksAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<PendingTask>>("PendingTask");
            return response;
        }

        // Get a task by ID
        public async Task<PendingTask> GetTaskByIdAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<PendingTask>($"PendingTask/{id}");
            return response;
        }

        // Create a new task
        public async Task<bool> CreateTaskAsync(PendingTask task)
        {
            var response = await _httpClient.PostAsJsonAsync("PendingTask", task);
            return response.IsSuccessStatusCode;
        }

        // Update an existing task
        public async Task<bool> UpdateTaskAsync(int id, PendingTask task)
        {
            var response = await _httpClient.PutAsJsonAsync($"PendingTask/{id}", task);
            return response.IsSuccessStatusCode;
        }

        // Delete a task by ID
        public async Task<bool> DeleteTaskAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"PendingTask/{id}");
            return response.IsSuccessStatusCode;
        }


        // Define other methods to interact with different endpoints of your API as needed
    }
}
