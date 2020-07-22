using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.MessageBus
{
    public interface ILogListener
    {
        IEnumerable<string> Channels { get; }

        void OnMessageRecieved(LogEvent message);
    }
}
