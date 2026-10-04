using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DSoft.MessageBus
{
    public class LogListernersCollection : Collection<ILogListener>
    {
        private readonly object _syncRoot = new object();

        /// <summary>
        /// Lock object guarding access to this collection.
        /// </summary>
        public object SyncRoot => _syncRoot;

        public void Register(ILogListener instance)
        {
            if (instance.Channels == null || instance.Channels.Count() == 0)
                throw new Exception($"Cannot register {instance.GetType().FullName} as an ILogListener as it has no channels to listen too");

            lock (_syncRoot)
            {
                if (this.Contains(instance))
                    return;

                this.Add(instance);
            }
        }

        public IEnumerable<ILogListener> FindAll(string channelName)
        {
            lock (_syncRoot)
            {
                return this.Items
                    .Where(item => item.Channels.Contains(channelName, StringComparer.OrdinalIgnoreCase))
                    .ToArray();
            }
        }
    }
}
