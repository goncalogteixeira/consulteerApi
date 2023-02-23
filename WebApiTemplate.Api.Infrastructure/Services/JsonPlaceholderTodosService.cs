using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.DTOs.Settings;
using WebApiTemplate.Api.Application.DTOs.Todos;
using WebApiTemplate.Api.Application.Interfaces.Services;

namespace WebApiTemplate.Api.Infrastructure.Services
{
    public class JsonPlaceholderTodosService : IExternalTodosService
    {
        private readonly ILogger<JsonPlaceholderTodosService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ExternalApiSettings _externalApiSettings;

        public JsonPlaceholderTodosService(
            ILogger<JsonPlaceholderTodosService> logger
            , IHttpClientFactory httpClientFactory
            , IOptions<ExternalApiSettings> externalApiSettings
            )
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _externalApiSettings = externalApiSettings.Value;
        }

        public async Task<List<TodoDTO>> GetTodosAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient(_externalApiSettings.Name);

            using HttpResponseMessage response = await client.GetAsync(_externalApiSettings.GetUrl);

            List<TodoDTO> todos;

            if (response.IsSuccessStatusCode)
            {
                todos = await response.Content.ReadAsAsync<List<TodoDTO>>();
            }
            else
            {
                _logger.LogError(_externalApiSettings.FailureMessage);

                todos = null;
            }

            return todos;
        }

        public async Task<TodoDTO> CreateTodoAsync(TodoCreateDTO todo)
        {
            HttpClient client = _httpClientFactory.CreateClient(_externalApiSettings.Name);

            using StringContent todoJson = new(
                JsonSerializer.Serialize(todo)
                , Encoding.UTF8
                , "application/json"
                );

            using HttpResponseMessage response = await client.PostAsync(_externalApiSettings.PostUrl, todoJson);

            TodoDTO createdTodo;

            if (response.IsSuccessStatusCode)
            {
                createdTodo = await response.Content.ReadAsAsync<TodoDTO>();
            }
            else
            {
                _logger.LogError(_externalApiSettings.FailureMessage);

                createdTodo = null;
            }

            return createdTodo;
        }
    }
}
