# Working on Upbit.Net

Consumer guidance belongs to the [Upbit.Net skill](.claude/skills/upbit-net/SKILL.md). Use [llms.txt](llms.txt) to find its references. This file describes repository development.

## Layout and conventions

- [Client interfaces](Upbit.Net/Interfaces/Clients) and [implementations](Upbit.Net/Clients): public contracts, native REST/socket calls, V1 compatibility, and V2 adapters.
- [Models and options](Upbit.Net/Objects), [enums](Upbit.Net/Enums), and [converters](Upbit.Net/Converters): wire models, configuration, enum mappings, and JSON source-generation metadata.
- [Regional environments](Upbit.Net/UpbitEnvironment.cs), [DI registration](Upbit.Net/ExtensionMethods/ServiceCollectionExtensions.cs), and [local order books](Upbit.Net/SymbolOrderBooks): environment selection and lifecycle integration.
- [Tests](Upbit.Net.UnitTests): request/response fixtures, socket fixtures, integration tests, and example compilation.
- [Examples](Examples/README.md): console, API, order book, tracker, and standalone programs.

Follow neighboring implementations for namespaces, XML documentation, nullability, request definitions, validation, result mapping, cancellation, and `ConfigureAwait(false)`. When changing an endpoint, update its interface, implementation, wire models, enum mappings, serialization context, and relevant fixtures together. Keep Shared API adapters and capability options consistent with supported native behavior and regional environments. Public spot market data is the current implemented scope; do not document private account APIs based on another library's structure.

Preserve public compatibility and all declared target frameworks. Keep unrelated dependencies, release configuration, and generated output outside focused changes.

## Build and test

Use the SDK version configured in [CI](.github/workflows/dotnet.yml). From the repository root:

```powershell
dotnet restore Upbit.Net.sln
dotnet build Upbit.Net.sln --no-restore
dotnet test Upbit.Net.UnitTests/Upbit.Net.UnitTests.csproj --no-build --filter "FullyQualifiedName~AiExampleCompileTests"
```

Debug builds normally target only `netstandard2.0` and `net11.0`. Validate every declared target with:

```powershell
dotnet build Upbit.Net/Upbit.Net.csproj -c Release -p:BuildAllFrameworks=true
dotnet pack Upbit.Net/Upbit.Net.csproj -c Release --no-build
```

Run the request and socket fixture tests relevant to code changes. `Documentation/AiExampleCompileTests.cs` compiles all standalone AI examples without running them. Live tests are separate: `UpbitRestIntegrationTests` has a `Run` gate and `SharedRestIntegrationTests` uses `ManualRun` or `INTEGRATION=1`. Do not enable live exchange tests as part of documentation validation. Compiling the [complete examples](Examples/Examples.sln) does not execute their exchange calls.

## Documentation ownership

Maintain consumer routing in `.claude/skills/upbit-net/references/api-surfaces.md`, examples in `references/usage.md`, and distinct operational considerations in `references/safety.md`. Keep the skill entry point concise; `CLAUDE.md` imports this file and `llms.txt` is a link index.

The [project file](Upbit.Net/Upbit.Net.csproj) packs `.claude/skills/**` under `skills/`, without appending `%(RecursiveDir)`. Validate local links and anchors, compile changed snippets and examples, and inspect the archive: exactly five skill files under `skills/upbit-net/`, identical to their source files. Label repository links inside the skill as checkout-only so the packaged guidance remains usable independently.
