using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace DSoft.Messaging.Collections
{
	/// <summary>
	/// Collection of messagebuseventhandlers
	/// </summary>
	public class MessageBusEventHandlerCollection : Collection<MessageBusEventHandler>
	{
		#region Methods

		/// <summary>
		/// Handlers for event.
		/// </summary>
		/// <param name="EventId">The event identifier.</param>
		/// <returns></returns>
		public MessageBusEventHandler[] HandlersForEvent (String EventId)
		{
			var results = from item in this.Items
			              where item.EventId.ToLower ().Equals (EventId.ToLower ())
			              where item.EventAction != null
			              select item;

			return results.ToArray ();
		}
		//		/// <summary>
		//		/// Handlers for event of the specific type
		//		/// </summary>
		//		/// <returns>The for event.</returns>
		//		/// <typeparam name="T">The 1st type parameter.</typeparam>
		//		public MessageBusEventHandler[] HandlersForEvent<T> () where T : MessageBusEvent
		//		{
		//			var results = new List<MessageBusEventHandler> ();
		//
		//			foreach (var item in this.Items)
		//			{
		//				if (item.Event != null)
		//				{
		//					if (item.Event.GetType ().Equals (typeof(T)))
		//					{
		//						results.Add (item);
		//					}
		//				}
		//
		//			}
		//
		//			return results.ToArray ();
		//		}

		#endregion
	}
}

