using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DSoft.MessageBus
{
    public class LogListernersCollection : Collection<ILogListener>
    {
        public void Register(ILogListener instance)
        {
            if (this.Contains(instance))
                return;

            if (instance.Channels == null || instance.Channels.Count() == 0)
                throw new Exception($"Cannot register {instance.GetType().FullName} as an ILogListener as it has no channels to listen too");

            this.Add(instance);
        }

        public IEnumerable<ILogListener> FindAll(string channelName)
        {
            var results = from item in this.Items
                          where item.Channels.Contains(channelName, StringComparer.OrdinalIgnoreCase)
                          select item;

            
            return results;
        }
    }
}
