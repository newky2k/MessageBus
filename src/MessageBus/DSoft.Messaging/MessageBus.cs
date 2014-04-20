using System;
using DSoft.Messaging.Collections;

namespace DSoft.Messaging
{
	/// <summary>
	/// Message bus.
	/// </summary>
	public static class MessageBus
	{
		#region Fields

		private static MessageBusEventHandlerCollection mEventHandlers;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the registered event handlers.
		/// </summary>
		/// <value>The event handlers.</value>
		private static MessageBusEventHandlerCollection EventHandlers {
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
		public static void Register (MessageBusEventHandler EventHandler)
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
		public static void DeRegister (MessageBusEventHandler EventHandler)
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
		public static void Clear (string EventID)
		{
			foreach (var item in EventHandlers.HandlersForEvent(EventID))
			{
				EventHandlers.Remove (item);
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Posts the event.
		/// </summary>
		/// <param name="Event">The event.</param>
		public static void PostEvent (MessageBusEvent Event)
		{
			//find all the registered handlers
			foreach (var item in EventHandlers.HandlersForEvent(Event.EventId))
			{
				if (item.EventAction != null)
				{
					item.EventAction (Event.Sender, Event.Data);
				}
			}
		}

		#endregion

		#endregion
	}
}

