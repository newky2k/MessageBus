using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.MessageBus
{
    /// <summary>
    /// LogEvent
    /// </summary>
    public struct LogEvent
    {
        /// <summary>
        /// Gets or sets the channel.
        /// </summary>
        /// <value>The channel.</value>
        public string Channel { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the severity.
        /// </summary>
        /// <value>The severity.</value>
        public LogSeverity Severity { get; set; }

    }
}
