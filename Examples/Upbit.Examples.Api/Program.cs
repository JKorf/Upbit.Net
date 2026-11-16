using Upbit.Net.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add the Upbit services
builder.Services.AddUpbit();

// Public data requires no credentials. AddUpbit options can select a region or timeout.

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Map the endpoint and inject the rest client
app.MapGet("/{Symbol}", async ([FromServices] IUpbitRestClient client, string symbol) =>
{
    var result = await client.SpotApi.ExchangeData.GetTickerAsync(symbol);
    if (!result.Success)
        return Results.Problem(detail: result.Error?.ToString(), statusCode: StatusCodes.Status502BadGateway);
    return Results.Ok(result.Data.LastPrice);
})
.WithOpenApi();

app.Run();