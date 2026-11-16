# Usage

Install `JKorf.Upbit.Net`. Each C# block is an independent program with its imports. All requests below are public market-data reads; the library exposes no credential configuration, account reads, or trading operations. For routing and supported capabilities, see [API surfaces](api-surfaces.md). See [operational considerations](safety.md) for region, aggregation, freshness, and retry decisions.

## Public setup and results

```csharp
using Upbit.Net.Clients;

using var client = new UpbitRestClient();
var result = await client.SpotApi.ExchangeData.GetTickerAsync("USDT-ETH");
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine($"{result.Data.Symbol}: {result.Data.LastPrice}");
```

## Region and symbol validation

Select the region on both clients when using REST and sockets together. Discover symbols rather than assuming a market is listed there.

```csharp
using Upbit.Net;
using Upbit.Net.Clients;

using var client = new UpbitRestClient(options =>
    options.Environment = UpbitEnvironment.Singapore);
var result = await client.SpotApi.ExchangeData.GetSymbolsAsync(includeNotifications: true);
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.Any(market => market.Symbol == "SGD-BTC"));
```

## Candles, order book, and symbol configuration

```csharp
using Upbit.Net.Clients;
using Upbit.Net.Enums;

using var client = new UpbitRestClient();
const string symbol = "USDT-ETH";
var candles = await client.SpotApi.ExchangeData.GetKlinesAsync(
    symbol, KlineInterval.OneMinute, limit: 20);
if (!candles.Success)
    Console.WriteLine(candles.Error);
else
    Console.WriteLine($"Candles: {candles.Data.Length}");

var book = await client.SpotApi.ExchangeData.GetOrderBookAsync(symbol, levels: 15);
if (!book.Success)
    Console.WriteLine(book.Error);
else
{
    var best = book.Data.Entries.FirstOrDefault();
    if (best is not null)
        Console.WriteLine($"Bid: {best.BidPrice}; ask: {best.AskPrice}");
}

var config = await client.SpotApi.ExchangeData.GetSymbolConfigAsync(symbol);
if (!config.Success)
    Console.WriteLine(config.Error);
else
    foreach (var item in config.Data)
        Console.WriteLine($"{item.Symbol}: tick quantity {item.TickQuantity}");
```

## Subscription lifecycle

```csharp
using Upbit.Net.Clients;

using var socket = new UpbitSocketClient();
var subscription = await socket.SpotApi.SubscribeToTickerUpdatesAsync(
    "USDT-ETH", update => Console.WriteLine(update.Data.LastPrice));
if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}
try
{
    await Task.Delay(TimeSpan.FromSeconds(30));
}
finally
{
    await socket.UnsubscribeAsync(subscription.Data);
}
```

## Shared API V2

A known REST surface can be assigned to a narrow capability. Keep its native client alive for the call.

```csharp
using CryptoExchange.Net.SharedApis;
using Upbit.Net.Clients;

using var client = new UpbitRestClient();
IGetTickerRest ticker = client.SpotApi.SharedApi;
var symbol = new SharedSymbol(TradingMode.Spot, "ETH", "USDT");
var result = await ticker.GetTickerAsync(new GetTickerRequest(symbol));
if (!result.Success)
    Console.WriteLine($"{ticker.Exchange}: {result.Error}");
else
    Console.WriteLine(result.Data.LastPrice);
```

## DI and dynamic discovery

`AddUpbit` registers native clients, factories, the exchange aggregate, and supported capability interfaces. With multiple exchanges in a container, select the exchange aggregate rather than relying on the last registration of a common capability. The container owns the clients it creates.

```csharp
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;
using Upbit.Net;
using Upbit.Net.Interfaces.Clients;

var services = new ServiceCollection();
services.AddUpbit(options => options.Environment = UpbitEnvironment.Live);
using var provider = services.BuildServiceProvider();
var shared = provider.GetRequiredService<IUpbitSharedApiClient>();
IGetTickerRest knownSurface = shared.SpotRest;
var match = shared.GetCapability(
    SharedCapabilities.Tickers.GetTicker.Rest, TradingMode.Spot);
if (match is null)
{
    Console.WriteLine("No matching capability");
    return;
}
Console.WriteLine(match.Options);
var result = await match.Capability.GetTickerAsync(
    new GetTickerRequest(new SharedSymbol(TradingMode.Spot, "ETH", "USDT")));
if (!result.Success)
    Console.WriteLine(result.Error);
else
    Console.WriteLine(result.Data.LastPrice);
```

Checkout-only complete examples: [standalone programs](../../../../Examples/ai-friendly/README.md), [web API](../../../../Examples/Upbit.Examples.Api/Upbit.Examples.Api.csproj), [local order book](../../../../Examples/Upbit.Examples.OrderBook/Upbit.Examples.OrderBook.csproj), and [trade tracker](../../../../Examples/Upbit.Examples.Tracker/Upbit.Examples.Tracker.csproj). Compile them without running to validate API usage without exchange calls.
