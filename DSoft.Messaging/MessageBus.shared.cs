
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace DSoft.MessageBus
{
	/// <summary>
	/// Static Wrapper for MessageBusService
	/// </summary>
	public static class MessageBus
	{
		#region Fields
		private static Lazy<MessageBusService> _service = new Lazy<MessageBusService>(() => new MessageBusService());
        #endregion

        #region Properties

        /// <summary>
        /// Gets the internal instance of MessageBusService
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        private static MessageBusService Service => _service.Value;

        /// <summary>
        /// Execute Post action on a seperate task using Task.Run
        /// </summary>
        public static bool RunPostOnSeperateTask
		{
			get => Service.RunPostOnSeperateTask;
			set => Service.RunPostOnSeperateTask = value;
		}

        #endregion

        #region Static Methods

        #region Post

        /// <summary>
        /// Post the specified Event to the Default MessageBus
        /// </summary>
        /// <param name="busEvent">Event.</param>
        public static void Post (MessageBusEvent busEvent) => Service.Post(busEvent);

		/// <summary>
		/// Posts the event to the Default MessageBus
		/// </summary>
		/// <param name="eventId">Event identifier.</param>
		public static void Post (string eventId) => Service.Post(eventId);

		/// <summary>
		/// Post the specified EventId and Sender to the Default MessageBus
		/// </summary>
		/// <param name="eventId">Event identifier.</param>
		/// <param name="Sender">Sender.</param>
		public static void Post (string eventId, object Sender) => Service.Post(eventId, Sender);
		
		/// <summary>
		/// Post the specified EventId, Sender and Data to the Default MessageBus
		/// </summary>
		/// <param name="eventId">Event identifier.</param>
		/// <param name="Sender">Sender.</param>
		/// <param name="Data">Data.</param>
		public static void Post (string eventId, object Sender, params object[] Data) => Service.Post(eventId, Sender, Data);

		/// <summary>
		/// Post the specified EventId and Data packet to the Default MessageBus
		/// </summary>
		/// <param name="eventId">Event identifier.</param>
		/// <param name="Data">Data.</param>
		public static void Post(string eventId, params object[] Data) => Service.Post(eventId, Data);

		#endregion

		#region Unsubscribe

		/// <summary>
		/// Unsubscribe a previously registered handler for the specified event id
		/// </summary>
		/// <param name="eventId">Event identifier.</param>
		/// <param name="action">Handler action</param>
		public static void Unsubscribe(string eventId, Action<object, MessageBusEvent> action) => Service.Unsubscribe(eventId, action);

		/// <summary>
		/// Unsubscribe a previously registered event handler
		/// </summary>
		/// <param name="EventHandler">The event handler instance</param>
		public static void Unsubscribe(MessageBusEventHandler EventHandler) => Service.Unsubscribe(EventHandler);


		/// <summary>
		/// Unsubscribe the event action for a Generic message bus type
		/// </summary>
		/// <typeparam name="T">Type of MessageBusEvent to unsubscribe from</typeparam>
		/// <param name="Action">The action to remove</param>
		public static void Unsubscribe<T>(Action<object, MessageBusEvent> Action) where T : MessageBusEvent => Service.Unsubscribe<T>(Action);

		/// <summary>
		/// Unsubscribes all handlers for the specified event id
		/// </summary>
		/// <param name="eventId">Event identifier</param>
		public static void Unsubscribe(string eventId) => Unsubscribe(eventId);

		#endregion

		#region Subscribe
		/// <summary>
		/// Subscribe to the event with an parameterless action
		/// </summary>
		/// <param name="evenId">Event Id</param>
		/// <param name="action">Action to execute on the event occuring</param>
		public static void Subscribe(string evenId, Action action) => Service.Subscribe(evenId, action);

		/// <summary>
		/// Subscribe to the event with an action that recieve the data object from the event
		/// </summary>
		/// <param name="evenId">Event Id</param>
		/// <param name="action">Action to execute on the event occuring</param>
		public static void Subscribe(string evenId, Action<object[]> action) => Service.Subscribe(evenId, action);

		/// <summary>
		/// Subscribe to the event with an action
		/// </summary>
		/// <param name="eventId">Event Id</param>
		/// <param name="action">Action to execute on the event occuring</param>
		public static void Subscribe(string eventId, Action<object, MessageBusEvent> action) => Service.Subscribe(eventId, action);	

		/// <summary>
		/// Subscribe to the specified event handler.
		/// </summary>
		/// <param name="EventHandler">The event handler.</param>
		public static void Subscribe(MessageBusEventHandler EventHandler) => Service.Subscribe(EventHandler);
		/// <summary>
		/// Subscribe for notifications of a specific a type of MessageBusEvent
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Subscribe<T>(Action<object, MessageBusEvent> Action) where T : MessageBusEvent, new() => Service.Subscribe<T>(Action);

        #endregion

        #region Logging

        /// <summary>
        /// Send out a log message
        /// </summary>
        /// <param name="title">The title of the log entry</param>
        /// <param name="message">The message.</param>
        /// <param name="severity">The severity.</param>
        public static void Log(string title, string message = null, LogSeverity severity = LogSeverity.Notification) => Service.Log(Channels.All, title, message);

		/// <summary>
		///Send out a log message to the specified channel only
		/// </summary>
		/// <param name="channelName">Name of the channel.</param>
		/// <param name="title">The title of the log entry</param>
		/// <param name="message">The message.</param>
		/// <param name="severity">The severity.</param>
		public static void Log(string channelName, string title, string message = null, LogSeverity severity = LogSeverity.Notification) => Service.Log(channelName, title, message, severity);

		/// <summary>
		/// Register a class instance that implements ILogListener to listen for log messages
		/// </summary>
		/// <param name="instance">The instance of the class</param>
		public static void Listen(ILogListener instance) => Service.Listen(instance);

        /// <summary>
        /// Stops listening to the log channel
        /// </summary>
        /// <param name="instance">The instance.</param>
        public static void StopListening(ILogListener instance) => Service.StopListening(instance);
		#endregion

		#endregion

	}
}

