
using Webhook.Data;
using Webhook.Endpoints;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IDbConnectionFactory>(new SqliteConnectionFactory(builder.Configuration.GetValue<string>("Database:ConnectionString")));
builder.Services.AddSingleton<DatabaseInitializer>();

builder.Services.AddOrderEndpoint();

var app = builder.Build();

//use endpoints
app.UseOrderEndponts();

//init database
var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitializeAsync();


app.Run();
