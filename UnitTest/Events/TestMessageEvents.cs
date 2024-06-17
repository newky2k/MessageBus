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
    internal class TestMessageEvents : MessageBusEvent
    {
        public override string EventId => "123456";

        public TestMessageEvents() { }
    }
}
