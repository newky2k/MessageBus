MessageBus is a cross platform EventBus system similar to NSNoticationCenter on iOS and otto on Android that allows you to decouple your code whilst still allowing your applications components to communicate with each other.  MesssageBus can be used instead of events and can be used to communicate between objects that are not directly linked.

# Features

* Cross platform  
  * Works on iOS, Android, Windows Phone, Mac and Windows
* Small footprint
* Simple API
* Create custom events to easily pass additional data
* Allows you to decouple objects and classes within your projects  
* Portable Class library


## Usage

`MessageBus.Default` provides a central Multi-thread singleton for you to subscribe to and post events.  You can use this or have multiple separate `MessageBus` objects of your own.


	using DSoft.Messaging;
	...
	
	var newBus = new MessageBus();
	 

To subscribe to an event you can create a `MessageBusEventHandler` object, and then register it with the MessageBus.  The event handler allows you to set the Id of the event to subscribe to and an Action or Delegate to call be when the event occurs.

	using DSoft.Messaging;
	...
	
	var newEvHandler = new MessageBusEventHandler()
	{
		EventId = "1234",
		Action = (sender, data) =>
		{
			//Code goes here
		},
	};
	
	MessageBus.Default.Register(newEvHandler);
	
You can then post an event from anywhere in your application using the Id of the event to execute the Action in the registered `MessageBusEventHandler` objects.

To post an event you create an instance of `CoreMessageBusEvent`, set the EventId, sender and any additional data you want to send and then call Post on the relevant MessageBus.
 
	using DSoft.Messaging;
	...
	
	var newEvent = new CoreMessageBusEvent()
	{
		EventId = "1234",
		Sender = this,
		Data = new object[]{"This is a message"},
	};
	
	MessageBus.Default.Post(newEvent);
	 