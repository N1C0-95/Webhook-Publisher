using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webhook.Auth;
using Webhook.Model.Request;
using Webhook.Repository;
using Webhook.Service;

namespace Webhook.Endpoints
{
    public interface IOrderWebhook
    {

        Task<IResult> SubscribeWebhook(WebhookRequest webhook, IWebhookRepository webookRepository);

        Task<IResult> GetWebhooks(IWebhookRepository webookRepository, string? searchTerm);
        Task<IResult> InvokeOrderWebhookAsync(Order order, IWebhookRepository webhookRepository, IWebhookService webhookService);
        Task<IResult> Echo([FromHeader(Name = "x-functions-key")] string apiKey);

        Task<IResult> DeleteWebhook(int id, IWebhookRepository webookRepository);
        
    }
}
