using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.RateLimiting.Guards;
using Microsoft.Extensions.Logging;
using Upbit.Net.Enums;
using Upbit.Net.Interfaces.Clients.SpotApi;
using Upbit.Net.Objects.Models;

namespace Upbit.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class UpbitRestClientSpotApiExchangeData : IUpbitRestClientSpotApiExchangeData
    {
        private readonly UpbitRestClientSpotApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal UpbitRestClientSpotApiExchangeData(ILogger logger, UpbitRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Symbols

        /// <inheritdoc />
        public async Task<WebCallResult<UpbitSymbol[]>> GetSymbolsAsync(bool includeNotifications, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("is_details", includeNotifications);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/market/all", UpbitExchange.RateLimiter.Upbit, 1, false,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<UpbitSymbol[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Trade History

        /// <inheritdoc />
        public async Task<WebCallResult<UpbitTrade[]>> GetTradeHistoryAsync(
            string symbol,
            DateTime? endTime = null,
            int? limit = null, 
            string? cursor = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("market", symbol);
            if (endTime != null)
            {
                var time = endTime.Value.ToString("HHmmss");
                var daysAgo = Math.Floor((DateTime.UtcNow - endTime.Value).TotalDays);
                parameters.Add("to", time);
                parameters.Add("days_ago", daysAgo);
            }
            parameters.AddOptional("count", limit);
            parameters.AddOptional("cursor", cursor);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/trades/ticks", UpbitExchange.RateLimiter.Upbit, 1, false,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<UpbitTrade[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<WebCallResult<UpbitTicker>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            var result = await GetTickersAsync([symbol], ct).ConfigureAwait(false);
            if (!result)
                return result.As<UpbitTicker>(default);

            if (!result.Data.Any())
                return result.AsError<UpbitTicker>(new ServerError(new ErrorInfo(ErrorType.InvalidParameter, "Invalid ticker request")));

            return result.As(result.Data.Single());
        }

        /// <inheritdoc />
        public async Task<WebCallResult<UpbitTicker[]>> GetTickersAsync(IEnumerable<string> symbols, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("markets", string.Join(",", symbols));
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/ticker", UpbitExchange.RateLimiter.RestTicker, 1, false);
            var result = await _baseClient.SendAsync<UpbitTicker[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Tickers By Quote Asset

        /// <inheritdoc />
        public async Task<WebCallResult<UpbitTicker[]>> GetTickersByQuoteAssetsAsync(IEnumerable<string> quoteAssets, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("quote_currencies", string.Join(",", quoteAssets));
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/ticker/all", UpbitExchange.RateLimiter.RestTicker, 1, false);
            var result = await _baseClient.SendAsync<UpbitTicker[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<UpbitOrderBook>> GetOrderBookAsync(string symbol, int? levels = null, decimal? aggregation = null, CancellationToken ct = default)
        {
            var result = await GetOrderBooksAsync([symbol], levels, aggregation, ct).ConfigureAwait(false);
            if (!result)
                return result.As<UpbitOrderBook>(default);

            if (!result.Data.Any())
                return result.AsError<UpbitOrderBook>(new ServerError(new ErrorInfo(ErrorType.InvalidParameter, "Invalid book request")));

            return result.As(result.Data.Single());
        }

        /// <inheritdoc />
        public async Task<WebCallResult<UpbitOrderBook[]>> GetOrderBooksAsync(IEnumerable<string> symbols, int? levels = null, decimal? aggregation = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("markets", string.Join(",", symbols));
            parameters.AddOptional("count", levels);
            parameters.AddOptional("level", aggregation);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "v1/orderbook", UpbitExchange.RateLimiter.Upbit, 1, false,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<UpbitOrderBook[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<WebCallResult<UpbitKline[]>> GetKlinesAsync(
            string symbol,
            KlineInterval interval,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("market", symbol);
            parameters.AddOptional("count", limit);
            parameters.AddOptional("to", endTime?.ToString("yyyy-MM-ddThh:mm:ssZ"));
            var urlPath = "v1/candles/";
            var intInterval = (int)interval;
            if (interval == KlineInterval.OneSecond)
                urlPath += "seconds";
            else if (intInterval <= 14400)
                urlPath += "minutes/" + (TimeSpan.FromSeconds(intInterval).TotalMinutes);
            else if (interval == KlineInterval.OneDay)
                urlPath += "days";
            else if (interval == KlineInterval.OneWeek)
                urlPath += "weeks";
            else if (interval == KlineInterval.OneMonth)
                urlPath += "months";
            else if (interval == KlineInterval.OneYear)
                urlPath += "years";

            var request = _definitions.GetOrCreate(HttpMethod.Get, urlPath, UpbitExchange.RateLimiter.Upbit, 1, false,
                limitGuard: new SingleLimitGuard(10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            var result = await _baseClient.SendAsync<UpbitKline[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Symbol Config

        /// <inheritdoc />
        public async Task<WebCallResult<UpbitSymbolConfig[]>> GetSymbolConfigAsync(string symbols, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("markets", symbols);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "v1/orderbook/instruments", UpbitExchange.RateLimiter.Upbit, 1, false);
            var result = await _baseClient.SendAsync<UpbitSymbolConfig[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
