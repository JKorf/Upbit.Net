using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
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

        #region Get Server Time

        /// <inheritdoc />
        public async Task<WebCallResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, "XXX", UpbitExchange.RateLimiter.Upbit, 1, false);
            var result = await _baseClient.SendAsync<UpbitModel>(request, null, ct).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<WebCallResult<UpbitSymbol[]>> GetSymbolsAsync(bool includeNotifications, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("is_details", includeNotifications);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/market/all", UpbitExchange.RateLimiter.Upbit, 1, false);
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
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/trades/ticks", UpbitExchange.RateLimiter.Upbit, 1, false);
            var result = await _baseClient.SendAsync<UpbitTrade[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<WebCallResult<UpbitTicker[]>> GetTickersAsync(string symbols, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("markets", symbols);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/ticker", UpbitExchange.RateLimiter.Upbit, 1, false);
            var result = await _baseClient.SendAsync<UpbitTicker[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<WebCallResult<UpbitTicker[]>> GetTickersByQuoteAssetsAsync(string quoteAssets, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("quote_currencies", quoteAssets);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/v1/ticker/all", UpbitExchange.RateLimiter.Upbit, 1, false);
            var result = await _baseClient.SendAsync<UpbitTicker[]>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<UpbitOrderBook[]>> GetOrderBookAsync(string symbols, int? levels = null, decimal? aggregation = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.Add("markets", symbols);
            parameters.AddOptional("count", levels);
            parameters.AddOptional("level", aggregation);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "v1/orderbook", UpbitExchange.RateLimiter.Upbit, 1, false);
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
            parameters.AddOptional("to", endTime);
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

            var request = _definitions.GetOrCreate(HttpMethod.Get, urlPath, UpbitExchange.RateLimiter.Upbit, 1, false);
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
