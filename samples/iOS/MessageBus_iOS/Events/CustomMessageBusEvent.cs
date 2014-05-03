using System;
using DSoft.Messaging;

namespace MessageBus_iOS
{
	/// <summary>
	/// Custom message bus event.
	/// </summary>
	public class CustomMessageBusEvent : MessageBusEvent
	{
		public const string kCustEventId = "445566";

		#region Properties

		/// <summary>
		/// Gets or sets the event identifier.
		/// </summary>
		/// <value>The event identifier.</value>
		public override string EventId {
			get 
			{
				//return a constant eventId
				return kCustEventId;
			}
		}

		/// <summary>
		/// Custom property on the event that Gets or sets the timestamp 
		/// </summary>
		/// <value>The time stamp.</value>
		public DateTime TimeStamp { get; set;}
		#endregion

		#region Constructores
		/// <summary>
		/// Initializes a new instance of the <see cref="MessageBus_iOS.CustomMessageBusEvent"/> class.
		/// </summary>
		public CustomMessageBusEvent ()
		{
			//
			TimeStamp = DateTime.Now;
		}
		#endregion
	}
}

