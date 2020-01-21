
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using DSoft.MessageBus.Collections;

namespace DSoft.MessageBus
{
	/// <summary>
	/// Message bus.
	/// </summary>
	public partial class MessageBus
	{
		#region Fields

		private static MessageBusEventHandlerCollection mEventHandlers;

		#endregion

		#region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
		public MessageBus()
		{
		}

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

		/// <summary>
		/// Gets or sets the sync context for the UI thread.
		/// </summary>
		/// <value>The sync context.</value>
		public TaskScheduler SyncContext
		{
			get;
			set;
		}
		#endregion

		#region Methods

		#region Registration

        /// <summary>
        /// Clear Handlers for the specified event id
        /// </summary>
        /// <param name="eventId">The event id.</param>
        public void Clear (string eventId)
		{
			foreach (var item in FindHandlersForEvent(eventId))
			{
				EventHandlers.Remove (item);
			}
		}

		#endregion

		#region Private Methods

		private static void Execute(Action<object, MessageBusEvent> Action, object Sender, MessageBusEvent Evnt)
		{
			Action(Sender, Evnt);
		}

		private static IEnumerable<MessageBusEventHandler> FindHandlersForEvent(string eventId)
        {
            if (string.IsNullOrWhiteSpace(eventId))
                throw new Exception("EventId cannot be null or blank");

            var results = EventHandlers.HandlersForEvent(eventId);

            return results;
        }
        #endregion

        #region Static Methods

        #region Post

        /// <summary>
        /// Post the specified Event to the Default MessageBus
        /// </summary>
        /// <param name="Event">Event.</param>
        public static void Post (MessageBusEvent Event)
		{
			if (!(Event is CoreMessageBusEvent))
			{
				foreach (var item in EventHandlers.HandlersForEvent(Event.GetType()))
				{
					if (item.EventAction != null)
					{
						Execute(item.EventAction, Event.Sender, Event);
					}
				}
			}

			//find all the registered handlers
			foreach (var item in EventHandlers.HandlersForEvent(Event.EventId))
			{
				if (item.EventAction != null)
				{
					Execute(item.EventAction, Event.Sender, Event);
				}
			}
		}

		/// <summary>
		/// Posts the event to the Default MessageBus
		/// </summary>
		/// <param name="EventId">Event identifier.</param>
		public static void Post (String EventId)
		{
			Post (EventId, null);
		}

		/// <summary>
		/// Post the specified EventId and Sender to the Default MessageBus
		/// </summary>
		/// <param name="EventId">Event identifier.</param>
		/// <param name="Sender">Sender.</param>
		public static void Post (String EventId, object Sender)
		{
			Post(EventId, Sender, null);
		}
		
		/// <summary>
		/// Post the specified EventId, Sender and Data to the Default MessageBus
		/// </summary>
		/// <param name="EventId">Event identifier.</param>
		/// <param name="Sender">Sender.</param>
		/// <param name="Data">Data.</param>
		public static void Post (String EventId, object Sender, params object[] Data)
		{
			var aEvent = new CoreMessageBusEvent(EventId)
			{
				Sender = Sender,
				Data = Data,
			};

			Post(aEvent);
		}

        /// <summary>
        /// Post the specified EventId and Data packet to the Default MessageBus
        /// </summary>
        /// <param name="EventId">Event identifier.</param>
        /// <param name="Data">Data.</param>
        public static void Post(String EventId, params object[] Data)
        {
			Post(EventId, null, Data);
        }
		#endregion

		#region Unsubscribe

		/// <summary>
		/// Unsubscribe a previously registered handler for the specified event id
		/// </summary>
		/// <param name="eventId">Event identifier.</param>
		/// <param name="action">Handler action</param>
		public static void Unsubscribe(string eventId, Action<object, MessageBusEvent> action)
		{
			foreach (var item in FindHandlersForEvent(eventId))
			{
				if (item.EventAction.Equals(action))
				{
					EventHandlers.Remove(item);
				}
			}
		}

		/// <summary>
		/// Unsubscribe a previously registered event handler
		/// </summary>
		/// <param name="EventHandler">The event handler.</param>
		public static void Unsubscribe(MessageBusEventHandler EventHandler)
		{
			if (EventHandlers.Contains(EventHandler))
			{
				EventHandlers.Remove(EventHandler);
			}
		}

		/// <summary>
		/// Unsubscribe the event action for a Generic message bus type
		/// </summary>
		/// <param name="Action">Action.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Unsubscribe<T>(Action<object, MessageBusEvent> Action) where T : MessageBusEvent
		{
			var results = new List<MessageBusEventHandler>(EventHandlers.HandlersForEvent<T>());

			foreach (var item in results)
			{
				if (item.EventAction == Action)
				{
					EventHandlers.Remove(item);
				}
			}
		}
		#endregion

		#region Subscribe
		/// <summary>
		/// Subscribe to the event with an parameterless action
		/// </summary>
		/// <param name="evenId">Event Id</param>
		/// <param name="action">Action to execute on the event occuring</param>
		public static void Subscribe(string evenId, Action action)
        {
			Subscribe(evenId, (obj, evt) =>
			{
				action();

			});
        }

		/// <summary>
		/// Subscribe to the event with an action that recieve the data object from the event
		/// </summary>
		/// <param name="evenId">Event Id</param>
		/// <param name="action">Action to execute on the event occuring</param>
		public static void Subscribe(string evenId, Action<object[]> action)
		{
			Subscribe(evenId, (obj, evt) =>
			{
				action(evt.Data);

			});
		}

		/// <summary>
		/// Subscribe to the event with an action
		/// </summary>
		/// <param name="eventId">Event Id</param>
		/// <param name="action">Action to execute on the event occuring</param>
		public static void Subscribe(string eventId, Action<object, MessageBusEvent> action)
		{
			Subscribe(new MessageBusEventHandler(eventId, action));
		}

		/// <summary>
		/// Subscribe to the specified event handler.
		/// </summary>
		/// <param name="EventHandler">The event handler.</param>
		public static void Subscribe(MessageBusEventHandler EventHandler)
		{
			if (EventHandler == null)
				return;

			if (!EventHandlers.Contains(EventHandler))
			{
				EventHandlers.Add(EventHandler);
			}
		}
		/// <summary>
		/// Subscribe for notifications of a specific a type of MessageBusEvent
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Subscribe<T>(Action<object, MessageBusEvent> Action) where T : MessageBusEvent, new()
		{
			var aType = typeof(T);

			var typeHandler = new TypedMessageBusEventHandler()
			{
				EventType = aType,
				EventAction = Action,
			};

			Subscribe(typeHandler);


		}
		#endregion
		#endregion





		#endregion
	}
}

