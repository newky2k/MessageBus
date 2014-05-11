
MessageBus is a cross platform EventBus system similar to NSNoticationCenter on iOS and otto on Android that allow you to decouple your code, whilst still allowing your applications components to commincate with each other.  MesssageBus can be used instead of events, can be used to communicate between objects that are not directly linked.

# Features

* Cross platform  
  * Works on iOS, Android, Windows Phone, Mac and Windows  
* Small footprint
* Simple API
* Create custom events to easily pass addtional data
* Allows you to decouple objects and classes within your projects  
* Portable Class library


## Usage

`MessageBus.Default` provides a central bus for you to subsribe to and post events.  You can override this, set a new one of have muliple seperate `MessageBus` objects


	using DSoft.Messaging;
	...
	
	var newBus = new MessageBus();
	 

To subscribe to an event you can create a `MessageBusEventHandler` object, and then register is with the MessageBus.  The event handler allows you to set the Id of the event to subscribe to an a Action or Delegate to call be when the event occurs.

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

To post an event you create an instance of `MessageBusEvent`, set the EventId, sender and passing any addtional data you want to send
 
	using DSoft.Messaging;
	...
	
	var newBus = new MessageBus();
	 