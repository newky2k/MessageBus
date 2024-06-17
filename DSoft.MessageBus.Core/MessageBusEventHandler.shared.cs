﻿using System;

namespace DSoft.MessageBus
{
	/// <summary>
	/// Message bus event handler.
	/// </summary>
	public class MessageBusEventHandler
	{
		#region Properties

		/// <summary>
		/// Event Id
		/// </summary>
		public string EventId { get; set; }

		/// <summary>
		/// Action to perform on event
		/// </summary>
		public Action<object, MessageBusEvent> EventAction { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DSoft.MessageBus.MessageBusEventHandler"/> class.
        /// </summary>
        public MessageBusEventHandler ()
		{
			EventId = string.Empty;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="DSoft.MessageBus.MessageBusEventHandler"/> class.
        /// </summary>
        /// <param name="EventId">Event identifier.</param>
        /// <param name="Action">Action.</param>
        public MessageBusEventHandler (string EventId, Action<object, MessageBusEvent> Action)
		{
			this.EventId = EventId;
			this.EventAction = Action;
		}

		#endregion
	}

	/// <summary>
	/// Typed message bus event handler.
	/// </summary>
	public class TypedMessageBusEventHandler : MessageBusEventHandler
	{
		#region Properties

		/// <summary>
		/// Gets or sets the type of the event.
		/// </summary>
		/// <value>The type of the event.</value>
		public Type EventType { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DSoft.MessageBus.TypedMessageBusEventHandler"/> class.
        /// </summary>
        public TypedMessageBusEventHandler ()
		{
			
		}

		#endregion
	}
}

