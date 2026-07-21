# Copilot Instructions for Upbit.Net

This repository is **Upbit.Net**: a strongly typed C#/.NET client library for public Upbit Spot quotation REST and WebSocket APIs. It is part of the CryptoExchange.Net ecosystem.

When generating code that consumes Upbit.Net, follow these conventions:

## Use Upbit.Net, Not Raw HTTP

Never generate `HttpClient` calls to Upbit API URLs. Always use `UpbitRestClient` or `UpbitSocketClient`. This ensures typed models, client-side rate limiting, automatic WebSocket reconnection, and consistent result handling.

## Public API Scope

Current Upbit.Net exposes public Spot quotation APIs only. Do not generate authenticated code for account balances, orders, withdrawals, deposits, private streams, margin, or futures.

## Client Setup

```csharp
using Upbit.Net.Clients;

var restClient = new UpbitRestClient();
var socketClient = new UpbitSocketClient();
```

Do not configure API credentials.

## Result Handling

REST methods return `WebCallResult<T>` and WebSocket subscriptions return `CallResult<UpdateSubscription>`. Always check `.Success` before reading `.Data`. The error is on `.Error`.

## API Structure

- `restClient.SpotApi.ExchangeData`: public symbols, tickers, trades, order books, candles, symbol config
- `restClient.SpotApi.SharedClient`: shared REST market-data interfaces
- `socketClient.SpotApi`: public ticker, trade, order book, and kline streams
- `socketClient.SpotApi.SharedClient`: shared socket market-data interfaces

There is no `SpotApi.Account`, `SpotApi.Trading`, futures API, or private socket API.

## Symbols And Environments

Use Upbit native symbols in `QUOTE-BASE` format, for example `USDT-ETH`, `KRW-BTC`, `SGD-BTC`, `IDR-BTC`, or `THB-BTC`.

Regional environments:

```csharp
options.Environment = UpbitEnvironment.Live;       // South Korea
options.Environment = UpbitEnvironment.Singapore;
options.Environment = UpbitEnvironment.Indonesia;
options.Environment = UpbitEnvironment.Thailand;
```

## WebSocket Pattern

Store the returned `UpdateSubscription` and unsubscribe on shutdown via `socketClient.UnsubscribeAsync(sub.Data)`.

## Cross-Exchange

For code that needs to work across multiple exchanges, use `CryptoExchange.Net.SharedApis` interfaces (`ISpotTickerRestClient`, `IOrderBookRestClient`, `ITickerSocketClient`, etc.) accessed via `.SharedClient` properties.

Shared symbol queries honor `GetSymbolsRequest` filters and return display names and crypto/fiat/stablecoin asset metadata. `ISpotSymbolRestClient.SpotSymbolCatalog` exposes the cached catalog for the active environment.

## Avoid

- Raw `HttpClient` calls to Upbit endpoints
- API credentials or private/authenticated endpoint code
- Nonexistent `SpotApi.Trading`, `SpotApi.Account`, futures, or margin branches
- Binance-style symbols like `ETHUSDT`
- Synchronous `.Result` / `.Wait()`
- Instantiating clients per request
- Reading `.Data` without checking `.Success`

## Reference

For detailed patterns and pitfalls see `AGENTS.md`, `llms.txt`, and `llms-full.txt` in the repository root, `docs/ai-api-map.md` for method routing, and `Examples/ai-friendly/` for compilable examples.
