using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.MessageBus
{
    /// <summary>
    /// LogBroadcastMode Enum 
    /// </summary>
    public enum LogBroadcastMode
    {
        /// <summary>
        /// When a message is sent without a channel name it will go to all registered log recievers
        /// </summary>
        Implicit,
        /// <summary>
        /// When a message is sent without a channel name it will only goto to recievers that have registered for All messages, i'e without specifying a ChannelName
        /// </summary>
        Explicit,
    }
}
