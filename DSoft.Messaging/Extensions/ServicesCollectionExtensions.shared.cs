using DSoft.MessageBus;
using DSoft.MessageBus.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Services Collection Extensions
    /// </summary>
    public static class ServicesCollectionExtensions
    {
        /// <summary>
        /// Registers the DSoft.MessageBus DI Services
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterMessageBus(this IServiceCollection services)
        {
            services.TryAddSingleton<IMessageBusService, MessageBusService>();

            return services;
        }
    }
}
