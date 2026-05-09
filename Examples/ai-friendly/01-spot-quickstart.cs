// 01-spot-quickstart.cs
//
// Demonstrates: public Spot market-data basics with Upbit.Net.
//
// Setup: dotnet add package JKorf.Upbit.Net

using Upbit.Net.Clients;
using Upbit.Net.Enums;

// Upbit.Net currently exposes public quotation data only.
// Credentials are not configured and there are no account/trading endpoints.
var client = new UpbitRestClient();

// Upbit native symbols use QUOTE-BASE format.
// Examples: KRW-BTC, USDT-ETH, SGD-BTC, IDR-BTC, THB-BTC.
const string symbol = "USDT-ETH";

// ---- 1. TICKER ----
var ticker = await client.SpotApi.ExchangeData.GetTickerAsync(symbol);
if (!ticker.Success)
{
    Console.WriteLine($"Ticker request failed: {ticker.Error}");
    return;
}

Console.WriteLine($"{ticker.Data.Symbol} last price: {ticker.Data.LastPrice}");
Console.WriteLine($"24h base volume: {ticker.Data.Volume24h}");
Console.WriteLine($"24h quote volume: {ticker.Data.QuoteVolume24h}");

// ---- 2. SUPPORTED SYMBOLS ----
// includeNotifications: true maps to Upbit's is_details parameter.
var symbols = await client.SpotApi.ExchangeData.GetSymbolsAsync(includeNotifications: true);
if (!symbols.Success)
{
    Console.WriteLine($"Symbol request failed: {symbols.Error}");
    return;
}

Console.WriteLine($"Received {symbols.Data.Length} supported symbols.");

// ---- 3. RECENT CANDLES ----
var candles = await client.SpotApi.ExchangeData.GetKlinesAsync(
    symbol,
    KlineInterval.OneMinute,
    limit: 5);

if (!candles.Success)
{
    Console.WriteLine($"Kline request failed: {candles.Error}");
    return;
}

foreach (var candle in candles.Data)
{
    Console.WriteLine(
        $"{candle.OpenTime:u} O={candle.OpenPrice} H={candle.HighPrice} L={candle.LowPrice} C={candle.ClosePrice}");
}

// ---- 4. ORDER BOOK ----
var orderBook = await client.SpotApi.ExchangeData.GetOrderBookAsync(symbol, levels: 15);
if (!orderBook.Success)
{
    Console.WriteLine($"Order book request failed: {orderBook.Error}");
    return;
}

var best = orderBook.Data.Entries.FirstOrDefault();
if (best != null)
{
    Console.WriteLine($"Best bid: {best.BidPrice} x {best.BidQuantity}");
    Console.WriteLine($"Best ask: {best.AskPrice} x {best.AskQuantity}");
}

// Common variations:
//   Multiple tickers:      GetTickersAsync(new[] { "USDT-ETH", "USDT-BTC" })
//   All quote tickers:     GetTickersByQuoteAssetsAsync(new[] { "USDT", "BTC" })
//   Trade history:         GetTradeHistoryAsync(symbol, limit: 100)
//   Daily candles:         GetKlinesAsync(symbol, KlineInterval.OneDay, limit: 30)
