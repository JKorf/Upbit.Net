# AI-Friendly Examples

These examples are optimized for AI coding assistants and quick onboarding. Each file is:

- **Compilable**: drop into a console project with `dotnet add package JKorf.Upbit.Net` and it builds.
- **Self-contained**: single file, no external setup, no shared helpers.
- **Heavily commented**: explains why each line exists, not just what it does.
- **Idiomatic**: follows current Upbit.Net 2.x and CryptoExchange.Net 11.x patterns.

Important Upbit.Net scope: examples use public Spot market data only. This library does not expose authenticated account, wallet, order placement, futures, or private stream endpoints.

## Files

| File | What it shows |
|---|---|
| `01-spot-quickstart.cs` | Client setup, public ticker, supported symbols, candles, order book |
| `02-market-data.cs` | Symbols, regional quote assets, tickers, trades, klines, order books, symbol config |
| `03-websocket.cs` | Subscribe to ticker, trade, order book, and kline streams with proper teardown |
| `04-multi-exchange.cs` | `CryptoExchange.Net.SharedApis` pattern for exchange-agnostic market data |
| `05-error-handling.cs` | `WebCallResult` patterns, retry, common Upbit.Net pitfalls |

## Running

```bash
dotnet new console -n MyUpbitApp
cd MyUpbitApp
dotnet add package JKorf.Upbit.Net
# Copy the example .cs file content into Program.cs
dotnet run
```
