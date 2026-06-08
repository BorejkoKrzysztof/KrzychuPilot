using KrzychuPilot.Application.Common.Interfaces;
using KrzychuPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace KrzychuPilot.Infrastructure.BackgroundServices
{
    public class PromptProcessingWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PromptProcessingWorker> _logger;
        private readonly string _ollamaBaseUrl;

        public PromptProcessingWorker(
            IServiceProvider serviceProvider,
            IHttpClientFactory httpClientFactory,
            ILogger<PromptProcessingWorker> logger,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _ollamaBaseUrl = configuration.GetValue<string>("OLLAMA_URL") ?? "http://ollama:11434/";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                    var notifier = scope.ServiceProvider.GetRequiredService<IPromptNotificationService>();

                    var task = await context.PromptTasks
                                        .Where(t => t.Status == PromptStatus.Awaiting)
                                        .OrderBy(t => t.CreatedAt)
                                        .FirstOrDefaultAsync(stoppingToken);

                    if (task != null)
                    {
                        task.Status = PromptStatus.Awaiting;
                        await context.SaveChangesAsync(stoppingToken);
                        await notifier.NotifyStatusChanged(task.Id, "Processing");

                        try
                        {
                            string aiResponse = await CallOllamaAsync(task.Content, stoppingToken);

                            task.Result = aiResponse;
                            task.Status = PromptStatus.Finished;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"LLM Error for task {task.Id}: {ex.Message}");
                            task.Status = PromptStatus.Failed;
                            task.Result = "Error occured while generating response by AI";
                        }

                        task.ProcessedAt = DateTime.UtcNow;
                        await context.SaveChangesAsync(stoppingToken);
                        await notifier.NotifyStatusChanged(task.Id, task.Status.ToString(), task.Result);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error while background thread management {ex.Message}");
                }

                await Task.Delay(2000, stoppingToken);
            }
        }

        private async Task<string> CallOllamaAsync(string prompt, CancellationToken stoppingToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_ollamaBaseUrl);
            var requestBody = new { model = "phi3", prompt = prompt, stream = false };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/generate", content, stoppingToken);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync(stoppingToken);
            using var doc = JsonDocument.Parse(responseString);

            return doc.RootElement.GetProperty("response").GetString() ?? "No data generated";
        }
    }
}
