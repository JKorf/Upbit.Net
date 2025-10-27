using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Upbit.Net.Interfaces.Clients.SpotApi;
using Upbit.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Converters.MessageParsing;

namespace Upbit.Net.Clients.SpotApi
{
    /// <inheritdoc cref="IUpbitRestClientSpotApi" />
    internal partial class UpbitRestClientSpotApi : RestApiClient, IUpbitRestClientSpotApi
    {
        #region fields 
        internal static TimeSyncState _timeSyncState = new TimeSyncState("Spot Api");

        protected override ErrorMapping ErrorMapping => UpbitErrors.Errors;

        public new UpbitRestOptions ClientOptions => (UpbitRestOptions)base.ClientOptions;
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IUpbitRestClientSpotApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public string ExchangeName => "Upbit";
        #endregion

        #region constructor/destructor
        internal UpbitRestClientSpotApi(ILogger logger, HttpClient? httpClient, UpbitRestOptions options)
            : base(logger, httpClient, options.Environment.RestClientAddress, options, options.SpotOptions)
        {
            ExchangeData = new UpbitRestClientSpotApiExchangeData(logger, this);
        }
        #endregion

        /// <inheritdoc />
        protected override IStreamMessageAccessor CreateAccessor() => new SystemTextJsonStreamMessageAccessor(UpbitExchange._serializerContext);
        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(UpbitExchange._serializerContext);


        /// <inheritdoc />
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new UpbitAuthenticationProvider(credentials);

        internal Task<WebCallResult> SendAsync(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
            => SendToAddressAsync(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult> SendToAddressAsync(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        internal Task<WebCallResult<T>> SendAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
            => SendToAddressAsync<T>(BaseAddress, definition, parameters, cancellationToken, weight);

        internal async Task<WebCallResult<T>> SendToAddressAsync<T>(string baseAddress, RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<T>(baseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        protected override Error ParseErrorResponse(int httpStatusCode, KeyValuePair<string, string[]>[] responseHeaders, IMessageAccessor accessor, Exception? exception)
        {
            if (!accessor.IsValid)
                return new ServerError(ErrorInfo.Unknown, exception: exception);

            var code = accessor.GetValue<int?>(MessagePath.Get().Property("error").Property("name"));
            var msg = accessor.GetValue<string>(MessagePath.Get().Property("error").Property("message"));
            if (msg == null)
                return new ServerError(ErrorInfo.Unknown, exception: exception);

            if (code == null)
                return new ServerError(ErrorInfo.Unknown with { Message = msg }, exception);

            return new ServerError(code.Value, GetErrorInfo(code.Value, msg), exception);
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => throw new NotImplementedException();

        /// <inheritdoc />
        public override TimeSyncInfo? GetTimeSyncInfo()
            => new TimeSyncInfo(_logger, ApiOptions.AutoTimestamp ?? ClientOptions.AutoTimestamp, ApiOptions.TimestampRecalculationInterval ?? ClientOptions.TimestampRecalculationInterval, _timeSyncState);

        /// <inheritdoc />
        public override TimeSpan? GetTimeOffset()
            => _timeSyncState.TimeOffset;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => UpbitExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public IUpbitRestClientSpotApiShared SharedClient => this;

    }
}
