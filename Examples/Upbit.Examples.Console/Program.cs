
using Upbit.Net.Clients;

// REST
var restClient = new UpbitRestClient();
var ticker = await restClient.SpotApi.ExchangeData.GetTickerAsync("USDT-ETH");
Console.WriteLine($"Rest client ticker price for USDT-ETH: {ticker.Data.LastPrice}");

Console.WriteLine();
Console.WriteLine("Press enter to start websocket subscription");
Console.ReadLine();

// Websocket
var socketClient = new UpbitSocketClient();
var subscription = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync("USDT-ETH", update =>
{
    Console.WriteLine($"Websocket client ticker price for USDT-ETH: {update.Data.LastPrice}");
});

Console.ReadLine();
