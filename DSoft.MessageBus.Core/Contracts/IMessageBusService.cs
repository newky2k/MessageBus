using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.MessageBus.Contracts
{
    /// <summary>
    /// Interface for MessageBusService
    /// </summary>
    public interface IMessageBusService
    {
        #region Post

        /// <summary>
        /// Post the specified Event to the Default MessageBus
        /// </summary>
        /// <param name="busEvent">Event.</param>
        public void Post(MessageBusEvent busEvent);

        /// <summary>
        /// Posts the event to the Default MessageBus
        /// </summary>
        /// <param name="eventId">Event identifier.</param>
        public void Post(string eventId);

        /// <summary>
        /// Post the specified EventId and Sender to the Default MessageBus
        /// </summary>
        /// <param name="eventId">Event identifier.</param>
        /// <param name="Sender">Sender.</param>
        public void Post(string eventId, object Sender);

        /// <summary>
        /// Post the specified EventId, Sender and Data to the Default MessageBus
        /// </summary>
        /// <param name="eventId">Event identifier.</param>
        /// <param name="Sender">Sender.</param>
        /// <param name="Data">Data.</param>
        public void Post(string eventId, object Sender, params object[] Data);

        /// <summary>
        /// Post the specified EventId and Data packet to the Default MessageBus
        /// </summary>
        /// <param name="eventId">Event identifier.</param>
        /// <param name="Data">Data.</param>
        public void Post(string eventId, params object[] Data);

        #endregion

        #region Unsubscribe

        /// <summary>
        /// Unsubscribe a previously registered handler for the specified event id
        /// </summary>
        /// <param name="eventId">Event identifier.</param>
        /// <param name="action">Handler action</param>
        public void Unsubscribe(string eventId, Action<object, MessageBusEvent> action);

        /// <summary>
        /// Unsubscribe a previously registered event handler
        /// </summary>
        /// <param name="EventHandler">The event handler instance</param>
        public void Unsubscribe(MessageBusEventHandler EventHandler);


        /// <summary>
        /// Unsubscribe the event action for a Generic message bus type
        /// </summary>
        /// <typeparam name="T">Type of MessageBusEvent to unsubscribe from</typeparam>
        /// <param name="Action">The action to remove</param>
        public void Unsubscribe<T>(Action<object, MessageBusEvent> Action) where T : MessageBusEvent;

        /// <summary>
        /// Unsubscribes all handlers for the specified event id
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        public void Unsubscribe(string eventId);

        #endregion

        #region Subscribe
        /// <summary>
        /// Subscribe to the event with an parameterless action
        /// </summary>
        /// <param name="evenId">Event Id</param>
        /// <param name="action">Action to execute on the event occuring</param>
        public void Subscribe(string evenId, Action action);

        /// <summary>
        /// Subscribe to the event with an action that recieve the data object from the event
        /// </summary>
        /// <param name="evenId">Event Id</param>
        /// <param name="action">Action to execute on the event occuring</param>
        public void Subscribe(string evenId, Action<object[]> action);

        /// <summary>
        /// Subscribe to the event with an action
        /// </summary>
        /// <param name="eventId">Event Id</param>
        /// <param name="action">Action to execute on the event occuring</param>
        public void Subscribe(string eventId, Action<object, MessageBusEvent> action);

        /// <summary>
        /// Subscribe to the specified event handler.
        /// </summary>
        /// <param name="EventHandler">The event handler.</param>
        public void Subscribe(MessageBusEventHandler EventHandler);
        /// <summary>
        /// Subscribe for notifications of a specific a type of MessageBusEvent
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public void Subscribe<T>(Action<object, MessageBusEvent> Action) where T : MessageBusEvent, new();

        #endregion

        #region Logging

        /// <summary>
        /// Send out a log message
        /// </summary>
        /// <param name="title">The title of the log entry</param>
        /// <param name="message">The message.</param>
        /// <param name="severity">The severity.</param>
        public void Log(string title, string? message = null, LogSeverity severity = LogSeverity.Notification);

        /// <summary>
        ///Send out a log message to the specified channel only
        /// </summary>
        /// <param name="channelName">Name of the channel.</param>
        /// <param name="title">The title of the log entry</param>
        /// <param name="message">The message.</param>
        /// <param name="severity">The severity.</param>
        public void Log(string channelName, string title, string? message = null, LogSeverity severity = LogSeverity.Notification);

        /// <summary>
        /// Register a class instance that implements ILogListener to listen for log messages
        /// </summary>
        /// <param name="instance">The instance of the class</param>
        public void Listen(ILogListener instance);

        /// <summary>
        /// Stops listening to the log channel
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void StopListening(ILogListener instance);
        #endregion
    }
}
