# MessageBus
MessageBus is a cross platform EventBus system similar to `NSNoticationCenter` on iOS and `otto` on Android that allow you to decouple your code, whilst still allowing your applications components to communicate with each other. MessageBus can be used instead of events, can be used to communicate between objects that are not directly linked.

# Features

* Cross-platform  
  * .NETStandard 2.1, .NET 8+ including iOS, TVOS, Android, UWP, Mac and Windows with support for WPF and WinUI
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

### Attribution

`ThreadControl` contains portions of code from [Xamarin.Essentials](https://github.com/xamarin/Essentials/tree/master/Xamarin.Essentials/MainThread), specifically the `MainThread` functionality
