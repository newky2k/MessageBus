using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.MessageBus
{
    /// <summary>
    /// ILogListener Interface
    /// </summary>
    public interface ILogListener
    {
        /// <summary>
        /// Gets the channels.
        /// </summary>
        /// <value>The channels.</value>
        IEnumerable<string> Channels { get; }

        /// <summary>
        /// Called when [message recieved].
        /// </summary>
        /// <param name="message">The message.</param>
        void OnMessageRecieved(LogEvent message);
    }
}
