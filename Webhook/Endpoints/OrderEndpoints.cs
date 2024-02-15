using System.Text;
using System.Text.Json;
using Webhook.Model.Request;
using Webhook.Repository;

namespace Webhook.Endpoints
{
    public static class OrderEndpoints
    {
        public static void AddOrderEndpoint(this IServiceCollection services)
        {
            services.AddScoped<IWebhookRepository, WebhookRepository>();
        }
        public static void UseOrderEndponts(this IEndpointRouteBuilder app)
        {
            app.MapPost("webhook", SubscribeWebhook).WithName("CreateWebhook").Accepts<WebhookRequest>("application/json").Produces<WebhookRequest>(201);
            app.MapGet("webhook", GetWebhooks).WithName("GetWebhooks").Produces<WebhookRequest>(200);
            app.MapDelete("webhook/{id}", DeleteWebhook).WithName("DeleteWebhok").Produces(204).Produces(404);
            app.MapPost("order", InvokeOrderWebhook).WithName("AddOrder").Produces<Order>(200);
            
        }
        //Methods
        private static async Task<IResult> SubscribeWebhook(WebhookRequest webhook, IWebhookRepository webookRepository)
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
        private static async Task<IResult> GetWebhooks(IWebhookRepository webookRepository, string? searchTerm)
        {

            var result = await webookRepository.GetAllAsync();
            return Results.Ok(result);
        }
        private static async void InvokeOrderWebhook(Order order,IWebhookRepository webookRepository)
        {
            var result = await webookRepository.GetAllAsync();
            var webhookurl = result.Select(x => x.CallbackUrl).LastOrDefault() ?? "";

            var bldUri = new UriBuilder(webhookurl);

            if (bldUri != null)
            {

                HttpClient client = new() { BaseAddress = bldUri.Uri };
                string path = $"{bldUri.Path}{bldUri.Query}";

                using StringContent jsonContent = new(
                        JsonSerializer.Serialize(new
                        {
                            name = order.Name,
                            email = order.Mail
                        }),
                           Encoding.UTF8,
                           "application/json");

                HttpResponseMessage response = await client.PostAsync(path, jsonContent);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"{jsonResponse}\n");
            }




        }

        private static async Task<IResult> DeleteWebhook(int id, IWebhookRepository webookRepository)
        {
            var deleted = await webookRepository.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        }
    }
}
