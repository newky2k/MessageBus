using DSoft.MessageBus;
using DSoft.MessageBus.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTest
{
    [TestClass]
    public class ConcurrencyTest
    {
        private static IMessageBusService NewBus()
        {
            var services = new ServiceCollection();
            services.RegisterMessageBus();
            return services.BuildServiceProvider().GetRequiredService<IMessageBusService>();
        }

        /// <summary>
        /// Hammers Subscribe/Unsubscribe on the same event id from many threads while
        /// other threads Post continuously. Before the lock fix this raced the
        /// underlying Collection&lt;T&gt; and threw "Collection was modified" /
        /// IndexOutOfRange. Passing means no handler-collection corruption.
        /// </summary>
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        [DataRow(8)]
        [DataRow(9)]
        [DataRow(10)]
        public void ConcurrentSubscribeUnsubscribeAndPost_DoesNotThrow(int run)
        {
            _ = run;
            var bus = NewBus();
            var eventId = "concurrency-" + Guid.NewGuid();

            const int iterations = 5000;
            var exceptions = new System.Collections.Concurrent.ConcurrentQueue<Exception>();

            Action<object, MessageBusEvent> noop = (sender, evt) => { };

            // Writers: churn the handler collection.
            var subscribers = Enumerable.Range(0, 4).Select(_ => Task.Run(() =>
            {
                try
                {
                    for (var i = 0; i < iterations; i++)
                    {
                        var handler = new MessageBusEventHandler(eventId, (s, e) => { });
                        bus.Subscribe(handler);
                        bus.Unsubscribe(handler);
                    }
                }
                catch (Exception ex) { exceptions.Enqueue(ex); }
            }));

            // Readers: dispatch continuously across the churning collection.
            var posters = Enumerable.Range(0, 4).Select(_ => Task.Run(() =>
            {
                try
                {
                    for (var i = 0; i < iterations; i++)
                        bus.Post(eventId, this);
                }
                catch (Exception ex) { exceptions.Enqueue(ex); }
            }));

            Task.WaitAll(subscribers.Concat(posters).ToArray());

            Assert.IsTrue(exceptions.IsEmpty,
                "Concurrent access threw: " + string.Join("; ", exceptions.Select(e => e.GetType().Name + ": " + e.Message)));
        }

        /// <summary>
        /// A handler registered once must fire on every Post issued in parallel.
        /// Verifies dispatch stays correct (no lost/dropped invocations) under load.
        /// </summary>
        [TestMethod]
        public void ConcurrentPosts_AllReachSubscriber()
        {
            var bus = NewBus();
            var eventId = "concurrency-count-" + Guid.NewGuid();

            const int postsPerThread = 2000;
            const int threads = 8;
            var received = 0;

            bus.Subscribe(eventId, (s, e) => Interlocked.Increment(ref received));

            var tasks = Enumerable.Range(0, threads).Select(_ => Task.Run(() =>
            {
                for (var i = 0; i < postsPerThread; i++)
                    bus.Post(eventId, this);
            }));

            Task.WaitAll(tasks.ToArray());

            Assert.AreEqual(threads * postsPerThread, received);
        }
    }
}
