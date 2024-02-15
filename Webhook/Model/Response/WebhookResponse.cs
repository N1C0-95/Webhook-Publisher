namespace Webhook.Model.Response
{
    public class WebhookResponse
    {
        public int TopicID { get; set; }
        public string Name { get; set; }
        public string TopicName { get; set; }
        public string CallbackUrl { get; set; }
    }
}
