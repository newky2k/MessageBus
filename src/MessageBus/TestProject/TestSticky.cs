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

        MessageBusEventHandler handler;

        [TestInitialize]
        public void Init()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());

            target = new MessageBus();

            registerForStickyEvent("stickyEvent");

            resetLastEvent();
        }

        void registerForStickyEvent(string eventId)
        {
            handler = new MessageBusEventHandler()
            {
                EventAction = (obj, message) =>
                {
                    lastSender = obj;
                    lastEvent = message;
                },
                EventId = eventId
            };

            target.RegisterSticky(handler);
        }

        void unregisterEvent(string eventId)
        {
            target.DeRegister(handler);
        }

        void resetLastEvent()
        {
            lastEvent = null;
            lastSender = null;
        }

        [TestMethod]
        public void ShouldFireValidStickyLikeNormalEvent()
        {
            target.PostSticky("stickyEvent", this, new object[] { 1, 2 });

            Assert.IsTrue(receivedEvent);
            Assert.AreEqual(this, lastSender);
            Assert.AreEqual(this, lastEvent.Sender);
            Assert.AreEqual("stickyEvent", lastEvent.EventId);
            Assert.IsNotNull(lastEvent.Data);
            Assert.IsInstanceOfType(lastEvent.Data, typeof(object[]));
        }

        [TestMethod]
        public void ShouldFireStickyJustAfterRegistration()
        {
            unregisterEvent("stickyEvent");

            target.PostSticky("stickyEvent", this, new object[] { 1, 2 });

            Assert.IsFalse(receivedEvent);

            registerForStickyEvent("stickyEvent");

            Assert.IsNotNull(lastEvent);
            Assert.AreEqual("stickyEvent", lastEvent.EventId);
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
