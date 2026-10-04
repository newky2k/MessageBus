# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

DSoft.MessageBus is a cross-platform EventBus/messaging library (like `NSNotificationCenter` on iOS, `otto` on Android) that decouples application components. Shipped as two NuGet packages: `DSoft.MessageBus` (main) and `DSoft.MessageBus.Core` (contracts/core types).

## Build & Test

Solution file is `MessageBus.slnx` (new XML solution format — requires recent .NET 10 SDK / VS).

```bash
dotnet restore MessageBus.slnx
dotnet build MessageBus.slnx --configuration Release
dotnet test UnitTest/UnitTest.csproj          # tests target net9.0, MSTest
dotnet test UnitTest/UnitTest.csproj --filter "FullyQualifiedName~MessageBusTest.MethodName"   # single test
```

Building the full `DSoft.Messaging` project requires platform workloads (`dotnet workload restore`) because it multi-targets iOS/Android/macOS/tvos/MacCatalyst/Windows. To iterate quickly without workloads, build/test against a single TFM via the UnitTest project (which references the main project but is consumed as `net9.0`). Note `Directory.Build.props` sets `GeneratePackageOnBuild=true` and signs assemblies with `DSoft.snk`.

## Architecture

Three source projects plus a WPF sample:

- **DSoft.MessageBus.Core** (`netstandard2.0`) — platform-agnostic contracts and data types: `IMessageBusService`, the abstract `MessageBusEvent` + concrete `CoreMessageBusEvent`, `MessageBusEventHandler`/`TypedMessageBusEventHandler`, `LogEvent`, `ILogListener`, and the handler/listener collections. Namespace is `DSoft.MessageBus` (not `.Core`).
- **DSoft.Messaging** (multi-target, PackageId `DSoft.MessageBus`) — the runtime: `MessageBusService` (internal impl of `IMessageBusService`), the static `MessageBus` facade, DI registration, and `ThreadControl`.
- **UnitTest** — MSTest. `BaseTest` builds a DI `ServiceProvider` via `RegisterMessageBus()` in `[AssemblyInitialize]`; resolve `IMessageBusService` from `BaseTest.Provider`.

### Two ways to consume the bus
1. **Static facade** — `MessageBus.Post/Subscribe/...` wraps a lazily-created singleton `MessageBusService`.
2. **DI** — call `services.RegisterMessageBus()` (extension in `Microsoft.Extensions.DependencyInjection` namespace) to register `IMessageBusService` as a singleton, then inject it.

Both routes hit the same `MessageBusService` logic.

### Event dispatch model
- **String-id events**: subscribe/post by `eventId` string. Matching is **case-insensitive** (`HandlersForEvent` lowercases both sides). `CoreMessageBusEvent` auto-generates a GUID id if none supplied.
- **Typed events**: subclass `MessageBusEvent`, override `EventId`, subscribe with `Subscribe<T>()`. These register as `TypedMessageBusEventHandler` and dispatch by `Type`. In `PostInternal`, a non-`CoreMessageBusEvent` is delivered to both type handlers **and** any handlers registered for its `EventId`.
- `RunPostOnSeperateTask` (default false) offloads `Post`/`Log` to `Task.Run`; `PostAsync` variants always run on a background task.

### Platform-specific compilation
`DSoft.Messaging` uses **filename-suffix conventions** instead of one file per TFM — the `.csproj` includes files by suffix via `EnableDefaultCompileItems=false`:
- `*.shared.cs` — all targets
- `*.netstandard.cs` — netstandard/net8/net9/net10/net472
- `*.wpf.cs` (windows7.0), `*.winui.cs` (windows10), `*.android.cs`, `*.ios.cs` (ios+maccatalyst), `*.mac.cs` (macos), `*.tvos.cs`

`ThreadControl` is the main example: `ThreadControl.shared.cs` defines the public API and calls `partial` members (`PlatformIsMainThread`, `PlatformBeginInvokeOnMainThread`) implemented per platform. When adding platform-specific code, add a new `*.<platform>.cs` partial rather than `#if`. (ThreadControl's `MainThread` logic is adapted from Xamarin.Essentials.)

## Conventions
- Keep `IMessageBusService`, the static `MessageBus` facade, and `MessageBusService` in sync — all three expose the same Post/Subscribe/Unsubscribe/Log surface.
- New public APIs need XML doc comments (`GenerateDocumentationFile=true`; many nullable/doc warnings are suppressed in `Directory.Build.props`).
- Version/release notes live in the `.csproj` PropertyGroups (`Version`, `AssemblyVersion`, `PackageReleaseNotes`); bump there for releases.
