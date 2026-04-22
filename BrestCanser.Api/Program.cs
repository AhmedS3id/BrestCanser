using BrestCanser.Api.Clients.MLModel;
using BrestCanser.Api.Hubs;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDependencies(builder.Configuration);

builder.Services
	.AddRefitClient<IMLModelClient>()
	.ConfigureHttpClient(c =>
	{
		c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("MLModel:BaseUrl")!);
	});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();
