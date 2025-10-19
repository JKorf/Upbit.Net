using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Upbit.Net.Clients.SpotApi;
using Upbit.Net.Interfaces.Clients;
using Upbit.Net.Interfaces.Clients.SpotApi;
using Upbit.Net.Objects.Options;

namespace Upbit.Net.Clients
{
    /// <inheritdoc cref="IUpbitSocketClient" />
    public class UpbitSocketClient : BaseSocketClient, IUpbitSocketClient
    {
        #region fields
        #endregion

        #region Api clients
                
         /// <inheritdoc />
        public IUpbitSocketClientSpotApi SpotApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of UpbitSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public UpbitSocketClient(Action<UpbitSocketOptions>? optionsDelegate = null)
            : this(Options.Create(ApplyOptionsDelegate(optionsDelegate)), null)
        {
        }

        /// <summary>
        /// Create a new instance of UpbitSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="options">Option configuration</param>
        public UpbitSocketClient(IOptions<UpbitSocketOptions> options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "Upbit")
        {
            Initialize(options.Value);
            
            SpotApi = AddApiClient(new UpbitSocketClientSpotApi(_logger, options.Value));
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
        public static void SetDefaultOptions(Action<UpbitSocketOptions> optionsDelegate)
        {
            UpbitSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }

        /// <inheritdoc />
        public void SetApiCredentials(ApiCredentials credentials)
        {            
            SpotApi.SetApiCredentials(credentials);
        }
    }
}
