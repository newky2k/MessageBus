using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestClass]
    public abstract class BaseTest
    {
        public static ServiceProvider Provider { get; private set; }

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            Provider = services.BuildServiceProvider();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.RegisterMessageBus();
        }
    }
}
