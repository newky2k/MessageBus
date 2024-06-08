
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace DSoft.MessageBus
{
	/// <summary>
	/// Message bus.
	/// </summary>
	public partial class MessageBus
	{
        /// <summary>
        /// Execute Post action on a seperate task using Task.Run
        /// </summary>
        public static bool RunPostOnSeperateTask = false;

		#region Fields
		private static Lazy<MessageBusEventHandlerCollection> _eventHandlers = new Lazy<MessageBusEventHandlerCollection>(() => new MessageBusEventHandlerCollection());
		private static Lazy<LogListernersCollection> _logListeners = new Lazy<LogListernersCollection>(() => new LogListernersCollection());
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
		private static MessageBusEventHandlerCollection EventHandlers => _eventHandlers.Value;

        /// <summary>
        /// Gets the register log listeners
        /// </summary>
        /// <value>
        /// The current log listeners
        /// </value>
        private static LogListernersCollection LogListeners => _logListeners.Value;
		#endregion

		#region Methods

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

		private static void PostInternal(MessageBusEvent busEvent)
        {
			if (!(busEvent is CoreMessageBusEvent))
			{
				foreach (var item in EventHandlers.HandlersForEvent(busEvent.GetType()))
				{
					if (item.EventAction != null)
					{
						Execute(item.EventAction, busEvent.Sender, busEvent);
					}
				}
			}

			//find all the registered handlers
			foreach (var item in EventHandlers.HandlersForEvent(busEvent.EventId))
			{
				if (item.EventAction != null)
				{
					Execute(item.EventAction, busEvent.Sender, busEvent);
				}
			}
		}

		/// <summary>
		/// Post the specified Event to the Default MessageBus
		/// </summary>
		/// <param name="busEvent">Event.</param>
		public static void Post (MessageBusEvent busEvent)
		{
			if (RunPostOnSeperateTask == true)
            {
				Task.Run(() =>
				{
					PostInternal(busEvent);
				});
			}
			else
            {
				PostInternal(busEvent);
			}


        }

		/// <summary>
		/// Posts the event to the Default MessageBus
		/// </summary>
		/// <param name="eventId">Event identifier.</param>
		public static void Post (string eventId)
		{
			Post (eventId, null);
		}

		/// <summary>
		/// Post the specified EventId and Sender to the Default MessageBus
		/// </summary>
		/// <param name="eventId">Event identifier.</param>
		/// <param name="Sender">Sender.</param>
		public static void Post (string eventId, object Sender)
		{
			Post(eventId, Sender, null);
		}
		
		/// <summary>
		/// Post the specified EventId, Sender and Data to the Default MessageBus
		/// </summary>
		/// <param name="eventId">Event identifier.</param>
		/// <param name="Sender">Sender.</param>
		/// <param name="Data">Data.</param>
		public static void Post (string eventId, object Sender, params object[] Data)
		{
			var aEvent = new CoreMessageBusEvent(eventId)
			{
				Sender = Sender,
				Data = Data,
			};

			Post(aEvent);
		}

        /// <summary>
        /// Post the specified EventId and Data packet to the Default MessageBus
        /// </summary>
        /// <param name="eventId">Event identifier.</param>
        /// <param name="Data">Data.</param>
        public static void Post(string eventId, params object[] Data)
        {
			Post(eventId, null, Data);
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
		/// <param name="EventHandler">The event handler instance</param>
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
		/// <typeparam name="T">Type of MessageBusEvent to unsubscribe from</typeparam>
		/// <param name="Action">The action to remove</param>
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

		/// <summary>
		/// Unsubscribes all handlers for the specified event id
		/// </summary>
		/// <param name="eventId">Event identifier</param>
		public static void Unsubscribe(string eventId)
		{
			foreach (var item in FindHandlersForEvent(eventId))
			{
				EventHandlers.Remove(item);
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

        #region Logging

        /// <summary>
        /// Send out a log message
        /// </summary>
        /// <param name="title">The title of the log entry</param>
        /// <param name="message">The message.</param>
        /// <param name="severity">The severity.</param>
        public static void Log(string title, string message = null, LogSeverity severity = LogSeverity.Notification) => Log(Channels.All, title, message);

		/// <summary>
		///Send out a log message to the specified channel only
		/// </summary>
		/// <param name="channelName">Name of the channel.</param>
		/// <param name="title">The title of the log entry</param>
		/// <param name="message">The message.</param>
		/// <param name="severity">The severity.</param>
		public static void Log(string channelName, string title, string message = null, LogSeverity severity = LogSeverity.Notification)
        {

			if (RunPostOnSeperateTask == true)
			{
				Task.Run(() =>
				{
					LogInternal(channelName, title, message, severity);

				});
			}
			else
			{
				LogInternal(channelName, title, message, severity);
			}

		}

		private static void LogInternal(string channelName, string title, string message = null, LogSeverity severity = LogSeverity.Notification)
        {
			var newLog = new LogEvent()
			{
				Channel = channelName,
				Title = title,
				Message = message,
				Severity = severity,
			};

			var listeners = LogListeners.FindAll(channelName);

			foreach (var listener in listeners)
				listener.OnMessageRecieved(newLog);
		}


		/// <summary>
		/// Register a class instance that implements ILogListener to listen for log messages
		/// </summary>
		/// <param name="instance">The instance of the class</param>
		public static void Listen(ILogListener instance)
		{
			LogListeners.Register(instance);

		}

        /// <summary>
        /// Stops listening to the log channel
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static void StopListening(ILogListener instance)
        {
			if (LogListeners == null)
				return;

			LogListeners.Remove(instance);
        }
		#endregion

		#endregion

		#endregion
	}
}

