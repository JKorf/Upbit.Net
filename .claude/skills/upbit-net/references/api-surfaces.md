# API surfaces

## Clients, regions, and scope

| Intent | Entry point |
| --- | --- |
| Native REST | `UpbitRestClient.SpotApi.ExchangeData` |
| Native public streams | `UpbitSocketClient.SpotApi` |
| Portable REST | `restClient.SpotApi.SharedApi` (`IUpbitRestClientSpotSharedApi`) |
| Portable streams | `socketClient.SpotApi.SharedApi` (`IUpbitSocketClientSpotSharedApi`) |
| DI | `services.AddUpbit(...)` |
| South Korea | `UpbitEnvironment.Live` |
| Singapore / Indonesia / Thailand | `UpbitEnvironment.Singapore` / `.Indonesia` / `.Thailand` |
| Custom endpoints | `UpbitEnvironment.CreateCustom(name, restAddress, socketAddress)` |

`IUpbitRestClient` and `IUpbitSocketClient` expose the native clients. The library has no authentication/credential type, account/trading branches, margin/futures surface, or private streams. REST and socket clients must use the intended regional environment. Environment names are `live`, `live-singapore`, `live-indonesia`, and `live-thailand`; `UpbitEnvironment.All` lists them.

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
| Shared spot REST client | `restClient.SpotApi.SharedApi` |
| Shared spot ticker REST | `IGetTickerRest.GetTickerAsync(new GetTickerRequest(symbol))` |
| Shared spot tickers REST | `IGetAllTickersRest.GetAllTickersAsync(new GetTickersRequest())` |
| Shared symbols REST | `IGetSpotSymbolsRest.GetSpotSymbolsAsync(new GetSymbolsRequest())` |
| Filter shared symbols | Set filters on `GetSymbolsRequest`; the returned list honors them |
| Cached shared symbol catalog | `IGetSpotSymbolsRest.SpotSymbolCatalog` (environment-specific) |
| Shared symbol metadata | `SharedSpotSymbol.DisplayName`, `BaseAssetType`, `QuoteAssetType`, `QuoteAssetSubType` (stablecoins use `SharedAssetSubType.StableCoin`) |
| Shared klines REST | `IGetKlinesRest.GetKlinesAsync(new GetKlinesRequest(symbol, interval), pageRequest, ct)` |
| Shared order book REST | `IGetOrderBookRest.GetOrderBookAsync(new GetOrderBookRequest(symbol), ct)` |
| Shared recent trades REST | `IGetRecentTradesRest.GetRecentTradesAsync(new GetRecentTradesRequest(symbol), ct)` |
| Shared trade history REST | `IGetTradeHistoryRest.GetTradeHistoryAsync(new GetTradeHistoryRequest(symbol), pageRequest, ct)` |
| Shared book ticker REST | `IGetBookTickerRest.GetBookTickerAsync(new GetBookTickerRequest(symbol), ct)` |

## SharedApis WebSocket

| User intent | Upbit.Net member or interface |
|---|---|
| Shared spot socket client | `socketClient.SpotApi.SharedApi` |
| Shared ticker socket | `ISubscribeTickerSocket.SubscribeToTickerUpdatesAsync(new SubscribeTickerRequest(symbol), handler)` |
| Shared trade socket | `ISubscribeTradesSocket.SubscribeToTradeUpdatesAsync(new SubscribeTradeRequest(symbol), handler)` |
| Shared order book socket | `ISubscribeOrderBookSocket.SubscribeToOrderBookUpdatesAsync(new SubscribeOrderBookRequest(symbol), handler)` |
| Shared book ticker socket | `ISubscribeBookTickerSocket.SubscribeToBookTickerUpdatesAsync(new SubscribeBookTickerRequest(symbol), handler)` |
| Shared kline socket | `ISubscribeKlinesSocket.SubscribeToKlineUpdatesAsync(new SubscribeKlineRequest(symbol, interval), handler)` |
| Shared socket unsubscribe | Keep concrete `UpbitSocketClient` and call `socketClient.UnsubscribeAsync(subscription.Data)` |
| Resolve a runtime-selected Shared API capability | `IUpbitSharedApiClient.GetCapability(...)` |

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

Native symbols use `QUOTE-BASE`: `USDT-ETH`, `KRW-BTC`, `SGD-BTC`, `IDR-BTC`, or `THB-BTC`. Discover listings in the selected region; fiat quote examples do not guarantee a particular market exists.

Shared socket order-book options accept levels `1`, `5`, `15`, and `30`. Native order-book aggregation is available on the native REST/socket methods; inspect regional symbol configuration before choosing it. The Shared API does not forward a native aggregation value. REST depth options and socket depth options are distinct; inspect the appropriate capability's options.

## Shared API discovery and selection

`IUpbitSharedApiClient` implements `ISharedApiClientBase`, exposing `SpotRest` and `SpotSocket` as the typed V2 aggregates above. For a known operation and transport, assign the surface directly to the corresponding narrow interface. For dynamic code, use `GetCapability(SharedCapabilities.Tickers.GetTicker.Rest, TradingMode.Spot)` or `GetCapabilities(...)`; handle a nullable `SharedCapabilityResolution<T>`. `Discover()` returns summary metadata. Descriptors identify contracts, not proof of exchange support.

The REST V2 aggregate implements `IGetTickerRest`, `IGetAllTickersRest`, `IGetSpotSymbolsRest`, `IGetKlinesRest`, `IGetOrderBookRest`, `IGetRecentTradesRest`, `IGetTradeHistoryRest`, and `IGetBookTickerRest`. The socket aggregate implements `ISubscribeTickerSocket`, `ISubscribeTradesSocket`, `ISubscribeBookTickerSocket`, `ISubscribeKlinesSocket`, and `ISubscribeOrderBookSocket`. No account or order-execution capability is implemented.

A resolution contains `Capability` and `Options`. Inspect `SupportedTradingModes`, request/exchange parameter rules, limits, and notes. `PreferredTransport` influences generic selection; `.Rest` requires REST. `GetCapabilities` returns all matching surfaces. With multiple exchange registrations, select the exchange aggregate or an explicitly chosen capability implementation instead of relying on single-service DI resolution order.

## Symbols, catalogs, and quantities

Native symbols use `QUOTE-BASE`: `KRW-BTC`, `USDT-ETH`, `SGD-BTC`, `IDR-BTC`, or `THB-BTC` are format examples, not a guarantee of listing. Query `GetSymbolsAsync` in the chosen environment. Shared requests normally use `new SharedSymbol(TradingMode.Spot, "ETH", "USDT")`, which the adapter formats.

Shared symbol queries honor `GetSymbolsRequest` filters and populate `IGetSpotSymbolsRest.SpotSymbolCatalog` for the current API/environment. They expose `DisplayName`, base/quote asset classification, and stablecoin subtypes. Fiat quotes include KRW, SGD, IDR, and THB. Treat missing classification as unknown and cached listings as a snapshot.

Shared ticker `Volumes` and book-ticker `BestBidQuantities`/`BestAskQuantities` are `SharedOrderQuantity` values, with nullable base/quote/contracts quantities. Do not treat an absent unit as zero or use obsolete scalar volume fields. Upbit's shared order-book adapter reports `SharedQuantityType.BaseAsset`. Order-request `SharedQuantity` is a different type; Upbit.Net has no order-submission capability.

## Order books, trackers, and constraints

`IUpbitOrderBookFactory` exposes `Spot`, `Create(SharedSymbol, options)`, and `CreateSpot(nativeSymbol, options)`. Start the created book, check its result, then stop/dispose it when its workload ends. `IUpbitTrackerFactory` implements public trade and kline tracking through `ITrackerFactory`.

Shared socket order-book options enumerate supported levels (1, 5, 15, 30). Native order-book aggregation and symbol configuration use exchange-specific values: consult symbol configuration for the selected market rather than assuming any decimal aggregation is accepted. Use operation options for shared interval/limit support; a native enum member does not prove every region supports it.

Checkout-only sources: [public interfaces](../../../../Upbit.Net/Interfaces/Clients), [REST adapters](../../../../Upbit.Net/Clients/SpotApi/Rest/SharedApi), [socket adapters](../../../../Upbit.Net/Clients/SpotApi/Socket/SharedApi), and [environment definitions](../../../../Upbit.Net/UpbitEnvironment.cs).

## Local order books and trackers

`IUpbitOrderBookFactory` exposes `Spot`, `Create(SharedSymbol, options)`, and `CreateSpot(nativeSymbol, options)`. Create a book for the configured environment, check `StartAsync()` success, then call `StopAsync()` and dispose it when finished. `IUpbitTrackerFactory` implements `ITrackerFactory` for public kline/trade tracking; this does not provide private account or order trackers.

Checkout-only source references: [public contracts](../../../../Upbit.Net/Interfaces/Clients), [order-book factory](../../../../Upbit.Net/Interfaces/IUpbitOrderBookFactory.cs), [tracker factory](../../../../Upbit.Net/Interfaces/IUpbitTrackerFactory.cs), and [DI registration](../../../../Upbit.Net/ExtensionMethods/ServiceCollectionExtensions.cs).
