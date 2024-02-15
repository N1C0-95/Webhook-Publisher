namespace Webhook.Model.Request
{
    public class WebhookRequest
    {
        public string Name { get; set; }
        public string TopicName { get; set; }
        public string CallbackUrl { get; set; }
    }
}
