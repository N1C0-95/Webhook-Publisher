using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Webhook.Auth
{
    public class ApiKeySchemeConstant
    {
        public const string SchemeName = "ApiKeyScheme";
    }
}
