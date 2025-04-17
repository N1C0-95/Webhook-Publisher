using Webhook.Model.Request;
using Webhook.Repository;

namespace Webhook.Endpoints;

public static class OrderEndpoints
{        
    public static void AddOrderEndpoint(this IServiceCollection services)
    {
        services.AddScoped<IWebhookRepository, WebhookRepository>();
        
    }
    public static void UseOrderEndponts(this IEndpointRouteBuilder app, IOrderWebhook orderWebhook )
    {
        app.MapPost("webhook", orderWebhook.SubscribeWebhook).WithName("CreateWebhook").Accepts<WebhookRequest>("application/json").Produces<WebhookRequest>(201).Produces(401);
        app.MapGet("webhook", orderWebhook.GetWebhooks).WithName("GetWebhooks").Produces<WebhookRequest>(200);
        app.MapDelete("webhook/{id}", orderWebhook.DeleteWebhook).WithName("DeleteWebhok").Produces(204).Produces(404);
        app.MapPost("order", orderWebhook.InvokeOrderWebhookAsync).WithName("AddOrder").Produces<Order>(200);
        app.MapPost("echo", orderWebhook.Echo).WithName("Echo").Produces(200);
    }
}
