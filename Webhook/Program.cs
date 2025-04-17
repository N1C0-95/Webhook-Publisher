
using System.Runtime.CompilerServices;
using Webhook.Auth;
using Webhook.Data;
using Webhook.Endpoints;
using Webhook.Service;

var builder = WebApplication.CreateBuilder(args);

//service registration start here
builder.Services.AddAuthentication(ApiKeySchemeConstant.SchemeName)
    .AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthHandler>(ApiKeySchemeConstant.SchemeName, _ => { });
builder.Services.AddAuthorization();

builder.Services.AddSingleton<IDbConnectionFactory>(new SqliteConnectionFactory(builder.Configuration.GetValue<string>("Database:ConnectionString")));
builder.Services.AddSingleton<DatabaseInitializer>();

builder.Services.AddTransient<IWebhookService, WebhookService>();
builder.Services.AddSingleton<IOrderWebhook, OrderWebhook>();


builder.Services.AddOrderEndpoint();

var app = builder.Build();

// middleware registration starts here
app.UseAuthorization();

//use endpoints
app.UseOrderEndponts(app.Services.GetRequiredService<IOrderWebhook>());

//init database
var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();


app.Run();
