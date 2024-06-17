using DSoft.MessageBus;
using DSoft.MessageBus.Contracts;
using Microsoft.Extensions.DependencyInjection;
using UnitTest.Events;

namespace UnitTest
{
    [TestClass]
    public class MessageBusTest : BaseTest
    {
        public MessageBusTest() : base() { }

        [TestMethod]
        public void CanReturnDI()
        {
            var messageBus = Provider.GetRequiredService<IMessageBusService>();
        }

        #region Subscribe


        [TestMethod]
        public void CanSubscribeWithEventHandler()
        {
            var messageBus = Provider.GetRequiredService<IMessageBusService>();

            var newMessage = new TestMessageEvents();

            var wasCalled = false;

            Action<object, MessageBusEvent> action = (sender, evt) => 
            {
                wasCalled = true;
            };

            var messageHandler = new MessageBusEventHandler()
            {
                EventId = newMessage.EventId,
                EventAction = action,
            };
            messageBus.Subscribe(messageHandler);

            messageBus.Post(newMessage);

            Assert.IsTrue(wasCalled);

        }

        [TestMethod]
        public void CanSubscribeWithEventId()
        {
            var messageBus = Provider.GetRequiredService<IMessageBusService>();

            var eventId = "12009636";

            var wasCalled = false;

            Action<object, MessageBusEvent> action = (sender, evt) =>
            {
                wasCalled = true;
            };

            messageBus.Subscribe(eventId, action);

            messageBus.Post(eventId);

            Assert.IsTrue(wasCalled);

        }

        [TestMethod]
        public void CanSubscribeWithEventIdAndSender()
        {
            var messageBus = Provider.GetRequiredService<IMessageBusService>();

            var eventId = "78909187363";

            var wasCalled = false;

            Action<object, MessageBusEvent> action = (sender, evt) =>
            {
                wasCalled = true;
            };

            messageBus.Subscribe(eventId, action);

            messageBus.Post(eventId, this);

            Assert.IsTrue(wasCalled);

        }

        [TestMethod]
        public void CanSubscribeWithEventIdAndSenderAndData()
        {
            var messageBus = Provider.GetRequiredService<IMessageBusService>();

            var eventId = "89537373";

            var wasCalled = false;

            var someData = "WBYCBOZSAEuflPLXzGKPtg==";

            Action<object, MessageBusEvent> action = (sender, evt) =>
            {
                wasCalled = true;
            };

            messageBus.Subscribe(eventId, action);

            messageBus.Post(eventId, this, someData);

            Assert.IsTrue(wasCalled);

        }

        [TestMethod]
        public void CanSubscribeWithEventIdAndData()
        {
            var messageBus = Provider.GetRequiredService<IMessageBusService>();

            var eventId = "12894725271";

            var wasCalled = false;

            var someData = "WBYCBOZSAEuflPLXzGKPtg==";

            Action<object, MessageBusEvent> action = (sender, evt) =>
            {
                wasCalled = true;
            };

            messageBus.Subscribe(eventId, action);

            messageBus.PostData(eventId, Data: someData);

            Assert.IsTrue(wasCalled);

        }

        [TestMethod]
        public void CanSubscribeWithSimpleAction()
        {
            var messageBus = Provider.GetRequiredService<IMessageBusService>();

            var eventId = "1937191885";

            var wasCalled = false;

            var someData = "WBYCBOZSAEuflPLXzGKPtg==";

            var action = () =>
            {
                wasCalled = true;
            };

            messageBus.Subscribe(eventId, action);

            messageBus.Post(eventId, this, someData);

            Assert.IsTrue(wasCalled);

        }

        [TestMethod]
        public void CanSubscribeWithObjectsAction()
        {
            var messageBus = Provider.GetRequiredService<IMessageBusService>();

            var eventId = "1937191885";

            var wasCalled = false;

            var someData = "WBYCBOZSAEuflPLXzGKPtg==";

            Action<object[]> action = (data) =>
            {
                wasCalled = true;
            };

            messageBus.Subscribe(eventId, action);

            messageBus.Post(eventId, this, someData);

            Assert.IsTrue(wasCalled);

        }

        [TestMethod]
        public void CanSubscribeWithSubscribeEventHandler()
        {
            var messageBus = Provider.GetRequiredService<IMessageBusService>();

            var newMessage = new TestMessageEventsTwo();

            var wasCalled = false;

            Action<object, MessageBusEvent> action = (sender, evt) =>
            {
                wasCalled = true;
            };

            var messageHandler = new MessageBusEventHandler()
            {
                EventId = newMessage.EventId,
                EventAction = action,
            };
            messageBus.Subscribe<TestMessageEventsTwo>(action);


            messageBus.Post(newMessage);

            Assert.IsTrue(wasCalled);

        }

        #endregion

        #region Unsubscribe

        [TestMethod]
        public void CanUnsubscribeWithEventHandler()
        {
            var messageBus = Provider.GetRequiredService<IMessageBusService>();

            var newMessage = new TestMessageEvents();

            var wasCalled = false;

            Action<object, MessageBusEvent> action = (sender, evt) =>
            {
                wasCalled = true;
            };

            var messageHandler = new MessageBusEventHandler()
            {
                EventId = newMessage.EventId,
                EventAction = action,
            };
            messageBus.Subscribe(messageHandler);

            messageBus.Unsubscribe(messageHandler);

            messageBus.Post(newMessage);

            Assert.IsFalse(wasCalled);

        }

        [TestMethod]
        public void CanUnsubscribeWithEventId()
        {
            var messageBus = Provider.GetRequiredService<IMessageBusService>();

            var eventId = "12009636";

            var wasCalled = false;

            Action<object, MessageBusEvent> action = (sender, evt) =>
            {
                wasCalled = true;
            };

            messageBus.Subscribe(eventId, action);

            messageBus.Unsubscribe(eventId, action);

            messageBus.Post(eventId);

            Assert.IsFalse(wasCalled);

        }

        [TestMethod]
        public void CanUnsubscribeWithEventIdOnly()
        {
            var messageBus = Provider.GetRequiredService<IMessageBusService>();

            var eventId = "3366558899";

            var wasCalled = false;

            Action<object, MessageBusEvent> action = (sender, evt) =>
            {
                wasCalled = true;
            };

            messageBus.Subscribe(eventId, action);

            messageBus.Unsubscribe(eventId);

            messageBus.Post(eventId);

            Assert.IsFalse(wasCalled);

        }

        [TestMethod]
        public void CanUnsubscribeWithSubscribeEventHandler()
        {
            var messageBus = Provider.GetRequiredService<IMessageBusService>();

            var newMessage = new TestMessageEventsTwo();

            var wasCalled = false;

            Action<object, MessageBusEvent> action = (sender, evt) =>
            {
                wasCalled = true;
            };

            var messageHandler = new MessageBusEventHandler()
            {
                EventId = newMessage.EventId,
                EventAction = action,
            };
            messageBus.Subscribe<TestMessageEventsTwo>(action);

            messageBus.Unsubscribe<TestMessageEventsTwo>(action);

            messageBus.Post(newMessage);

            Assert.IsFalse(wasCalled);

        }
        #endregion
    }
}