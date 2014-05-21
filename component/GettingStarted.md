
MessageBus is a cross platform EventBus system similar to NSNoticationCenter on iOS and otto on Android that allows you to decouple your code whilst still allowing your applications components to communicate with each other.  MesssageBus can be used instead of events and can be used to communicate between objects that are not directly linked.

## Usage

`MessageBus.Default` provides a central bus for you to subscribe to and post events.  You can override this by setting a new one or have multiple separate `MessageBus` objects


	using DSoft.Messaging;
	...
	
	var newBus = new MessageBus();
	
	MessageBus.Default = aNewBus;
	 
**Registering Event Handlers**

To subscribe to an event you can create a `MessageBusEventHandler` object and then register it with the MessageBus.  The event handler allows you to set the Id of the event to subscribe to and an Action or Delegate to call be when the event occurs.

	using DSoft.Messaging;
	...
	
	var newEvHandler = new MessageBusEventHandler()
	{
		EventId = "1234",
		Action = (sender, evnt) =>
		{
			//Code goes here
		},
	};
	
	MessageBus.Default.Register(newEvHandler);

You can then deregister an event handler or simply clear all handlers for a specific EventId


	using DSoft.Messaging;
	...

	//deregister
	MessageBus.Default.DeRegister(newEvHandler);
	
	//Clear all event handlers
	MessageBus.Default.Clear("1234");

*Note: You must execute any code that updates the UI, contained within your event action or delegate, on the UI thread*

**Posting Events**

You can then post an event from anywhere in your application using the Id of the event to execute the Action in the registered `MessageBusEventHandler` objects.

To post an event you create an instance of `CoreMessageBusEvent`, set the EventId, sender and any additional data you want to send
 
	using DSoft.Messaging;
	...
	
	var newEvent = new CoreMessageBusEvent()
	{
		EventId = "1234",
		Sender = this,
		Data = new object[]{"This is a message"},
	};
	
	MessageBus.Default.Post(newEvent);

You can also post an event with just the EventId, with the EventId and the sender or EventId, Sender and data, without creating a CoreMessageBusEvent object

	using DSoft.Messaging;
	...
	
	MessageBus.Default.Post("1234");
	MessageBus.Default.Post("1234",this);
	MessageBus.Default.Post("1234",this, new object[]{"This is a message"});
	
If you are only using the default MessageBus you can post using the class methods instead.

	using DSoft.Messaging;
	...
	
	var newEvent = new CoreMessageBusEvent()
	{
		EventId = "1234",
		Sender = this,
		Data = new object[]{"This is a message"},
	};
	
	MessageBus.PostEvent(newEvent);
	MessageBus.PostEvent("1234");
	MessageBus.PostEvent("1234",this);
	MessageBus.PostEvent("1234",this, new object[]{"This is a message"});

*Note: You must execute any code that updates the UI, contained within your event action or delegate, on the UI thread*

**Custom Events**	

You can sub-class `MessageBusEvent` to allow you to pass additional information in the event, without having to pass it in the Data property.  

You can then register for events based on the type of the event, rather than the event Id.


	using DSoft.Messaging;
	...
	
	MessageBus.Default.Register<CustomMessageBusEvent> (CustomMessageEventHandler);

	MessageBus.Default.Register<CustomMessageBusEvent> ((sendr, evnt) => {
    
      	//code goes here
	});

You can also deregister based on type

	MessageBus.Default.DeRegister<CustomMessageBusEvent> (CustomMessageEventHandler);
