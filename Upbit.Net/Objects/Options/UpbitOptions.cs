using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Configuration;
using System;

namespace Upbit.Net.Objects.Options
{
    /// <summary>
    /// Upbit options
    /// </summary>
    public class UpbitOptions : LibraryOptions<UpbitRestOptions, UpbitSocketOptions, UpbitEnvironment>
    {
        /// <summary>
        /// Options for Shared API usage
        /// </summary>
        public SharedApiOptions SharedApi { get; set; } = new();
        /// <summary>
        /// Create UpbitOptions instance using the provided configuration action
        /// </summary>
        public static UpbitOptions Create(Action<UpbitOptions>? configure = null)
        {
            var options = CreateUnconfigured();
            configure?.Invoke(options);
            return Normalize(options);
        }

        /// <summary>
        /// Create UpbitOptions using the provided IConfiguration
        /// </summary>
        public static UpbitOptions CreateFromConfiguration(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var options = CreateUnconfigured();
            try
            {
                configuration.Bind(options);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Invalid Upbit configuration provided", ex);
            }

            if (options.Environment != null)
                options.Environment = UpbitEnvironment.GetEnvironmentByName(options.Environment.Name) ?? options.Environment;
            if (options.Rest?.Environment != null)
                options.Rest.Environment = UpbitEnvironment.GetEnvironmentByName(options.Rest.Environment.Name) ?? options.Rest.Environment;
            if (options.Socket?.Environment != null)
                options.Socket.Environment = UpbitEnvironment.GetEnvironmentByName(options.Socket.Environment.Name) ?? options.Socket.Environment;

            return Normalize(options);
        }

        private static UpbitOptions CreateUnconfigured()
        {
            var options = new UpbitOptions();
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            return options;
        }

        private static UpbitOptions Normalize(UpbitOptions options)
        {
            if (options.Rest == null)
                throw new ArgumentException("REST options cannot be null", nameof(options));
            if (options.Socket == null)
                throw new ArgumentException("Socket options cannot be null", nameof(options));

            options.Rest.Environment ??= options.Environment ?? UpbitEnvironment.Live;
            options.Socket.Environment ??= options.Environment ?? UpbitEnvironment.Live;
            return options;
        }
    }
}
