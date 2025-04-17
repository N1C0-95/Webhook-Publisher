using System.Net.Http;
using System.Text.Json;
using System.Text;
using Webhook.Model.Request;
using Webhook.Repository;

namespace Webhook.Service
{
    public class WebhookService : IWebhookService
    {
        
        private readonly ILogger<WebhookService> _logger;
        private readonly IWebhookRepository _webookRepository;

        public WebhookService(ILogger<WebhookService> logger, IWebhookRepository webhookRepository)
        {
            _logger = logger;
            _webookRepository = webhookRepository;
        }


        public async Task ProcessOrderAsync(Order order)
        {
            _logger.LogInformation("Inizio elaborazione ordine");

            try
            {
                await SimulateProcessingAsync();

                var webhookUrl = await GetWebhookUrlAsync();
                if (string.IsNullOrEmpty(webhookUrl))
                {
                    _logger.LogWarning("Nessun webhook URL trovato.");
                    return;
                }

                var response = await SendWebhookNotificationAsync(webhookUrl, order);
                _logger.LogInformation($"Webhook chiamato con status: {(int)response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore nell'elaborazione ordine: {ex.Message}");
            }
        }

        private async Task SimulateProcessingAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(10)); // Simula elaborazione  
        }

        private async Task<string> GetWebhookUrlAsync()
        {
            var result = await _webookRepository.GetAllAsync();
            return result.Select(x => x.CallbackUrl).LastOrDefault() ?? string.Empty;
        }

        private async Task<HttpResponseMessage> SendWebhookNotificationAsync(string webhookUrl, Order order)
        {
            var uriBuilder = new UriBuilder(webhookUrl);
            using var client = new HttpClient { BaseAddress = uriBuilder.Uri };
            var path = $"{uriBuilder.Path}{uriBuilder.Query}";

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(new { name = order.Name, email = order.Mail }),
                Encoding.UTF8,
                "application/json"
            );

            return await client.PostAsync(path, jsonContent);
        }
    }
}

