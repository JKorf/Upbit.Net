using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using CryptoExchange.Net.Authentication;
using Upbit.Net.Interfaces.Clients;
using Upbit.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Options;
using CryptoExchange.Net.Objects.Options;
using Upbit.Net.Interfaces.Clients.SpotApi;
using Upbit.Net.Clients.SpotApi;

namespace Upbit.Net.Clients
{
    /// <inheritdoc cref="IUpbitRestClient" />
    public class UpbitRestClient : BaseRestClient, IUpbitRestClient
    {
        #region Api clients
        
         /// <inheritdoc />
        public IUpbitRestClientSpotApi SpotApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of the UpbitRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public UpbitRestClient(Action<UpbitRestOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate)))
        {
        }

        /// <summary>
        /// Create a new instance of the UpbitRestClient using provided options
        /// </summary>
        /// <param name="options">Option configuration</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public UpbitRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, IOptions<UpbitRestOptions> options) : base(loggerFactory, "Upbit")
        {
            Initialize(options.Value);
                        
            SpotApi = AddApiClient(new UpbitRestClientSpotApi(_logger, httpClient, options.Value));
        }

        #endregion

        /// <inheritdoc />
        public void SetOptions(UpdateOptions options)
        {
            SpotApi.SetOptions(options);
        }

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<UpbitRestOptions> optionsDelegate)
        {
            UpbitRestOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {            
            SpotApi.SetApiCredentials(credentials);
        }
    }
}
