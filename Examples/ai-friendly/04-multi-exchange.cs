// 04-multi-exchange.cs
//
// Demonstrates: writing exchange-agnostic public market-data code using
// CryptoExchange.Net.SharedApis. Same pattern works across Upbit, Binance,
// OKX, Kraken, and other CryptoExchange.Net libraries when the exchange
// supports the requested shared interface.
//
// Setup:
//   dotnet add package JKorf.Upbit.Net
//   dotnet add package Binance.Net   // optional, for another exchange
//   dotnet add package JK.OKX.Net    // optional, for another exchange

using Upbit.Net.Clients;
using CryptoExchange.Net.SharedApis;

// ---- THE PATTERN ----
// Each exchange client exposes a `.SharedClient` property on supported API surfaces.
// Upbit.Net implements shared public market-data interfaces such as:
// ISpotTickerRestClient, ISpotSymbolRestClient, IOrderBookRestClient,
// IRecentTradeRestClient, IKlineRestClient, ITickerSocketClient, and more.

ISpotTickerRestClient upbitShared = new UpbitRestClient().SpotApi.SharedClient;

// Common symbol type. SharedSymbol lets each exchange format the symbol in its
// native style. Upbit uses "USDT-ETH"; Binance uses "ETHUSDT"; OKX uses "ETH-USDT".
var ethusdt = new SharedSymbol(TradingMode.Spot, "ETH", "USDT");

await PrintTicker(upbitShared, ethusdt);

// ---- AGNOSTIC METHOD: works against any exchange implementing ISpotTickerRestClient ----
async Task PrintTicker(ISpotTickerRestClient client, SharedSymbol symbol)
{
    var result = await client.GetSpotTickerAsync(new GetTickerRequest(symbol));
    if (!result.Success)
    {
        Console.WriteLine($"[{client.Exchange}] Failed: {result.Error}");
        return;
    }

    Console.WriteLine($"[{client.Exchange}] {result.Data.Symbol}: {result.Data.LastPrice}");
}

// ---- OTHER SHARED REST INTERFACES SUPPORTED BY UPBIT.NET ----
IOrderBookRestClient orderBookClient = new UpbitRestClient().SpotApi.SharedClient;
var orderBook = await orderBookClient.GetOrderBookAsync(
    new GetOrderBookRequest(ethusdt)
    {
        Limit = 15
    },
    ct: default);

if (orderBook.Success)
    Console.WriteLine($"[{orderBookClient.Exchange}] order book received");

// ---- WEBSOCKET EXAMPLE: SHARED SUBSCRIPTION ----
var upbitSocket = new UpbitSocketClient();
ITickerSocketClient upbitTickerSocket = upbitSocket.SpotApi.SharedClient;

var sub = await upbitTickerSocket.SubscribeToTickerUpdatesAsync(
    new SubscribeTickerRequest(ethusdt),
    update => Console.WriteLine($"[{upbitTickerSocket.Exchange}] {update.Data.Symbol}: {update.Data.LastPrice}"));

if (!sub.Success)
{
    Console.WriteLine($"Subscribe failed: {sub.Error}");
    return;
}

Console.WriteLine("Press Enter to exit");
Console.ReadLine();

await upbitSocket.UnsubscribeAsync(sub.Data);

// Common variations:
//   Multi-exchange scanner: loop over List<ISpotTickerRestClient>, compare prices.
//   Cross-exchange orderbook: use IOrderBookSocketClient on each exchange.
//   Unified charting: use IKlineRestClient or IKlineSocketClient across exchanges.
//   Symbol aliases: SharedSymbol handles common assets; exotic assets may need exchange-specific checks.
