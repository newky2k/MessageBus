using System;

namespace DSoft.MessageBus
{
	/// <summary>
	/// MessageBus object extensions
	/// </summary>
	public static class MessageBusExtensions
	{
		/// <summary>
		/// Posts the event.
		/// </summary>
		/// <param name="Sender">Sender.</param>
		/// <param name="EventId">Event Id</param>
		public static void PostEvent (this object Sender, string EventId) => Sender.PostEvent(EventId, null);

		/// <summary>
		/// Posts the event.
		/// </summary>
		/// <param name="Sender">Sender.</param>
		/// <param name="EventId">Event Id</param>
		/// <param name="Data">Additonal data</param>
		public static void PostEvent (this object Sender, string EventId, object[] Data) => MessageBus.Post(EventId, Sender, Data);
	}
}

