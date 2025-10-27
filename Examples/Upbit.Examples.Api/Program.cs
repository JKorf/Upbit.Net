using Upbit.Net.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the Upbit services
builder.Services.AddUpbit();

// OR to provide API credentials for accessing private endpoints, or setting other options:
/*
builder.Services.AddUpbit(options =>
{
    options.ApiCredentials = new ApiCredentials("<APIKEY>", "<APISECRET>");
    options.Rest.RequestTimeout = TimeSpan.FromSeconds(5);
});
*/

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Map the endpoint and inject the rest client
app.MapGet("/{Symbol}", async ([FromServices] IUpbitRestClient client, string symbol) =>
{
    var result = await client.SpotApi.ExchangeData.GetTickerAsync(symbol);
    return result.Data.LastPrice;
})
.WithOpenApi();

app.Run();