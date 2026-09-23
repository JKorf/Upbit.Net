using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using Upbit.Net;
using Upbit.Net.Clients;
using Upbit.Net.Interfaces;
using Upbit.Net.Interfaces.Clients;
using Upbit.Net.Objects.Options;
using Upbit.Net.SymbolOrderBooks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Add services such as the IUpbitRestClient and IUpbitSocketClient. Configures the services based on the provided configuration.<br />
        /// See <see href="https://github.com/JKorf/Upbit.Net/blob/main/Examples/example-config.json" /> for an example of how to set up the configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        /// <returns></returns>
        public static IServiceCollection AddUpbit(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = UpbitOptions.CreateFromConfiguration(configuration);

            services.AddSingleton(Options.Options.Create(options.Rest));
            services.AddSingleton(Options.Options.Create(options.Socket));
            services.AddSingleton(Options.Options.Create(options));

            return AddUpbitCore(services, options.SocketClientLifeTime);
        }

        /// <summary>
        /// Add services such as the IUpbitRestClient and IUpbitSocketClient. Services will be configured based on the provided options.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsDelegate">Set options for the Upbit services</param>
        /// <returns></returns>
        public static IServiceCollection AddUpbit(
            this IServiceCollection services,
            Action<UpbitOptions>? optionsDelegate = null)
        {
            var options = UpbitOptions.Create(optionsDelegate);

            services.AddSingleton(Options.Options.Create(options.Rest));
            services.AddSingleton(Options.Options.Create(options.Socket));
            services.AddSingleton(Options.Options.Create(options));

            return AddUpbitCore(services, options.SocketClientLifeTime);
        }

        private static IServiceCollection AddUpbitCore(
            this IServiceCollection services,
            ServiceLifetime? socketClientLifeTime = null)
        {
            services.AddHttpClient<IUpbitRestClient, UpbitRestClient>((client, serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<UpbitRestOptions>>().Value;
                client.Timeout = options.RequestTimeout;
                return new UpbitRestClient(client, serviceProvider.GetRequiredService<ILoggerFactory>(), serviceProvider.GetRequiredService<IOptions<UpbitRestOptions>>());
            }).ConfigurePrimaryHttpMessageHandler((serviceProvider) => {
                var options = serviceProvider.GetRequiredService<IOptions<UpbitRestOptions>>().Value;
                return LibraryHelpers.CreateHttpClientMessageHandler(options);
            }).SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            services.Add(new ServiceDescriptor(typeof(IUpbitSocketClient), x => { return new UpbitSocketClient(x.GetRequiredService<IOptions<UpbitSocketOptions>>(), x.GetRequiredService<ILoggerFactory>()); }, socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddTransient<IUpbitOrderBookFactory, UpbitOrderBookFactory>();
            services.AddTransient<ITrackerFactory, UpbitTrackerFactory>();
            services.AddTransient<IUpbitTrackerFactory, UpbitTrackerFactory>();

            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IUpbitRestClient>().SpotApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IUpbitSocketClient>().SpotApi.SharedClient);

            services.RegisterSharedApiClient<
                IUpbitSharedApiClient,
                UpbitSharedApiClient>(sharedApis => sharedApis
                    .Add(client => client.SpotRest)
                    .Add(client => client.SpotSocket)
                    );

            return services;
        }
    }
}
