using System;

namespace DSoft.Messaging
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
		public String EventId { get; set; }

		/// <summary>
		/// Action to perform on event
		/// </summary>
		public Action<object, object[]> EventAction { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DSoft.Messaging.MessageBusEventHandler"/> class.
		/// </summary>
		public MessageBusEventHandler ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DSoft.Messaging.MessageBusEventHandler"/> class.
		/// </summary>
		/// <param name="EventId">Event identifier.</param>
		/// <param name="Action">Action.</param>
		public MessageBusEventHandler (String EventId, Action<object, object[]> Action)
		{
			this.EventId = EventId;
			this.EventAction = Action;
		}

		#endregion
	}
}

