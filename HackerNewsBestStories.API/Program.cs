using HackerNewsBestStories.API.Interfaces;
using HackerNewsBestStories.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register HttpClient and HackerNewsService
builder.Services.AddScoped<IHackerNewsService, HackerNewsService>();
builder.Services.AddHttpClient<IHackerNewsHttpClient, HackerNewsHttpClient>();

// Register Memory Cache
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
