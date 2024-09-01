using Microsoft.AspNetCore.Authentication;

namespace Webhook.Auth
{
    public class ApiKeyAuthSchemeOptions : AuthenticationSchemeOptions
    {
        public string ApiKey { get; set; } = "=UST]15'uNsj78lH9`F^2uibNyfRRd";
    }
}
