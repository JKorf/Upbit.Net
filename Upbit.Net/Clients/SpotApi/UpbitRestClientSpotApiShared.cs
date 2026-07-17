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
    internal partial class UpbitRestClientSpotApi : IUpbitRestClientSpotApiShared
    {
        private const string _exchange = "Upbit";
        private const string _topicId = "UpbitSpot";

        public bool Authenticated => false;

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
        public SharedClientInfo Discover() => SharedUtils.GetClientInfo(UpbitExchange.Metadata, this);

        private static readonly HashSet<string> _fiatCurrencies = ["KRW", "SGD", "IDR", "THB"];

        #region Klines Client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(_exchange, false, true, true, 1000, false, [
            SharedKlineInterval.OneMinute,
            SharedKlineInterval.ThreeMinutes,
            SharedKlineInterval.FiveMinutes,
            SharedKlineInterval.FifteenMinutes,
            SharedKlineInterval.ThirtyMinutes,
            SharedKlineInterval.OneHour,
            SharedKlineInterval.FourHours,
            SharedKlineInterval.OneHour,
            SharedKlineInterval.OneDay,
            SharedKlineInterval.OneWeek,
            SharedKlineInterval.OneMonth
        ]);

        async Task<HttpResult<SharedKline[]>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetKlinesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedKline[]>(Exchange, validationError);

            int limit = request.Limit ?? 1000;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, false);

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetKlinesAsync(
                symbol,
                (Enums.KlineInterval)request.Interval,
                pageParams.EndTime,
                pageParams.Limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedKline[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => Pagination.NextPageFromTime(pageParams, result.Data.Min(x => x.OpenTime)),
                     result.Data.Length,
                     result.Data.Select(x => x.OpenTime),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result, 
                    ExchangeHelpers.ApplyFilter(result.Data, x => x.OpenTime, request.StartTime, request.EndTime, direction)
                    .Select(x => 
                        new SharedKline(request.Symbol, symbol, x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume))
                    .ToArray(), nextPageRequest);
        }

        #endregion

        #region Spot Symbol client
        SharedSymbolCatalog? ISpotSymbolRestClient.SpotSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_topicId, EnvironmentName, null);
        GetSpotSymbolsOptions ISpotSymbolRestClient.GetSpotSymbolsOptions { get; }
            = new GetSpotSymbolsOptions(_exchange, false);

        async Task<HttpResult<SharedSpotSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotSymbol[]>(Exchange, validationError);

            var result = await ExchangeData.GetSymbolsAsync(true, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotSymbol[]>(result);

            // Need to request in multiple request or the server returns an error for too long URI
            var batchSize = 500;
            var batches = Math.Ceiling(result.Data.Length / (decimal)batchSize);
            var resultConfigs = new List<UpbitSymbolConfig>();
            for(var batch = 0; batch < batches; batch++)
            {
                var batchSymbols = result.Data.Skip(batch * batchSize).Take(batchSize).Select(x => x.Symbol).ToArray();
                var batchResultConfig = await ExchangeData.GetSymbolConfigAsync(string.Join(",", batchSymbols), ct: ct).ConfigureAwait(false);
                if (!batchResultConfig.Success)
                    return HttpResult.Fail<SharedSpotSymbol[]>(batchResultConfig);

                resultConfigs.AddRange(batchResultConfig.Data);
            }

            var resultData =
                 result.Data.Select(x => ParseSymbol(x, resultConfigs))
                .ToArray();

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, EnvironmentName, null, resultData);
            return HttpResult.Ok(result, SharedUtils.ApplySymbolFilter(resultData, request));
        }

        private SharedSpotSymbol ParseSymbol(UpbitSymbol s, List<UpbitSymbolConfig> resultConfigs)
        {
            var split = s.Symbol.Split('-');
            var config = resultConfigs.SingleOrDefault(x => x.Symbol == s.Symbol);
            var result = new SharedSpotSymbol(split[1], split[0], s.Symbol, true)
            {
                PriceStep = config?.TickQuantity,
                BaseAssetType = SharedAssetType.Crypto
            };

            if (_fiatCurrencies.Contains(result.QuoteAsset))
            {
                result.QuoteAssetType = SharedAssetType.Fiat;
            }
            else if(LibraryHelpers.IsStableCoin(result.QuoteAsset))
            {
                result.QuoteAssetType = SharedAssetType.Crypto;
                result.QuoteAssetSubType = SharedAssetSubType.StableCoin;
            }

            return result;
        }

        async Task<ExchangeCallResult<SharedSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, EnvironmentName, null, baseAsset));
        }

        async Task<ExchangeCallResult<bool>> ISpotSymbolRestClient.SupportsSpotSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Only Spot symbols allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, EnvironmentName, null, symbol));
        }

        async Task<ExchangeCallResult<bool>> ISpotSymbolRestClient.SupportsSpotSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, EnvironmentName, null, symbolName));
        }
        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(_exchange, 1, 5000, false);
        async Task<HttpResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedOrderBook>(Exchange, validationError);

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                levels: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedOrderBook>(result);

            var bids = result.Data.Entries.Select(x => new UpbitOrderBookItem { Price = x.BidPrice, Quantity = x.BidQuantity }).ToArray();
            var asks = result.Data.Entries.Select(x => new UpbitOrderBookItem { Price = x.AskPrice, Quantity = x.AskQuantity }).ToArray();
            return HttpResult.Ok(result, new SharedOrderBook(asks, bids));
        }

        #endregion

        #region Recent Trades client
        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(_exchange, 500, false);

        async Task<HttpResult<SharedTrade[]>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetRecentTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetTradeHistoryAsync(
                symbol,
                limit: request.Limit ?? 100,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTrade[]>(result);

            // Return
            return HttpResult.Ok(result, result.Data.Select(x =>
                new SharedTrade(request.Symbol, symbol, x.Quantity, x.Price, x.Timestamp)
                {
                    Side = x.OrderSide == Enums.OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
                }).ToArray());
        }
        #endregion

        #region Spot Ticker client

        GetSpotTickerOptions ISpotTickerRestClient.GetSpotTickerOptions { get; } = new GetSpotTickerOptions(_exchange, SharedTickerType.Other);
        async Task<HttpResult<SharedSpotTicker>> ISpotTickerRestClient.GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker>(Exchange, validationError);
            
            var result = await ExchangeData.GetTickerAsync(request.Symbol!.GetSymbol(FormatSymbol), ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker>(result);

            return HttpResult.Ok(result,
                new SharedSpotTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, result.Data.Symbol), 
                    result.Data.Symbol,
                    result.Data.LastPrice, 
                    result.Data.HighPrice,
                    result.Data.LowPrice,
                    result.Data.Volume24h,
                    result.Data.ChangeRate * 100)
            {
                    QuoteVolume = result.Data.QuoteVolume24h
            });
        }

        GetSpotTickersOptions ISpotTickerRestClient.GetSpotTickersOptions { get; } 
            = new GetSpotTickersOptions(_exchange, SharedTickerType.Other);
        async Task<HttpResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker[]>(Exchange, validationError);

            string[] quoteAssets = [];
            if (ClientOptions.Environment.Name == UpbitEnvironment.Live.Name)
                quoteAssets = new[] { "KRW", "BTC", "USDT" };
            else if (ClientOptions.Environment.Name == UpbitEnvironment.Singapore.Name)
                quoteAssets = new[] { "SGD", "BTC", "USDT" };
            else if (ClientOptions.Environment.Name == UpbitEnvironment.Indonesia.Name)
                quoteAssets = new[] { "IDR", "BTC", "USDT" };
            else if (ClientOptions.Environment.Name == UpbitEnvironment.Thailand.Name)
                quoteAssets = new[] { "THB", "BTC", "USDT" };

            var result = await ExchangeData.GetTickersByQuoteAssetsAsync(quoteAssets, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker[]>(result);

            return HttpResult.Ok(result, result.Data.Select(x =>
                new SharedSpotTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    x.LastPrice, 
                    x.HighPrice,
                    x.LowPrice,
                    x.Volume24h,
                    x.ChangeRate * 100)
                {
                    QuoteVolume = x.QuoteVolume24h
                }).ToArray());
        }

        #endregion

        #region Book Ticker client

        GetBookTickerOptions IBookTickerRestClient.GetBookTickerOptions { get; } 
            = new GetBookTickerOptions(_exchange, false);
        async Task<HttpResult<SharedBookTicker>> IBookTickerRestClient.GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBookTicker>(Exchange, validationError);

            var resultTicker = await ExchangeData.GetOrderBookAsync(request.Symbol!.GetSymbol(FormatSymbol), 1, ct: ct).ConfigureAwait(false);
            if (!resultTicker.Success)
                return HttpResult.Fail<SharedBookTicker>(resultTicker);

            return HttpResult.Ok(resultTicker, new SharedBookTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, resultTicker.Data.Symbol),
                resultTicker.Data.Symbol,
                resultTicker.Data.Entries[0].AskPrice,
                resultTicker.Data.Entries[0].AskQuantity,
                resultTicker.Data.Entries[0].BidPrice,
                resultTicker.Data.Entries[0].BidQuantity));
        }

        #endregion

        #region Trade History client
        GetTradeHistoryOptions ITradeHistoryRestClient.GetTradeHistoryOptions { get; } = new GetTradeHistoryOptions(_exchange, false, true, true, 500, false);

        async Task<HttpResult<SharedTrade[]>> ITradeHistoryRestClient.GetTradeHistoryAsync(GetTradeHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetTradeHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

            int limit = request.Limit ?? 500;
            var direction = DataDirection.Descending;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, false);

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetTradeHistoryAsync(
                symbol,
                endTime: pageParams.EndTime,
                limit: pageParams.Limit,
                cursor: pageParams.Cursor,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTrade[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                     () => Pagination.NextPageFromCursor(result.Data.Min(x => x.SequentialId).ToString()),
                     result.Data.Length,
                     result.Data.Select(x => x.Timestamp),
                     request.StartTime,
                     request.EndTime ?? DateTime.UtcNow,
                     pageParams);

            return HttpResult.Ok(result,
                    ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                    .Select(x => 
                        new SharedTrade(request.Symbol, symbol, x.Quantity, x.Price, x.Timestamp)
                        {
                            Side = x.OrderSide == Enums.OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
                        })
                    .ToArray(), nextPageRequest);
        }
        #endregion
    }
}
