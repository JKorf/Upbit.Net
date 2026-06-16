// 05-error-handling.cs
//
// Demonstrates: HttpResult patterns, retry logic, and common Upbit.Net pitfalls.
//
// Setup: dotnet add package JKorf.Upbit.Net

using CryptoExchange.Net.Objects;
using Upbit.Net.Clients;
using Upbit.Net.Enums;
using Upbit.Net.Objects.Models;

var client = new UpbitRestClient();

// ---- 1. THE BASIC PATTERN ----
// Direct and SharedApis REST methods return HttpResult<T> or HttpResult.
// Direct and SharedApis WebSocket subscriptions return WebSocketResult<UpdateSubscription>.
// .Success is true/false. .Data is the payload, valid only when .Success is true.
// .Error contains structured error info when .Success is false.
// .Error.IsTransient hints if retry may succeed, for example after network or server issues.

var result = await client.SpotApi.ExchangeData.GetTickerAsync("USDT-ETH");

if (result.Success)
{
    Console.WriteLine($"Price: {result.Data.LastPrice}");
}
else
{
    Console.WriteLine($"Code:      {result.Error?.Code}");
    Console.WriteLine($"Message:   {result.Error?.Message}");
    Console.WriteLine($"Type:      {result.Error?.ErrorType}");
    Console.WriteLine($"Transient: {result.Error?.IsTransient}");
}

// ---- 2. SIMPLE RETRY WITH BACKOFF ----
// Retry only on transient errors. Do not retry invalid symbols or unsupported features.

async Task<HttpResult<T>> WithRetry<T>(
    Func<Task<HttpResult<T>>> call,
    int maxAttempts = 3)
{
    HttpResult<T> last = default!;

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        last = await call();
        if (last.Success)
            return last;

        if (last.Error?.IsTransient != true)
            return last;

        await Task.Delay(TimeSpan.FromMilliseconds(250 * Math.Pow(2, attempt)));
    }

    return last;
}

var ticker = await WithRetry(
    () => client.SpotApi.ExchangeData.GetTickerAsync("USDT-ETH"));

if (!ticker.Success)
    Console.WriteLine($"Final failure: {ticker.Error}");

// ---- 3. COMMON UPBIT.NET ERROR SCENARIOS ----
//
// Invalid symbol:
//   Upbit symbols use QUOTE-BASE format. Use "USDT-ETH", not "ETHUSDT".
//   Verify symbols with GetSymbolsAsync(includeNotifications: true).
//
// Unsupported authenticated feature:
//   Current Upbit.Net exposes public quotation APIs only. Do not generate
//   SpotApi.Trading, SpotApi.Account, wallet, private stream, or API-key code.
//
// Wrong region or quote asset:
//   Korea live commonly uses KRW/BTC/USDT quote markets. Singapore uses SGD,
//   Indonesia uses IDR, and Thailand uses THB. Select the right UpbitEnvironment.
//
// Invalid kline interval:
//   Use Upbit.Net.Enums.KlineInterval values. Do not pass arbitrary strings.
//
// Rate limit / network issue:
//   The library has client-side rate limiting, but server/network failures can
//   still occur. Retry only when result.Error?.IsTransient == true.

// ---- 4. VALIDATE SYMBOL BEFORE REQUESTING SPECIFIC DATA ----
async Task<bool> SupportsSymbolAsync(string symbol)
{
    var symbols = await client.SpotApi.ExchangeData.GetSymbolsAsync(includeNotifications: true);
    if (!symbols.Success)
    {
        Console.WriteLine($"Could not load symbols: {symbols.Error}");
        return false;
    }

    return symbols.Data.Any(x => string.Equals(x.Symbol, symbol, StringComparison.OrdinalIgnoreCase));
}

if (!await SupportsSymbolAsync("USDT-ETH"))
{
    Console.WriteLine("USDT-ETH is not available in the selected Upbit environment.");
    return;
}

// ---- 5. WRAP CALLS FOR CALLER-FRIENDLY ERRORS ----
async Task<UpbitKline[]> GetRecentCandlesOrEmptyAsync(string symbol)
{
    var candles = await WithRetry(
        () => client.SpotApi.ExchangeData.GetKlinesAsync(symbol, KlineInterval.OneMinute, limit: 20));

    if (candles.Success)
        return candles.Data;

    Console.WriteLine($"Could not load candles for {symbol}: {candles.Error}");
    return Array.Empty<UpbitKline>();
}

var recentCandles = await GetRecentCandlesOrEmptyAsync("USDT-ETH");
Console.WriteLine($"Candles loaded: {recentCandles.Length}");

// ---- 6. EXCEPTIONS VS ERROR RESULTS ----
// Upbit.Net returns normal API/network/rate-limit failures via HttpResult.Error.
// Exceptions are reserved for disposal, cancellation, invalid local arguments, or
// other programming/misconfiguration issues.

// Common variations:
//   With CancellationToken:    pass `ct: cancellationToken` to any method.
//   With timeout per request:  options.RequestTimeout = TimeSpan.FromSeconds(10).
//   Polly integration:         use IsTransient as the retry predicate.
