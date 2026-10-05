# MessageBus
MessageBus is a cross platform EventBus system similar to `NSNoticationCenter` on iOS and `otto` on Android that allow you to decouple your code, whilst still allowing your applications components to communicate with each other. MessageBus can be used instead of events, can be used to communicate between objects that are not directly linked.

# Features

* Cross-platform  
  * .NETStandard 2.1, .NET Framework 4.7.2, .NET 8 and .NET 10 including iOS, tvOS, Android, Mac Catalyst, macOS and Windows with support for WPF and WinUI
* Small footprint
* Simple API
* Create custom events to easily pass additional data
* Allows you to decouple objects and classes within your projects
* New in 3.1 - Dependency Injection support

## Usage

To use Dependency Injection follow these steps.

Add package `DSoft.MessageBus` to your main application and call `RegisterMessageBus` to register the services.

	private static void ConfigureServices(IServiceCollection services)
    {
        services.RegisterMessageBus();
    }

You can then inject `IMessageBusService` into your own services.

Please check the Unit tests and sample WPF app for examples of usage

## Building and releasing

Builds run on GitHub Actions:

* `.github/workflows/ci.yml` validates every pull request into `main`: it builds Release and runs the tests, and publishes nothing.
* `.github/workflows/release.yml` runs on every push to `main` (or manually from the Actions tab). It builds, tests, pushes the packages to nuget.org as `4.5.<yyMM>.<run number>`, then tags the commit and creates a GitHub release with the packages attached. Set `RELEASE_SUFFIX` (e.g. `-prerelease`) to publish a prerelease.

Publishing uses [NuGet trusted publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing), so no API key is stored: the nuget.org policy trusts workflow `release.yml` in environment `nuget`, and the `NUGET_USER` secret holds the nuget.org profile name.

### Attribution

`ThreadControl` contains portions of code from [Xamarin.Essentials](https://github.com/xamarin/Essentials/tree/master/Xamarin.Essentials/MainThread), specifically the `MainThread` functionality
