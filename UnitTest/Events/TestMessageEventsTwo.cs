using DSoft.MessageBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Events
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DSoft.MessageBus.MessageBusEvent" />
    internal class TestMessageEventsTwo : MessageBusEvent
    {
        public override string EventId => "94848484838181";

        public TestMessageEventsTwo() { }
    }
}
