# Standalone examples

Each `.cs` file is an independent top-level console program for public Upbit spot market data. Copy one file into `Program.cs` at a time.

| File | Purpose |
| --- | --- |
| [01-spot-quickstart.cs](01-spot-quickstart.cs) | Ticker, symbols, candles, and order book |
| [02-market-data.cs](02-market-data.cs) | Regional markets, trades, tickers, and configuration |
| [03-websocket.cs](03-websocket.cs) | Four native streams with cleanup on partial setup or shutdown |
| [04-multi-exchange.cs](04-multi-exchange.cs) | Shared API V2 REST and socket capabilities |
| [05-error-handling.cs](05-error-handling.cs) | Error inspection, bounded read retries, and symbol validation |

```shell
dotnet new console -n MyUpbitApp
cd MyUpbitApp
dotnet add package JKorf.Upbit.Net
# Copy ONE example into Program.cs
dotnet build
# Optional: run it to contact the exchange
dotnet run
```

Compilation performs no exchange operations. Running the socket examples waits for Enter before unsubscribing. See the [consumer skill](../../.claude/skills/upbit-net/SKILL.md) for routing and conventions, or the [complete projects](../README.md).
