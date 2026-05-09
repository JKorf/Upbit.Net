// 03-websocket.cs
//
// Demonstrates: public WebSocket subscriptions for ticker, trades,
// order books, and klines. Includes proper teardown.
//
// Setup: dotnet add package JKorf.Upbit.Net

using Upbit.Net.Clients;
using Upbit.Net.Enums;

var socketClient = new UpbitSocketClient();
const string symbol = "USDT-ETH";

// ---- 1. TICKER STREAM ----
var tickerSub = await socketClient.SpotApi.SubscribeToTickerUpdatesAsync(
    symbol,
    update =>
    {
        Console.WriteLine($"{update.Data.Symbol} ticker: {update.Data.LastPrice}");
    });

if (!tickerSub.Success)
{
    Console.WriteLine($"Failed to subscribe ticker: {tickerSub.Error}");
    return;
}

// ---- 2. TRADE STREAM ----
var tradeSub = await socketClient.SpotApi.SubscribeToTradeUpdatesAsync(
    symbol,
    update =>
    {
        Console.WriteLine($"{update.Data.Symbol} trade: {update.Data.Quantity} @ {update.Data.Price}");
    });

if (!tradeSub.Success)
{
    Console.WriteLine($"Failed to subscribe trades: {tradeSub.Error}");
    await socketClient.UnsubscribeAsync(tickerSub.Data);
    return;
}

// ---- 3. ORDER BOOK STREAM ----
// Upbit supports 1, 5, 15, or 30 levels for the shared socket order book shape.
var orderBookSub = await socketClient.SpotApi.SubscribeToOrderBookUpdatesAsync(
    symbol,
    levels: 15,
    update =>
    {
        var best = update.Data.Entries.FirstOrDefault();
        if (best != null)
            Console.WriteLine($"{update.Data.Symbol} bid={best.BidPrice} ask={best.AskPrice}");
    });

if (!orderBookSub.Success)
{
    Console.WriteLine($"Failed to subscribe order book: {orderBookSub.Error}");
    await socketClient.UnsubscribeAsync(tickerSub.Data);
    await socketClient.UnsubscribeAsync(tradeSub.Data);
    return;
}

// ---- 4. KLINE STREAM ----
var klineSub = await socketClient.SpotApi.SubscribeToKlineUpdatesAsync(
    symbol,
    KlineInterval.OneMinute,
    update =>
    {
        Console.WriteLine($"{update.Data.Symbol} 1m close: {update.Data.ClosePrice}");
    });

if (!klineSub.Success)
{
    Console.WriteLine($"Failed to subscribe klines: {klineSub.Error}");
    await socketClient.UnsubscribeAsync(tickerSub.Data);
    await socketClient.UnsubscribeAsync(tradeSub.Data);
    await socketClient.UnsubscribeAsync(orderBookSub.Data);
    return;
}

Console.WriteLine("Subscriptions active. Press Enter to teardown...");
Console.ReadLine();

// ---- 5. TEARDOWN ----
// Always unsubscribe on shutdown to release resources cleanly.
await socketClient.UnsubscribeAsync(tickerSub.Data);
await socketClient.UnsubscribeAsync(tradeSub.Data);
await socketClient.UnsubscribeAsync(orderBookSub.Data);
await socketClient.UnsubscribeAsync(klineSub.Data);

Console.WriteLine("Clean shutdown complete.");

// Common variations:
//   Multiple symbols:  SubscribeToTickerUpdatesAsync(new[] { "USDT-ETH", "USDT-BTC" }, handler)
//   Aggregation:       SubscribeToOrderBookUpdatesAsync(symbol, 15, handler, aggregation: 0.01m)
//   All shutdown:      await socketClient.UnsubscribeAllAsync()
//   Reconnect events:  tickerSub.Data.ConnectionLost / ConnectionRestored
