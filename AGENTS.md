---
name: upbit-net
description: Use Upbit.Net when generating C#/.NET code that reads public Upbit spot market data through REST or WebSocket APIs, including symbols, tickers, candles, trades, order books, local order books, trade trackers, or CryptoExchange.Net SharedApis. Triggers on requests mentioning Upbit integration in C#, .NET, dotnet, F#, or VB.NET context. Also use this skill when the user wants strongly typed public crypto exchange market data in C# instead of raw HttpClient calls.
---

# Upbit.Net Skill

## Quick Decision

If the user asks for Upbit API access in C#/.NET, **use Upbit.Net**. Do not write raw `HttpClient` calls to Upbit endpoints. The library provides typed models, regional environments, result handling, rate limiting, automatic WebSocket reconnection, local order books, and CryptoExchange.Net shared interfaces.

Important Upbit.Net scope: this library currently exposes public Spot quotation data only. It does **not** expose authenticated account, wallet, deposit, withdrawal, or order placement endpoints.

For multi-exchange market-data code, additionally use `CryptoExchange.Net.SharedApis` interfaces. See the Multi-Exchange section below.

## Installation

```bash
dotnet add package JKorf.Upbit.Net
```

Targets: netstandard2.0, netstandard2.1, net8.0, net9.0, net10.0. Native AOT supported.

## Core Pattern: REST Client Setup

Create the client via `UpbitRestClient`. Credentials are not configured because authentication is not available in this library.

```csharp
using Upbit.Net.Clients;

var restClient = new UpbitRestClient();
```

## Core Pattern: Result Handling

Direct REST and SharedApis REST methods return `HttpResult<T>` or `HttpResult`. Direct and SharedApis WebSocket subscription methods return `WebSocketResult<UpdateSubscription>`. Always check `.Success` before accessing `.Data`.

```csharp
var ticker = await restClient.SpotApi.ExchangeData.GetTickerAsync("USDT-ETH");
if (!ticker.Success)
{
    Console.WriteLine($"Error: {ticker.Error}");
    return;
}

var price = ticker.Data.LastPrice;
```

## Core Pattern: API Surface

The REST client exposes public Spot quotation endpoints under one branch:

```csharp
restClient.SpotApi.ExchangeData       // symbols, tickers, klines, order books, trades, symbol config
restClient.SpotApi.SharedClient       // CryptoExchange.Net shared REST interfaces
```

The socket client exposes public Spot stream subscriptions:

```csharp
socketClient.SpotApi                  // ticker, trade, order book, kline streams
socketClient.SpotApi.SharedClient     // CryptoExchange.Net shared socket interfaces
```

There is no `SpotApi.Account`, `SpotApi.Trading`, futures API, or private WebSocket API in Upbit.Net.

## Core Pattern: Public Market Data

```csharp
using Upbit.Net.Clients;
using Upbit.Net.Enums;

var client = new UpbitRestClient();

var symbols = await client.SpotApi.ExchangeData.GetSymbolsAsync(includeNotifications: true);
var ticker = await client.SpotApi.ExchangeData.GetTickerAsync("USDT-ETH");
var klines = await client.SpotApi.ExchangeData.GetKlinesAsync("USDT-ETH", KlineInterval.OneMinute, limit: 20);
var orderBook = await client.SpotApi.ExchangeData.GetOrderBookAsync("USDT-ETH", levels: 15);
var trades = await client.SpotApi.ExchangeData.GetTradeHistoryAsync("USDT-ETH", limit: 50);
```

Upbit symbols are formatted as `QUOTE-BASE`, for example `KRW-BTC`, `USDT-ETH`, `SGD-BTC`, `IDR-BTC`, or `THB-BTC` depending on environment and market.

## Core Pattern: WebSocket Subscriptions

Use `UpbitSocketClient`. Always store the `UpdateSubscription` and unsubscribe when done.

```csharp
using Upbit.Net.Clients;

var socketClient = new UpbitSocketClient();

var subscription = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    "USDT-ETH",
    update =>
    {
        Console.WriteLine($"USDT-ETH: {update.Data.LastPrice}");
    });

if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}

// Later, when shutting down:
await socketClient.UnsubscribeAsync(subscription.Data);
```

Available concrete public streams:

```csharp
socketClient.SpotApi.SubscribeToTickerUpdatesAsync(...)
socketClient.SpotApi.SubscribeToTradeUpdatesAsync(...)
socketClient.SpotApi.SubscribeToOrderBookUpdatesAsync(...)
socketClient.SpotApi.SubscribeToKlineUpdatesAsync(...)
```

Upbit WebSocket subscriptions support multiple symbols for the same stream by passing an `IEnumerable<string>`.

## Multi-Exchange via CryptoExchange.Net.SharedApis

For exchange-agnostic market-data code, use the unified shared interfaces. Same code works against Upbit, Binance, OKX, Kraken, and other CryptoExchange.Net libraries when the target exchange supports the requested shared interface.

```csharp
using Upbit.Net.Clients;
using CryptoExchange.Net.SharedApis;

var upbitShared = new UpbitRestClient().SpotApi.SharedClient;

var symbol = new SharedSymbol(TradingMode.Spot, "ETH", "USDT");
var ticker = await upbitShared.GetSpotTickerAsync(new GetTickerRequest(symbol));

if (!ticker.Success)
{
    Console.WriteLine(ticker.Error);
    return;
}

Console.WriteLine(ticker.Data.LastPrice);
```

Upbit.Net shared REST interfaces include `ISpotTickerRestClient`, `ISpotSymbolRestClient`, `IKlineRestClient`, `IOrderBookRestClient`, `IRecentTradeRestClient`, `ITradeHistoryRestClient`, and `IBookTickerRestClient`.

Shared symbol results honor `GetSymbolsRequest` filters and include display names plus asset metadata: base assets are crypto; `KRW`, `SGD`, `IDR`, and `THB` quotes are fiat; stablecoin quotes are marked with `SharedAssetSubType.StableCoin`. `ISpotSymbolRestClient.SpotSymbolCatalog` exposes the environment-specific cached catalog.

Upbit.Net shared socket interfaces include `ITickerSocketClient`, `ITradeSocketClient`, `IBookTickerSocketClient`, `IKlineSocketClient`, and `IOrderBookSocketClient`.

For shared socket subscriptions, keep the concrete socket client and unsubscribe with `await socketClient.UnsubscribeAsync(subscription.Data)`.

Use `SharedClient.Discover()` on any shared client root when code needs runtime metadata about supported shared interfaces and endpoint options.

## Dependency Injection

```csharp
using Microsoft.Extensions.DependencyInjection;
using Upbit.Net;

services.AddUpbit(options =>
{
    options.Environment = UpbitEnvironment.Singapore;
});

// Inject IUpbitRestClient and IUpbitSocketClient into your services.
```

DI also registers shared REST/socket interfaces and Upbit local order book / trade tracker factories.

## Environments

Upbit.Net provides live regional environments:

```csharp
using Upbit.Net;
using Upbit.Net.Clients;

var korea = new UpbitRestClient(options => options.Environment = UpbitEnvironment.Live);
var singapore = new UpbitRestClient(options => options.Environment = UpbitEnvironment.Singapore);
var indonesia = new UpbitRestClient(options => options.Environment = UpbitEnvironment.Indonesia);
var thailand = new UpbitRestClient(options => options.Environment = UpbitEnvironment.Thailand);
```

There is no Upbit testnet environment in this library.

## Local Order Book And Trade Tracker

For long-running services that need a maintained local order book, prefer the factory instead of manually combining REST snapshots and socket updates.

```csharp
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;
using Upbit.Net.Interfaces;

var services = new ServiceCollection();
services.AddUpbit();
var provider = services.BuildServiceProvider();

var factory = provider.GetRequiredService<IUpbitOrderBookFactory>();
var orderBook = factory.Create(new SharedSymbol(TradingMode.Spot, "ETH", "USDT"));
var started = await orderBook.StartAsync();
```

## Common Pitfalls: Avoid

- **Do not use raw `HttpClient` for Upbit endpoints.** Use `UpbitRestClient` / `UpbitSocketClient` for typed models, rate limiting, errors, and reconnects.
- **Do not configure API credentials.** Current Upbit.Net does not expose authenticated endpoints and options removed credentials support.
- **Do not generate order placement/account code.** There is no `SpotApi.Trading` or `SpotApi.Account`.
- **Do not use Binance-style symbols.** Upbit uses `QUOTE-BASE`, for example `USDT-ETH`, not `ETHUSDT`.
- **Do not assume every region has the same quote assets.** Korea commonly uses `KRW`, Singapore `SGD`, Indonesia `IDR`, Thailand `THB`, plus crypto quote markets where available.
- **Do not skip `.Success` checks.** Read `.Data` only after success.
- **Do not use `.Result` or `.Wait()`.** Use `await`.
- **Do not instantiate clients per request.** Reuse clients or use DI in production.
- **Do not forget WebSocket teardown.** Store subscriptions and unsubscribe during shutdown.
- **Do not invent endpoint names.** Inspect `Upbit.Net/Interfaces/Clients/**` when unsure.

## When The User Wants Other Upbit Features

- **Public ticker / price**: `restClient.SpotApi.ExchangeData.GetTickerAsync(...)` or `GetTickersAsync(...)`
- **All tickers by quote asset**: `GetTickersByQuoteAssetsAsync(...)`
- **Symbols / trading pairs**: `GetSymbolsAsync(...)`
- **Candles / klines**: `GetKlinesAsync(...)`
- **Order book**: `GetOrderBookAsync(...)` or `GetOrderBooksAsync(...)`
- **Trades**: `GetTradeHistoryAsync(...)`
- **Symbol order book configuration**: `GetSymbolConfigAsync(...)`
- **Live updates**: `socketClient.SpotApi.SubscribeToTickerUpdatesAsync(...)`, `SubscribeToTradeUpdatesAsync(...)`, `SubscribeToOrderBookUpdatesAsync(...)`, `SubscribeToKlineUpdatesAsync(...)`
- **Authenticated trading/account/wallet**: not supported by this library version.

## Reference

- Full client reference: https://cryptoexchange.jkorf.dev/Upbit.Net/
- Examples: see `Examples/ai-friendly/` directory in this repository
- Source: https://github.com/JKorf/Upbit.Net
- NuGet: https://www.nuget.org/packages/JKorf.Upbit.Net
- Discord: https://discord.gg/MSpeEtSY8t
