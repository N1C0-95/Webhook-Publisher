using Webhook.Model.Request;

namespace Webhook.Service
{
    public interface IWebhookService
    {
        Task ProcessOrderAsync(Order order);
    }
}
