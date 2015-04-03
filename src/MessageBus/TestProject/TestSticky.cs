using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DSoft.Messaging;
using System.Threading;

namespace TestProject
{
    [TestClass]
    public class TestSticky
    {
        MessageBus target;

        object lastSender;
        MessageBusEvent lastEvent;

        [TestInitialize]
        public void Init()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            target = new MessageBus();
            target.Register(new MessageBusEventHandler()
            {
                EventAction = (obj, message) =>
                {
                    lastSender = obj;
                    lastEvent = message;
                },
                EventId = "stickyEvent"
            });

            resetLastEvent();
        }

        void resetLastEvent()
        {
            lastEvent = null;
            lastSender = null;
        }

        [TestMethod]
        public void ShouldFireStickyLikeNormalEvent()
        {
            target.PostSticky("stickyEvent", this, new object[] { 1, 2 });

            Assert.IsTrue(receivedEvent);
            Assert.AreEqual(this, lastSender);
            Assert.AreEqual(this, lastEvent.Sender);
            Assert.AreEqual("stickyEvent", lastEvent.EventId);
            Assert.IsNotNull(lastEvent.Data);
            Assert.IsInstanceOfType(lastEvent.Data, typeof(object[]));
        }

        bool receivedEvent
        {
            get
            {
                return lastEvent != null;
            }
        }
    }
}
