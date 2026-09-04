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

        #region Get Spot Ticker

        async Task<ICallResult<SharedSpotTicker>> IGetSpotTicker.GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
            => await GetSpotTickerAsync(request, ct).ConfigureAwait(false);

        public GetSpotTickerOptions GetSpotTickerOptions { get; } = new GetSpotTickerOptions(_exchange, SharedTickerType.Other);
        public async Task<HttpResult<SharedSpotTicker>> GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = GetSpotTickerOptions.ValidateRequest(request, this);
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

        #region Get All Spot Tickers

        async Task<ICallResult<SharedSpotTicker[]>> IGetAllSpotTickers.GetAllSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
            => await GetAllSpotTickersAsync(request, ct).ConfigureAwait(false);

        Task<HttpResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
            => GetAllSpotTickersAsync(request, ct);
        GetAllSpotTickersOptions ISpotTickerRestClient.GetSpotTickersOptions => GetAllSpotTickersOptions;

        public GetAllSpotTickersOptions GetAllSpotTickersOptions { get; } 
            = new GetAllSpotTickersOptions(_exchange, SharedTickerType.Other);
        public async Task<HttpResult<SharedSpotTicker[]>> GetAllSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = GetAllSpotTickersOptions.ValidateRequest(request, this);
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
