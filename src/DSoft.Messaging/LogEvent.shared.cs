using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.MessageBus
{
    public struct LogEvent
    {
        public string Channel { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public LogSeverity Severity { get; set; }

    }
}
