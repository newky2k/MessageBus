using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace DSoft.MessageBus
{
	/// <summary>
	/// Collection of messagebuseventhandlers
	/// </summary>
	public class MessageBusEventHandlerCollection : Collection<MessageBusEventHandler>
	{
		#region Fields

		private readonly object _syncRoot = new object ();

		#endregion

		#region Properties

		/// <summary>
		/// Lock object guarding access to this collection. Callers performing
		/// compound (check-then-act) operations should lock on this.
		/// </summary>
		public object SyncRoot => _syncRoot;

		#endregion

		#region Methods

		/// <summary>
		/// Handlers for event.
		/// </summary>
		/// <param name="EventId">The event identifier.</param>
		/// <returns></returns>
		public MessageBusEventHandler[] HandlersForEvent (String EventId)
		{
			List<MessageBusEventHandler> results = null;

			lock (_syncRoot)
			{
				foreach (var item in this.Items)
				{
					if (item.EventAction == null || String.IsNullOrWhiteSpace (item.EventId))
						continue;

					if (!String.Equals (item.EventId, EventId, StringComparison.OrdinalIgnoreCase))
						continue;

					(results ??= new List<MessageBusEventHandler> ()).Add (item);
				}
			}

			return results == null ? Array.Empty<MessageBusEventHandler> () : results.ToArray ();
		}

		/// <summary>
		/// Handlerses for event type
		/// </summary>
		/// <returns>The for event.</returns>
		/// <param name="EventType">Event type.</param>
		public MessageBusEventHandler[] HandlersForEvent (Type EventType)
		{
			List<MessageBusEventHandler> results = null;

			lock (_syncRoot)
			{
				foreach (var item in this.Items)
				{
					if (item.EventAction == null || !(item is TypedMessageBusEventHandler typed))
						continue;

					if (typed.EventType == null || !typed.EventType.Equals (EventType))
						continue;

					(results ??= new List<MessageBusEventHandler> ()).Add (typed);
				}
			}

			return results == null ? Array.Empty<MessageBusEventHandler> () : results.ToArray ();
		}

		/// <summary>
		/// Returns the event handlers for the specified Generic MessageBusEvent Type 
		/// </summary>
		/// <returns>The for event.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public MessageBusEventHandler[] HandlersForEvent<T> () where T : MessageBusEvent
		{
			return HandlersForEvent (typeof(T));
		}

		#endregion
	}
}

