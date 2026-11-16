# Examples

| Example | Purpose |
| --- | --- |
| [Web API](Upbit.Examples.Api/Upbit.Examples.Api.csproj) | Inject an Upbit client into a minimal API |
| [Console](Upbit.Examples.Console/Upbit.Examples.Console.csproj) | Public market-data reads |
| [Order book](Upbit.Examples.OrderBook/Upbit.Examples.OrderBook.csproj) | Maintained local order book |
| [Tracker](Upbit.Examples.Tracker/Upbit.Examples.Tracker.csproj) | Trade tracking |
| [Standalone programs](ai-friendly/README.md) | Focused REST, WebSocket, Shared API V2, and error-handling examples |

Build the [example solution](Examples.sln) with `dotnet build Examples/Examples.sln` from the repository root. Builds do not execute exchange calls. Running these programs connects to live public markets.

See the [consumer skill](../.claude/skills/upbit-net/SKILL.md) for API guidance and [repository instructions](../AGENTS.md) for validation.
