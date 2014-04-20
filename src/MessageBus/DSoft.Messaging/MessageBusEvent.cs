using System;

namespace DSoft.Messaging
{
	/// <summary>
	/// Message bus event class
	/// </summary>
	public class MessageBusEvent
	{
		#region Fields

		private String mEventID;

		#endregion

		#region Properties

		/// <summary>
		/// Event Id
		/// </summary>
		public String EventId { 
			get
			{
				if (String.IsNullOrWhiteSpace (mEventID))
				{
					mEventID = Guid.NewGuid ().ToString ();
				}

				return mEventID;
			}
			set
			{
				mEventID = value;
			}
		}

		/// <summary>
		/// Sender of the event
		/// </summary>
		public object Sender { get; set; }

		/// <summary>
		/// Data to pass with the event
		/// </summary>
		public object[] Data { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DSoft.Messaging.MessageBusEvent"/> class.
		/// </summary>
		public MessageBusEvent ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DSoft.Messaging.MessageBusEvent"/> class.
		/// </summary>
		/// <param name="Sender">Sender.</param>
		/// <param name="EventID">Event I.</param>
		/// <param name="Data">Data.</param>
		public MessageBusEvent (object Sender, String EventID, object[] Data)
		{
			this.Sender = Sender;
			this.EventId = EventID;
			this.Data = Data;
		}

		#endregion
	}
}

