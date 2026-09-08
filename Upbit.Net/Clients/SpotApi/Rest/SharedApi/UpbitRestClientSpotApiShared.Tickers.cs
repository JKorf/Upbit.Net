using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Upbit.Net.Interfaces.Clients.SpotApi;
using Upbit.Net.Objects.Models;

namespace Upbit.Net.Clients.SpotApi
{
    internal partial class UpbitRestClientSpotSharedApi
    {

        #region Get Ticker

        async Task<ICallResult<SharedTicker>> IGetTicker.GetTickerAsync(GetTickerRequest request, CancellationToken ct)
            => await ((IGetTickerRest)this).GetTickerAsync(request, ct).ConfigureAwait(false);

        async Task<HttpResult<SharedTicker>> IGetTickerRest.GetTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var result = await GetSpotTickerAsync(request, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTicker>(result);

            return HttpResult.Ok<SharedTicker>(result, result.Data);
        }

        GetTickerOptions ISpotTickerRestClient.GetSpotTickerOptions => GetTickerOptions;

        public GetTickerOptions GetTickerOptions { get; } = new GetTickerOptions(_exchange, SharedTickerType.Other);
        public async Task<HttpResult<SharedSpotTicker>> GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = GetTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker>(Exchange, validationError);
            
            var result = await _api.ExchangeData.GetTickerAsync(request.Symbol!.GetSymbol(FormatSymbol), ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker>(result);

            return HttpResult.Ok(result,
                new SharedSpotTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, result.Data.Symbol), 
                    result.Data.Symbol,
                    result.Data.LastPrice, 
                    result.Data.HighPrice,
                    result.Data.LowPrice,
                    new SharedOrderQuantity(result.Data.Volume24h, result.Data.QuoteVolume24h),
                    result.Data.ChangeRate * 100)
            {
            });
        }

        #endregion

        #region Get All Tickers

        async Task<ICallResult<SharedTicker[]>> IGetAllTickers.GetAllTickersAsync(GetTickersRequest request, CancellationToken ct)
            => await ((IGetAllTickersRest)this).GetAllTickersAsync(request, ct).ConfigureAwait(false);

        async Task<HttpResult<SharedTicker[]>> IGetAllTickersRest.GetAllTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var result = await GetAllSpotTickersAsync(request, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTicker[]>(result);

            return HttpResult.Ok<SharedTicker[]>(result, result.Data);
        }

        Task<HttpResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
            => GetAllSpotTickersAsync(request, ct);
        GetAllTickersOptions ISpotTickerRestClient.GetSpotTickersOptions => GetAllTickersOptions;

        public GetAllTickersOptions GetAllTickersOptions { get; }
            = new GetAllTickersOptions(_exchange, SharedTickerType.Other);
        public async Task<HttpResult<SharedSpotTicker[]>> GetAllSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = GetAllTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker[]>(Exchange, validationError);

            string[] quoteAssets = [];
            if (_api.ClientOptions.Environment.Name == UpbitEnvironment.Live.Name)
                quoteAssets = new[] { "KRW", "BTC", "USDT" };
            else if (_api.ClientOptions.Environment.Name == UpbitEnvironment.Singapore.Name)
                quoteAssets = new[] { "SGD", "BTC", "USDT" };
            else if (_api.ClientOptions.Environment.Name == UpbitEnvironment.Indonesia.Name)
                quoteAssets = new[] { "IDR", "BTC", "USDT" };
            else if (_api.ClientOptions.Environment.Name == UpbitEnvironment.Thailand.Name)
                quoteAssets = new[] { "THB", "BTC", "USDT" };

            var result = await _api.ExchangeData.GetTickersByQuoteAssetsAsync(quoteAssets, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x =>
                new SharedSpotTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    x.LastPrice, 
                    x.HighPrice,
                    x.LowPrice,
                    new SharedOrderQuantity(x.Volume24h, x.QuoteVolume24h),
                    x.ChangeRate * 100)
                {
                }).ToArray());
        }

        #endregion

    }
}
