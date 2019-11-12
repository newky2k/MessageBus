
using System;
using DSoft.Messaging.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace DSoft.Messaging
{
	/// <summary>
	/// Message bus.
	/// </summary>
	public class MessageBus
	{
		#region Fields

		private static volatile MessageBus mDefault;
		private static object syncRoot = new object();
		private MessageBusEventHandlerCollection mEventHandlers;

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
		/// Gets the default message bus
		/// </summary>
		/// <value>The default.</value>
		public static MessageBus Default {
			get
			{
				if (mDefault == null)
				{
					lock (syncRoot) 
					{
						if (mDefault == null)
							mDefault = new MessageBus ();
					}

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
        /// Register for an eventId
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="action"></param>
        public void Register (string eventId, Action<object, MessageBusEvent> action)
        {
            Register(new MessageBusEventHandler(eventId, action));
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
        /// Deregister a specific action for an event
        /// </summary>
        /// <param name="eventId">the event id</param>
        /// <param name="action">the action</param>
        public void DeRegister(string eventId, Action<object, MessageBusEvent> action)
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

		#region Generic Methods

		/// <summary>
		/// Register for a type of MessageBusEvent
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void Register<T> (Action<object,MessageBusEvent> Action) where T : MessageBusEvent, new()
		{
			var aType = typeof(T);

			var typeHandler = new TypedMessageBusEventHandler () {
				EventType = aType,
				EventAction = Action,
			};

			EventHandlers.Add (typeHandler);
		}

		/// <summary>
		/// Deregister the event action for a Generic message bus type
		/// </summary>
		/// <param name="Action">Action.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void DeRegister<T> (Action<object,MessageBusEvent> Action) where T : MessageBusEvent
		{
			var results = new List<MessageBusEventHandler> (EventHandlers.HandlersForEvent<T> ());

			foreach (var item in results)
			{
				if (item.EventAction == Action)
				{
					EventHandlers.Remove (item);
				}
			}
		}

		#endregion

		#region Posting

		private void Execute (Action<object,MessageBusEvent> Action, object Sender, MessageBusEvent Evnt)
		{
            Action(Sender, Evnt);
		}

		/// <summary>
		/// Posts the even
		/// </summary>
		/// <param name="Event">The event object</param>
		public void Post (MessageBusEvent Event)
		{
			if (!(Event is CoreMessageBusEvent))
			{
				foreach (var item in EventHandlers.HandlersForEvent(Event.GetType()))
				{
					if (item.EventAction != null)
					{
						Execute (item.EventAction, Event.Sender, Event);
					}
				}
			}

			//find all the registered handlers
			foreach (var item in EventHandlers.HandlersForEvent(Event.EventId))
			{
				if (item.EventAction != null)
				{
					Execute (item.EventAction, Event.Sender, Event);
				}
			}

		}

		/// <summary>
		/// Posts the event.
		/// </summary>
		/// <param name="EventId">Event Id</param>
		public void Post (String EventId)
		{
			Post (EventId, null);
		}

		/// <summary>
		/// Posts the event
		/// </summary>
		/// <param name="EventId">Event Id</param>
		/// <param name="Sender">The Sender</param>
		public void Post (String EventId, object Sender)
		{
			Post (EventId, null, null);
		}

		/// <summary>
		/// Posts the event.
		/// </summary>
		/// <param name="EventId">Event Id</param>
		/// <param name="Sender">The Sender</param>
		/// <param name="Data">Data objects to pass through with the event </param>
		public void Post (String EventId, object Sender, object[] Data)
		{
			var aEvent = new CoreMessageBusEvent (EventId) {
				Sender = Sender,
				Data = Data,
			};

			Post (aEvent);
		}

        #endregion

        #region Private Methods

        private IEnumerable<MessageBusEventHandler> FindHandlersForEvent(string eventId)
        {
            if (string.IsNullOrWhiteSpace(eventId))
                throw new Exception("EventId cannot be null or blank");

            var results = EventHandlers.HandlersForEvent(eventId);

            return results;
        }
        #endregion

        #endregion

        #region Static Methods

        /// <summary>
        /// Post the specified Event to the Default MessageBus
        /// </summary>
        /// <param name="Event">Event.</param>
        public static void PostEvent (MessageBusEvent Event)
		{
			Default.Post (Event);
		}

		/// <summary>
		/// Posts the event to the Default MessageBus
		/// </summary>
		/// <param name="EventId">Event identifier.</param>
		public static void PostEvent (String EventId)
		{
			Default.Post (EventId);
		}

		/// <summary>
		/// Post the specified EventId and Sender to the Default MessageBus
		/// </summary>
		/// <param name="EventId">Event identifier.</param>
		/// <param name="Sender">Sender.</param>
		public static void PostEvent (String EventId, object Sender)
		{
			Default.Post (EventId, Sender);
		}

		/// <summary>
		/// Post the specified EventId, Sender and Data to the Default MessageBus
		/// </summary>
		/// <param name="EventId">Event identifier.</param>
		/// <param name="Sender">Sender.</param>
		/// <param name="Data">Data.</param>
		public static void PostEvent (String EventId, object Sender, object[] Data)
		{
			Default.Post (EventId, Sender, Data);
		}

        /// <summary>
        /// Register a handler for the specified event id
        /// </summary>
        /// <param name="eventId">Event identifier.</param>
        /// <param name="action">Handler action</param>
        public static void RegisterHandler(string eventId, Action<object, MessageBusEvent> action)
        {
            Default.Register(eventId, action);
        }

        /// <summary>
        /// Deregisters a previously register handler for the specified event id
        /// </summary>
        /// <param name="eventId">Event identifier.</param>
        /// <param name="action">Handler action</param>
        public static void DeRegisterHandler(string eventId, Action<object, MessageBusEvent> action)
        {
            Default.DeRegister(eventId, action);
        }

        /// <summary>
        /// Execute the action on the UI thread
        /// </summary>
        /// <param name="Command">Command.</param>
        public void RunOnUiThread(Action Command)
		{
            if (SyncContext == null)
                throw new ArgumentNullException ("SyncContext");

            //update the UI
			Task.Factory.StartNew(() =>
			{
				if (Command != null)
				{
					Command();
				}
			}, CancellationToken.None, TaskCreationOptions.None, SyncContext);
		}
		#endregion
	}
}

