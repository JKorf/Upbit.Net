# Operational considerations

## Regional markets and aggregation

Upbit regions have separate endpoints and market catalogs. A correctly formatted symbol can still be unavailable in the selected region. Apply environment selection consistently to REST, sockets, factories, and any persisted catalog. Custom endpoints do not establish a sandbox or add private API support.

Order-book levels and price aggregation change the meaning of returned depth. Supported aggregation can depend on the market; use symbol configuration and the operation's supported options. Do not infer executable liquidity at a specific price from an aggregated bucket. Sparse candles or trades need not represent a transport failure.

## Stream and local state

A successful subscription acknowledges setup; it does not guarantee continuous or current data. Handle interruption and reconnection, keep callbacks short, and bound any queue used to process updates. Distinguish the latest received value from a verified current snapshot.

For a maintained local order book, use the factory lifecycle rather than treating one stream payload as a synchronized book. Check startup failure and stop the book on shutdown. Retain and clean up every successful subscription even if a later subscription fails. Cached symbols and tracker history can become stale independently of a socket connection.

## Retries and error interpretation

Public read retries can use `Error.IsTransient` as one signal, with bounded attempts, cancellation, and backoff. Do not keep retrying invalid symbols, unsupported intervals/levels, or wrong-region markets. Rate limiting can still apply to retries and reconnects.

Empty data and a failed request have different meanings; preserve the error when downstream consumers need that distinction. Current Upbit.Net supplies no account-changing calls, so order idempotency, batch trading outcomes, cancellation races, and position-mode behavior are outside this implemented surface.
