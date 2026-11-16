---
name: upbit-net
description: Build C#/.NET Upbit public spot market-data integrations with Upbit.Net, including regional REST clients, WebSocket streams, local order books, trackers, and Shared API V2 capabilities. Use for Upbit symbols, tickers, trades, candles, and order-book workflows.
---

# Upbit.Net

Install the concrete package:

```shell
dotnet add package JKorf.Upbit.Net
```

Use `UpbitRestClient` and `UpbitSocketClient` from `Upbit.Net.Clients`. The implemented surface is public spot quotation data: REST through `SpotApi.ExchangeData`, streams through `SpotApi`. It provides no credential configuration, authenticated accounts, trading, wallets, futures, or private streams. Explain that boundary when those features are requested.

Choose the same `UpbitEnvironment` for REST and socket clients. Native symbols use `QUOTE-BASE`, for example `USDT-ETH`; verify markets in the selected region. Regional environments are live environments, not testnets.

Check `Success` before accessing `Data`: REST returns `HttpResult<T>`, subscriptions return `WebSocketResult<UpdateSubscription>`, and some shared cache helpers return `ExchangeCallResult<T>`. Keep clients for the intended workload, retain successful subscriptions, and unsubscribe before disposing the owning socket client. Respect container ownership for DI-managed clients.

For portable code, use narrow V2 capabilities through `.SharedApi`; `.SharedClient` is the V1 compatibility surface. `IUpbitSharedApiClient` exposes `SpotRest` and `SpotSocket` and supports runtime discovery. Handle missing capability resolutions and inspect their options rather than assuming support. Shared requests and models are separate from native Upbit types. No additional skill is required.

## References

- [API surfaces](references/api-surfaces.md): endpoint routing, regional selection, supported V2 capabilities, catalogs, quantities, and factories.
- [Usage](references/usage.md): standalone setup, results, subscriptions, Shared API and DI snippets.
- [Operational considerations](references/safety.md): region and aggregation semantics, freshness, reconnects, and bounded retries.

Checkout-only: [repository instructions](../../../AGENTS.md) and [complete examples](../../../Examples/README.md). The references above are self-contained when distributed in the NuGet package.
