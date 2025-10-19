using Upbit.Net.Interfaces.Clients;
using Upbit.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace Upbit.Net.Clients
{
    /// <inheritdoc />
    public class UpbitUserClientProvider : IUpbitUserClientProvider
    {
        private static ConcurrentDictionary<string, IUpbitRestClient> _restClients = new ConcurrentDictionary<string, IUpbitRestClient>();
        private static ConcurrentDictionary<string, IUpbitSocketClient> _socketClients = new ConcurrentDictionary<string, IUpbitSocketClient>();
        
        private readonly IOptions<UpbitRestOptions> _restOptions;
        private readonly IOptions<UpbitSocketOptions> _socketOptions;
        private readonly HttpClient _httpClient;
        private readonly ILoggerFactory? _loggerFactory;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public UpbitUserClientProvider(Action<UpbitOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }
        
        /// <summary>
        /// ctor
        /// </summary>
        public UpbitUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<UpbitRestOptions> restOptions,
            IOptions<UpbitSocketOptions> socketOptions)
        {
            _httpClient = httpClient ?? new HttpClient();
            _loggerFactory = loggerFactory;
            _restOptions = restOptions;
            _socketOptions = socketOptions;
        }

        /// <inheritdoc />
        public void InitializeUserClient(string userIdentifier, ApiCredentials credentials, UpbitEnvironment? environment = null)
        {
            CreateRestClient(userIdentifier, credentials, environment);
            CreateSocketClient(userIdentifier, credentials, environment);
        }

        /// <inheritdoc />
        public void ClearUserClients(string userIdentifier)
        {
            _restClients.TryRemove(userIdentifier, out _);
            _socketClients.TryRemove(userIdentifier, out _);
        }

        /// <inheritdoc />
        public IUpbitRestClient GetRestClient(string userIdentifier, ApiCredentials? credentials = null, UpbitEnvironment? environment = null)
        {
            if (!_restClients.TryGetValue(userIdentifier, out var client))
                client = CreateRestClient(userIdentifier, credentials, environment);

            return client;
        }

        /// <inheritdoc />
        public IUpbitSocketClient GetSocketClient(string userIdentifier, ApiCredentials? credentials = null, UpbitEnvironment? environment = null)
        {
            if (!_socketClients.TryGetValue(userIdentifier, out var client))
                client = CreateSocketClient(userIdentifier, credentials, environment);

            return client;
        }

        private IUpbitRestClient CreateRestClient(string userIdentifier, ApiCredentials? credentials, UpbitEnvironment? environment)
        {
            var clientRestOptions = SetRestEnvironment(environment);
            var client = new UpbitRestClient(_httpClient, _loggerFactory, clientRestOptions);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _restClients.TryAdd(userIdentifier, client);
            }
            return client;
        }

        private IUpbitSocketClient CreateSocketClient(string userIdentifier, ApiCredentials? credentials, UpbitEnvironment? environment)
        {
            var clientSocketOptions = SetSocketEnvironment(environment);
            var client = new UpbitSocketClient(clientSocketOptions!, _loggerFactory);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _socketClients.TryAdd(userIdentifier, client);
            }
            return client;
        }

        private IOptions<UpbitRestOptions> SetRestEnvironment(UpbitEnvironment? environment)
        {
            if (environment == null)
                return _restOptions;

            var newRestClientOptions = new UpbitRestOptions();
            var restOptions = _restOptions.Value.Set(newRestClientOptions);
            newRestClientOptions.Environment = environment;
            return Options.Create(newRestClientOptions);
        }

        private IOptions<UpbitSocketOptions> SetSocketEnvironment(UpbitEnvironment? environment)
        {
            if (environment == null)
                return _socketOptions;

            var newSocketClientOptions = new UpbitSocketOptions();
            var restOptions = _socketOptions.Value.Set(newSocketClientOptions);
            newSocketClientOptions.Environment = environment;
            return Options.Create(newSocketClientOptions);
        }

        private static T ApplyOptionsDelegate<T>(Action<T>? del) where T : new()
        {
            var opts = new T();
            del?.Invoke(opts);
            return opts;
        }
    }
}
