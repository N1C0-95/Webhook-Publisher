using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webhook.Auth;
using Webhook.Model.Request;
using Webhook.Repository;
using Webhook.Service;

namespace Webhook.Endpoints
{
    public class OrderWebhook : IOrderWebhook
    {

        [Authorize(AuthenticationSchemes = ApiKeySchemeConstant.SchemeName)]
        public  async Task<IResult> SubscribeWebhook(WebhookRequest webhook, IWebhookRepository webookRepository)
        {

            bool result = await webookRepository.CreateAsync(webhook);
            if (!result)
            {
                return Results.BadRequest(new
                {
                    erroreMessage = "Error during creation"
                });
            }
            return Results.Created($"callback:{webhook.CallbackUrl}", webhook);
        }
        public  async Task<IResult> GetWebhooks(IWebhookRepository webookRepository, string? searchTerm)
        {

            var result = await webookRepository.GetAllAsync();
            return Results.Ok(result);
        }
        public  async Task<IResult> InvokeOrderWebhookAsync(Order order, IWebhookRepository webhookRepository, IWebhookService webhookService)
        {
            if (order == null)
            {
                return Results.BadRequest("Order not valid");
            }

            // Start queue and track the task result
            _ = Task.Run(() => webhookService.ProcessOrderAsync(order));

            return Results.Ok("Order accepted successfully");
        }

        public  async Task<IResult> Echo([FromHeader(Name = "x-functions-key")] string apiKey)
        {

            return Results.Ok($"API Key Value : {apiKey} ");
        }

        public  async Task<IResult> DeleteWebhook(int id, IWebhookRepository webookRepository)
        {
            var deleted = await webookRepository.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        }

        
    }
}
