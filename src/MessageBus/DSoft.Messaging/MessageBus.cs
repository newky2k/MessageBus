using System;
using DSoft.Messaging.Collections;
using System.Threading.Tasks;

namespace DSoft.Messaging
{
	/// <summary>
	/// Message bus.
	/// </summary>
	public class MessageBus
	{

		#region Fields

		private static MessageBus mDefault;
		private MessageBusEventHandlerCollection mEventHandlers;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the default message bus
		/// </summary>
		/// <value>The default.</value>
		public static MessageBus Default
		{
			get 
			{
				if (mDefault == null) 
				{
					mDefault = new MessageBus ();
				}

				return mDefault;
			}
		}

		/// <summary>
		/// Gets the registered event handlers.
		/// </summary>
		/// <value>The event handlers.</value>
		internal MessageBusEventHandlerCollection EventHandlers {
			get
			{
				if (mEventHandlers == null)
				{
					mEventHandlers = new MessageBusEventHandlerCollection ();
				}

				return mEventHandlers;
			}
		}

		#endregion

		#region Methods

		#region Registration

		/// <summary>
		/// Registers the specified event handler.
		/// </summary>
		/// <param name="EventHandler">The event handler.</param>
		public void Register (MessageBusEventHandler EventHandler)
		{
			if (EventHandler == null)
				return;

			if (!EventHandlers.Contains (EventHandler))
			{
				EventHandlers.Add (EventHandler);
			}
		}

		/// <summary>
		/// DeRegister the event handler
		/// </summary>
		/// <param name="EventHandler">The event handler.</param>
		public void DeRegister (MessageBusEventHandler EventHandler)
		{
			if (EventHandlers.Contains (EventHandler))
			{
				EventHandlers.Remove (EventHandler);
			}
		}

		/// <summary>
		/// Clear Handlers for the specified event id
		/// </summary>
		/// <param name="EventID">Event I.</param>
		public void Clear (string EventID)
		{
			foreach (var item in EventHandlers.HandlersForEvent(EventID))
			{
				EventHandlers.Remove (item);
			}
		}

		#endregion

		#region Generic Methods
		/// <summary>
		/// Register for a type of MessageBusEvent
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void Register<T>(Action<object,MessageBusEvent> Action) where T : MessageBusEvent,new()
		{
			var aType = typeof(T);

			var typeHandler = new TypedMessageBusEventHandler () {
				EventType = aType,
				EventAction = Action,
			};

			EventHandlers.Add (typeHandler);
		}

	
		#endregion
		#region Events

		/// <summary>
		/// Posts the event.
		/// </summary>
		/// <param name="Event">The event.</param>
		public void PostEvent (MessageBusEvent Event)
		{
			if (!(Event is CoreMessageBusEvent))
			{
				foreach (var item in EventHandlers.HandlersForEvent(Event.GetType()))
				{
					if (item.EventAction != null)
					{
						item.EventAction (Event.Sender, Event);
					}
				}
			}

			//find all the registered handlers
			foreach (var item in EventHandlers.HandlersForEvent(Event.EventId))
			{
				if (item.EventAction != null)
				{
					item.EventAction (Event.Sender, Event);
				}
			}

		}

		#endregion

		#endregion
	}
}

