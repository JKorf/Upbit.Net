// 02-market-data.cs
//
// Demonstrates: the public Spot REST surface available in Upbit.Net.
//
// Setup: dotnet add package JKorf.Upbit.Net

using Upbit.Net;
using Upbit.Net.Clients;
using Upbit.Net.Enums;

// Upbit has regional live environments. Pick the region matching the markets
// you want to query. Live is the South Korea environment.
var client = new UpbitRestClient(options =>
{
    options.Environment = UpbitEnvironment.Live;
});

const string symbol = "USDT-ETH";

// ---- 1. SYMBOLS / MARKETS ----
var symbols = await client.SpotApi.ExchangeData.GetSymbolsAsync(includeNotifications: true);
if (!symbols.Success)
{
    Console.WriteLine($"Failed to load symbols: {symbols.Error}");
    return;
}

foreach (var market in symbols.Data.Take(5))
    Console.WriteLine($"Symbol: {market.Symbol} ({market.NameKorean} / {market.Name})");

// ---- 2. TICKERS ----
// One ticker:
var oneTicker = await client.SpotApi.ExchangeData.GetTickerAsync(symbol);
if (!oneTicker.Success)
{
    Console.WriteLine($"Failed to load ticker: {oneTicker.Error}");
    return;
}

Console.WriteLine($"{symbol}: {oneTicker.Data.LastPrice}");

// Multiple tickers:
var manyTickers = await client.SpotApi.ExchangeData.GetTickersAsync(new[] { "USDT-ETH", "USDT-BTC" });
if (!manyTickers.Success)
{
    Console.WriteLine($"Failed to load tickers: {manyTickers.Error}");
    return;
}

// All tickers for quote assets. For Korea use KRW/BTC/USDT. Other environments
// have their own fiat quote asset, for example SGD, IDR, or THB.
var quoteTickers = await client.SpotApi.ExchangeData.GetTickersByQuoteAssetsAsync(new[] { "KRW", "BTC", "USDT" });
if (!quoteTickers.Success)
{
    Console.WriteLine($"Failed to load quote tickers: {quoteTickers.Error}");
    return;
}

Console.WriteLine($"Quote ticker count: {quoteTickers.Data.Length}");

// ---- 3. TRADES ----
var trades = await client.SpotApi.ExchangeData.GetTradeHistoryAsync(symbol, limit: 10);
if (!trades.Success)
{
    Console.WriteLine($"Failed to load trades: {trades.Error}");
    return;
}

foreach (var trade in trades.Data.Take(3))
    Console.WriteLine($"{trade.Timestamp:u} {trade.OrderSide} {trade.Quantity} @ {trade.Price}");

// ---- 4. CANDLES ----
var klines = await client.SpotApi.ExchangeData.GetKlinesAsync(
    symbol,
    KlineInterval.OneMinute,
    endTime: DateTime.UtcNow,
    limit: 10);

if (!klines.Success)
{
    Console.WriteLine($"Failed to load klines: {klines.Error}");
    return;
}

Console.WriteLine($"Klines received: {klines.Data.Length}");

// ---- 5. ORDER BOOKS ----
var orderBook = await client.SpotApi.ExchangeData.GetOrderBookAsync(symbol, levels: 15);
if (!orderBook.Success)
{
    Console.WriteLine($"Failed to load order book: {orderBook.Error}");
    return;
}

var orderBooks = await client.SpotApi.ExchangeData.GetOrderBooksAsync(new[] { "USDT-ETH", "USDT-BTC" }, levels: 15);
if (!orderBooks.Success)
{
    Console.WriteLine($"Failed to load order books: {orderBooks.Error}");
    return;
}

// ---- 6. SYMBOL CONFIG ----
// Symbol config exposes order book instrument details such as tick quantity.
var config = await client.SpotApi.ExchangeData.GetSymbolConfigAsync(symbol);
if (!config.Success)
{
    Console.WriteLine($"Failed to load symbol config: {config.Error}");
    return;
}

foreach (var item in config.Data)
    Console.WriteLine($"{item.Symbol} tick quantity: {item.TickQuantity}");

// Common variations:
//   Singapore region:  options.Environment = UpbitEnvironment.Singapore; quote assets SGD/BTC/USDT
//   Indonesia region:  options.Environment = UpbitEnvironment.Indonesia; quote assets IDR/BTC/USDT
//   Thailand region:   options.Environment = UpbitEnvironment.Thailand; quote assets THB/BTC/USDT
