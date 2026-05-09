# Upbit.Net AI API Quick Map

Use this file to route common user intents to the correct Upbit.Net client member. If a method name or parameter is not listed here, inspect `Upbit.Net/Interfaces/Clients/**` before generating code.

## Client Roots

| Intent | Use |
|---|---|
| REST calls | `new UpbitRestClient()` |
| WebSocket streams | `new UpbitSocketClient()` |
| API key authentication | Not supported by this library |
| Live South Korea environment | `UpbitEnvironment.Live` |
| Live Singapore environment | `UpbitEnvironment.Singapore` |
| Live Indonesia environment | `UpbitEnvironment.Indonesia` |
| Live Thailand environment | `UpbitEnvironment.Thailand` |
| Custom environment | `UpbitEnvironment.CreateCustom(name, restAddress, socketAddress)` |
| Dependency injection | `services.AddUpbit(options => { ... })` |

## Spot REST

| User intent | Upbit.Net member |
|---|---|
| Get supported symbols / markets | `client.SpotApi.ExchangeData.GetSymbolsAsync(includeNotifications: true)` |
| Get supported symbols without notifications | `client.SpotApi.ExchangeData.GetSymbolsAsync(includeNotifications: false)` |
| Get one ticker | `client.SpotApi.ExchangeData.GetTickerAsync("USDT-ETH")` |
| Get multiple tickers | `client.SpotApi.ExchangeData.GetTickersAsync(new[] { "USDT-ETH", "USDT-BTC" })` |
| Get all tickers for quote assets | `client.SpotApi.ExchangeData.GetTickersByQuoteAssetsAsync(new[] { "USDT", "BTC" })` |
| Get recent trades | `client.SpotApi.ExchangeData.GetTradeHistoryAsync("USDT-ETH", limit: 100)` |
| Get trade history before time | `client.SpotApi.ExchangeData.GetTradeHistoryAsync("USDT-ETH", endTime: endTime, limit: 100)` |
| Get trade history with cursor | `client.SpotApi.ExchangeData.GetTradeHistoryAsync("USDT-ETH", cursor: cursor)` |
| Get order book | `client.SpotApi.ExchangeData.GetOrderBookAsync("USDT-ETH", levels: 15)` |
| Get aggregated order book | `client.SpotApi.ExchangeData.GetOrderBookAsync("USDT-ETH", levels: 15, aggregation: 0.01m)` |
| Get multiple order books | `client.SpotApi.ExchangeData.GetOrderBooksAsync(new[] { "USDT-ETH", "USDT-BTC" }, levels: 15)` |
| Get klines/candles | `client.SpotApi.ExchangeData.GetKlinesAsync("USDT-ETH", KlineInterval.OneMinute, limit: 100)` |
| Get klines before time | `client.SpotApi.ExchangeData.GetKlinesAsync("USDT-ETH", KlineInterval.OneMinute, endTime: endTime, limit: 100)` |
| Get one second candles | `client.SpotApi.ExchangeData.GetKlinesAsync("USDT-ETH", KlineInterval.OneSecond)` |
| Get daily candles | `client.SpotApi.ExchangeData.GetKlinesAsync("USDT-ETH", KlineInterval.OneDay)` |
| Get symbol order book config | `client.SpotApi.ExchangeData.GetSymbolConfigAsync("USDT-ETH")` |
| Get config for multiple symbols | `client.SpotApi.ExchangeData.GetSymbolConfigAsync("USDT-ETH,USDT-BTC")` |

## Spot WebSocket

| User intent | Upbit.Net member |
|---|---|
| Subscribe ticker updates | `socketClient.SpotApi.SubscribeToTickerUpdatesAsync("USDT-ETH", handler)` |
| Subscribe many ticker updates | `socketClient.SpotApi.SubscribeToTickerUpdatesAsync(new[] { "USDT-ETH", "USDT-BTC" }, handler)` |
| Subscribe trade updates | `socketClient.SpotApi.SubscribeToTradeUpdatesAsync("USDT-ETH", handler)` |
| Subscribe many trade updates | `socketClient.SpotApi.SubscribeToTradeUpdatesAsync(new[] { "USDT-ETH", "USDT-BTC" }, handler)` |
| Subscribe order book updates | `socketClient.SpotApi.SubscribeToOrderBookUpdatesAsync("USDT-ETH", 15, handler)` |
| Subscribe aggregated order book updates | `socketClient.SpotApi.SubscribeToOrderBookUpdatesAsync("USDT-ETH", 15, handler, aggregation: 0.01m)` |
| Subscribe many order book updates | `socketClient.SpotApi.SubscribeToOrderBookUpdatesAsync(new[] { "USDT-ETH", "USDT-BTC" }, 15, handler)` |
| Subscribe kline/candle updates | `socketClient.SpotApi.SubscribeToKlineUpdatesAsync("USDT-ETH", KlineInterval.OneMinute, handler)` |
| Subscribe many kline/candle updates | `socketClient.SpotApi.SubscribeToKlineUpdatesAsync(new[] { "USDT-ETH", "USDT-BTC" }, KlineInterval.OneMinute, handler)` |
| Unsubscribe one stream | `await socketClient.UnsubscribeAsync(subscription.Data)` |
| Unsubscribe all streams | `await socketClient.UnsubscribeAllAsync()` |

## SharedApis REST

Use SharedApis for exchange-agnostic market-data code across Upbit, Binance, OKX, Kraken, and other CryptoExchange.Net libraries.

| User intent | Upbit.Net member or interface |
|---|---|
| Shared spot REST client | `new UpbitRestClient().SpotApi.SharedClient` |
| Shared spot ticker REST | `ISpotTickerRestClient.GetSpotTickerAsync(new GetTickerRequest(symbol))` |
| Shared spot tickers REST | `ISpotTickerRestClient.GetSpotTickersAsync(new GetTickersRequest())` |
| Shared symbols REST | `ISpotSymbolRestClient.GetSpotSymbolsAsync(new GetSymbolsRequest())` |
| Shared klines REST | `IKlineRestClient.GetKlinesAsync(new GetKlinesRequest(symbol, interval), pageRequest, ct)` |
| Shared order book REST | `IOrderBookRestClient.GetOrderBookAsync(new GetOrderBookRequest(symbol), ct)` |
| Shared recent trades REST | `IRecentTradeRestClient.GetRecentTradesAsync(new GetRecentTradesRequest(symbol), ct)` |
| Shared trade history REST | `ITradeHistoryRestClient.GetTradeHistoryAsync(new GetTradeHistoryRequest(symbol), pageRequest, ct)` |
| Shared book ticker REST | `IBookTickerRestClient.GetBookTickerAsync(new GetBookTickerRequest(symbol), ct)` |

## SharedApis WebSocket

| User intent | Upbit.Net member or interface |
|---|---|
| Shared spot socket client | `new UpbitSocketClient().SpotApi.SharedClient` |
| Shared ticker socket | `ITickerSocketClient.SubscribeToTickerUpdatesAsync(new SubscribeTickerRequest(symbol), handler)` |
| Shared trade socket | `ITradeSocketClient.SubscribeToTradeUpdatesAsync(new SubscribeTradeRequest(symbol), handler)` |
| Shared order book socket | `IOrderBookSocketClient.SubscribeToOrderBookUpdatesAsync(new SubscribeOrderBookRequest(symbol), handler)` |
| Shared book ticker socket | `IBookTickerSocketClient.SubscribeToBookTickerUpdatesAsync(new SubscribeBookTickerRequest(symbol), handler)` |
| Shared kline socket | `IKlineSocketClient.SubscribeToKlineUpdatesAsync(new SubscribeKlineRequest(symbol, interval), handler)` |
| Shared socket unsubscribe | Keep concrete `UpbitSocketClient` and call `socketClient.UnsubscribeAsync(subscription.Data)` |

## Kline Intervals

| User intent | Upbit.Net enum |
|---|---|
| 1 second candle | `KlineInterval.OneSecond` |
| 1 minute candle | `KlineInterval.OneMinute` |
| 3 minute candle | `KlineInterval.ThreeMinutes` |
| 5 minute candle | `KlineInterval.FiveMinutes` |
| 10 minute candle | `KlineInterval.TenMinutes` |
| 15 minute candle | `KlineInterval.FifteenMinutes` |
| 30 minute candle | `KlineInterval.ThirtyMinutes` |
| 1 hour candle | `KlineInterval.OneHour` |
| 4 hour candle | `KlineInterval.FourHours` |
| 1 day candle | `KlineInterval.OneDay` |
| 1 week candle | `KlineInterval.OneWeek` |
| 1 month candle | `KlineInterval.OneMonth` |
| 1 year candle | `KlineInterval.OneYear` |

## Environments And Symbols

| User intent | Use |
|---|---|
| Korea live markets | `UpbitEnvironment.Live` |
| Singapore live markets | `UpbitEnvironment.Singapore` |
| Indonesia live markets | `UpbitEnvironment.Indonesia` |
| Thailand live markets | `UpbitEnvironment.Thailand` |
| Symbol format | `QUOTE-BASE`, for example `USDT-ETH`, `KRW-BTC`, `SGD-BTC` |
| SharedApis symbol | `new SharedSymbol(TradingMode.Spot, "ETH", "USDT")` |
| List available environment names | `UpbitEnvironment.All` |

## Result Handling

| Situation | Pattern |
|---|---|
| REST success check | `if (!result.Success) { Console.WriteLine(result.Error); return; }` |
| Socket subscription success check | `if (!sub.Success) { Console.WriteLine(sub.Error); return; }` |
| Read REST data | Read `result.Data` only after `result.Success` |
| Retry decision | Retry only when `result.Error?.IsTransient == true` |
| Cancellation | Pass `ct: cancellationToken` |
| Socket teardown | `await socketClient.UnsubscribeAsync(sub.Data)` |

## Common Routing Pitfalls

| Do not use | Use instead |
|---|---|
| Raw `HttpClient` to Upbit endpoints | `UpbitRestClient` / `UpbitSocketClient` |
| `ApiCredentials`, `UpbitCredentials`, API key setup | No credentials; public APIs only |
| `SpotApi.Trading` | Not supported by this library |
| `SpotApi.Account` | Not supported by this library |
| Futures, margin, private streams | Not supported by this library |
| `ETHUSDT` | `USDT-ETH` |
| Testnet environment | Regional live environments only |
| `.Data` without `.Success` check | Check `.Success` first |
| `ITickerSocketClient.UnsubscribeAsync(...)` | Keep the concrete socket client and call `socketClient.UnsubscribeAsync(subscription.Data)` |
