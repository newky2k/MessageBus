# MessageBus
MessageBus is a cross platform EventBus system similar to `NSNoticationCenter` on iOS and `otto` on Android that allow you to decouple your code, whilst still allowing your applications components to communicate with each other. MessageBus can be used instead of events, can be used to communicate between objects that are not directly linked.

# Features

* Cross-platform  
  * Works on iOS, WatchOS, TVOS, Android, UWP, Tizen(4.0), Mac and Windows
    * .NETCore 3.1, .NET 5+, WPF, UWP and WinUI
* Small footprint
* Simple API
* Create custom events to easily pass additional data
* Allows you to decouple objects and classes within your projects  

### Attribution

`ThreadControl` contains portions of code from [Xamarin.Essentials](https://github.com/xamarin/Essentials/tree/master/Xamarin.Essentials/MainThread), specifically the `MainThread` functionality
