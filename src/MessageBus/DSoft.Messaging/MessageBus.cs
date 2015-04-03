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
		private static object syncRoot = new Object();
		private MessageBusEventHandlerCollection mEventHandlers;

        private Dictionary<string, MessageBusEvent> mStickyEvents;

		#endregion

		#region Constructors

		public MessageBus()
		{
			SyncContext = TaskScheduler.FromCurrentSynchronizationContext();

            mStickyEvents = new Dictionary<string, MessageBusEvent>();
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
		/// Gets or sets the sync context.
		/// </summary>
		/// <value>The sync context.</value>
		private TaskScheduler SyncContext
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
        /// Registers the sticky event handler. If there is any sticky event waiting, it will be posted immediately.
        /// </summary>
        /// <param name="EventHandler">The event handler.</param>
        public void RegisterSticky(MessageBusEventHandler EventHandler)
        {
            Register(EventHandler);

            postStickyAfterRegistation(EventHandler);
        }

        void postStickyAfterRegistation(MessageBusEventHandler EventHandler)
        {
            MessageBusEvent eventToBePosted = null;

            lock (mStickyEvents)
            {
                if (mStickyEvents.ContainsKey(EventHandler.EventId))
                {
                    // consider using this:
                    // Execute(EventHandler.EventAction, this, mStickyEvents[EventHandler.EventId]);

                    eventToBePosted = mStickyEvents[EventHandler.EventId];
                }
            }

            if (eventToBePosted != null)
            {
                EventHandler.EventAction(this, eventToBePosted);
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
        /// Register for a type of MessageBusEvent. If there is any sticky event waiting, it will be posted immediately 
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void RegisterSticky<T>(Action<object, MessageBusEvent> Action) where T : MessageBusEvent, new()
        {
            var aType = typeof(T);

            var typeHandler = new TypedMessageBusEventHandler()
            {
                EventType = aType,
                EventAction = Action,
            };

            EventHandlers.Add(typeHandler);

            postStickyAfterRegistation(typeHandler);
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
		    Action (Sender, Evnt);
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

        /// <summary>
        /// Posts the sticky event.
        /// </summary>
        /// <param name="Event">The event object</param>
        public void PostSticky(MessageBusEvent Event)
        {
            lock (mStickyEvents)
            {
                mStickyEvents.Add(Event.EventId, Event);
            }

            Post(Event);
        }

        /// <summary>
        /// Posts the sticky event.
        /// </summary>
        /// <param name="EventId">Event Id</param>
        public void PostSticky(String EventId)
        {
            PostSticky(EventId, null);
        }

        /// <summary>
        /// Posts the sticky event
        /// </summary>
        /// <param name="EventId">Event Id</param>
        /// <param name="Sender">The Sender</param>
        public void PostSticky(String EventId, object Sender)
        {
            PostSticky(EventId, null, null);
        }

        /// <summary>
        /// Posts the sticky event.
        /// </summary>
        /// <param name="EventId">Event Id</param>
        /// <param name="Sender">The Sender</param>
        /// <param name="Data">Data objects to pass through with the event </param>
        public void PostSticky(String EventId, object Sender, object[] Data)
        {
            var aEvent = new CoreMessageBusEvent(EventId)
            {
                Sender = Sender,
                Data = Data,
            };

            PostSticky(aEvent);
        }

		#endregion

        #region Sticky events

        /// <summary>
        /// Removes all stored sticky events
        /// </summary>
        public void RemoveAllStickyEvents()
        {
            lock (mStickyEvents)
            {
                mStickyEvents.Clear();
            }
        }

        /// <summary>
        /// Removes a sticky event of a given ID
        /// </summary>
        /// <param name="eventId">event ID</param>
        public void RemoveStickyEvent(string eventId)
        {
            lock (mStickyEvents)
            {
                mStickyEvents.Remove(eventId);
            }
        }

        /// <summary>
        /// Gets a last stored sticky event.
        /// </summary>
        /// <param name="eventId">Event ID</param>
        /// <returns>Sticky Event</returns>
        public MessageBusEvent GetStickyEvent(string eventId)
        {
            lock (mStickyEvents)
            {
                return mStickyEvents[eventId];
            }
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
		/// Execute the action on the UI thread
		/// </summary>
		/// <param name="Command">Command.</param>
		public void RunOnUiThread(Action Command)
		{
			var cul = Thread.CurrentThread;

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

