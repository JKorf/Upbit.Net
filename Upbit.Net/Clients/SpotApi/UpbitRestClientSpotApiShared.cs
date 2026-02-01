using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Upbit.Net.Interfaces.Clients.SpotApi;
using Upbit.Net.Objects.Models;

namespace Upbit.Net.Clients.SpotApi
{
    internal partial class UpbitRestClientSpotApi : IUpbitRestClientSpotApiShared
    {
        private const string _topicId = "UpbitSpot";
        public string Exchange => "Upbit";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();

        #region Klines Client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(SharedPaginationSupport.Descending, true, 1000, false);

        async Task<ExchangeWebResult<SharedKline[]>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var interval = (Enums.KlineInterval)request.Interval;
            if (!Enum.IsDefined(typeof(Enums.KlineInterval), interval))
                return new ExchangeWebResult<SharedKline[]>(Exchange, ArgumentError.Invalid(nameof(GetKlinesRequest.Interval), "Interval not supported"));

            var validationError = ((IKlineRestClient)this).GetKlinesOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedKline[]>(Exchange, validationError);

            // Determine pagination
            // Data is normally returned oldest first, so to do newest first pagination we have to do some calc
            DateTime endTime = request.EndTime ?? DateTime.UtcNow;
            DateTime? startTime = request.StartTime;
            if (pageToken is DateTimeToken dateTimeToken)
                endTime = dateTimeToken.LastTime;

            var limit = request.Limit ?? 1000;
            if (startTime == null || startTime < endTime)
            {
                var offset = (int)interval * limit;
                startTime = endTime.AddSeconds(-offset);
            }

            if (startTime < request.StartTime)
                startTime = request.StartTime;

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetKlinesAsync(
                symbol,
                interval,
                endTime,
                limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result)
                return new ExchangeWebResult<SharedKline[]>(Exchange, TradingMode.Spot, result.As<SharedKline[]>(default));

            // Get next token
            DateTimeToken? nextToken = null;
            if (result.Data.Length == limit)
            {
                var minOpenTime = result.Data.Min(x => x.OpenTime);
                if (request.StartTime == null || minOpenTime > request.StartTime.Value)
                    nextToken = new DateTimeToken(minOpenTime.AddSeconds(-(int)(interval - 1)));
            }

            return result.AsExchangeResult(Exchange, request.Symbol!.TradingMode, result.Data.AsEnumerable().Reverse().Select(x => 
                new SharedKline(request.Symbol, symbol, x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume)).ToArray(), nextToken);
        }

        #endregion

        #region Spot Symbol client
        EndpointOptions<GetSymbolsRequest> ISpotSymbolRestClient.GetSpotSymbolsOptions { get; } = new EndpointOptions<GetSymbolsRequest>(false);

        async Task<ExchangeWebResult<SharedSpotSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotSymbolRestClient)this).GetSpotSymbolsOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotSymbol[]>(Exchange, validationError);

            var result = await ExchangeData.GetSymbolsAsync(true, ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedSpotSymbol[]>(Exchange, null, default);

            var resultConfig = await ExchangeData.GetSymbolConfigAsync(string.Join(",", result.Data.Select(x => x.Symbol)), ct: ct).ConfigureAwait(false);
            if (!resultConfig)
                return result.AsExchangeResult<SharedSpotSymbol[]>(Exchange, null, default);

            var resultData = result.AsExchangeResult(Exchange, TradingMode.Spot, result.Data.Select(s => {
                var split = s.Symbol.Split('-');
                var config = resultConfig.Data.SingleOrDefault(x => x.Symbol == s.Symbol);
                return new SharedSpotSymbol(split[1], split[0], s.Symbol, true)
                {
                    PriceStep = config?.TickQuantity
                };
            }).ToArray());

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, resultData.Data);
            return resultData;
        }

        public async Task<ExchangeResult<SharedSymbol[]>> GetSpotSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<SharedSymbol[]>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<SharedSymbol[]>(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, baseAsset));
        }

        public async Task<ExchangeResult<bool>> SupportsSpotSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Only Spot symbols allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<bool>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<bool>(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, symbol));
        }

        public async Task<ExchangeResult<bool>> SupportsSpotSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols)
                    return new ExchangeResult<bool>(Exchange, symbols.Error!);
            }

            return new ExchangeResult<bool>(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, symbolName));
        }
        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(1, 5000, false);
        async Task<ExchangeWebResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = ((IOrderBookRestClient)this).GetOrderBookOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedOrderBook>(Exchange, validationError);

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                levels: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedOrderBook>(Exchange, null, default);

            var bids = result.Data.Entries.Select(x => new UpbitOrderBookItem { Price = x.BidPrice, Quantity = x.BidQuantity }).ToArray();
            var asks = result.Data.Entries.Select(x => new UpbitOrderBookItem { Price = x.AskPrice, Quantity = x.AskQuantity }).ToArray();
            return result.AsExchangeResult(Exchange, TradingMode.Spot, new SharedOrderBook(asks, bids));
        }

        #endregion

        #region Recent Trades client
        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(500, false);

        async Task<ExchangeWebResult<SharedTrade[]>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = ((IRecentTradeRestClient)this).GetRecentTradesOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedTrade[]>(Exchange, validationError);

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetTradeHistoryAsync(
                symbol,
                limit: request.Limit ?? 100,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedTrade[]>(Exchange, null, default);

            // Return
            return result.AsExchangeResult(Exchange, TradingMode.Spot, result.Data.Select(x =>
                new SharedTrade(request.Symbol, symbol, x.Quantity, x.Price, x.Timestamp)
                {
                    Side = x.OrderSide == Enums.OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
                }).ToArray());
        }
        #endregion

        #region Spot Ticker client

        GetTickerOptions ISpotTickerRestClient.GetSpotTickerOptions { get; } = new GetTickerOptions(SharedTickerType.Other);
        async Task<ExchangeWebResult<SharedSpotTicker>> ISpotTickerRestClient.GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTickerRestClient)this).GetSpotTickerOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotTicker>(Exchange, validationError);

            var result = await ExchangeData.GetTickerAsync(request.Symbol!.GetSymbol(FormatSymbol), ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedSpotTicker>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, TradingMode.Spot, 
                new SharedSpotTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, result.Data.Symbol), 
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

        GetTickersOptions ISpotTickerRestClient.GetSpotTickersOptions { get; } = new GetTickersOptions(SharedTickerType.Other);
        async Task<ExchangeWebResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = ((ISpotTickerRestClient)this).GetSpotTickersOptions.ValidateRequest(Exchange, request, TradingMode.Spot, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedSpotTicker[]>(Exchange, validationError);

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
            if (!result)
                return result.AsExchangeResult<SharedSpotTicker[]>(Exchange, null, default);

            return result.AsExchangeResult(Exchange, TradingMode.Spot, result.Data.Select(x =>
                new SharedSpotTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, x.Symbol),
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

        EndpointOptions<GetBookTickerRequest> IBookTickerRestClient.GetBookTickerOptions { get; } = new EndpointOptions<GetBookTickerRequest>(false);
        async Task<ExchangeWebResult<SharedBookTicker>> IBookTickerRestClient.GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
        {
            var validationError = ((IBookTickerRestClient)this).GetBookTickerOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedBookTicker>(Exchange, validationError);

            var resultTicker = await ExchangeData.GetOrderBookAsync(request.Symbol!.GetSymbol(FormatSymbol), 1, ct: ct).ConfigureAwait(false);
            if (!resultTicker)
                return resultTicker.AsExchangeResult<SharedBookTicker>(Exchange, null, default);

            return resultTicker.AsExchangeResult(Exchange, request.Symbol!.TradingMode, new SharedBookTicker(
                ExchangeSymbolCache.ParseSymbol(_topicId, resultTicker.Data.Symbol),
                resultTicker.Data.Symbol,
                resultTicker.Data.Entries[0].AskPrice,
                resultTicker.Data.Entries[0].AskQuantity,
                resultTicker.Data.Entries[0].BidPrice,
                resultTicker.Data.Entries[0].BidQuantity));
        }

        #endregion

        #region Trade History client
        GetTradeHistoryOptions ITradeHistoryRestClient.GetTradeHistoryOptions { get; } = new GetTradeHistoryOptions(SharedPaginationSupport.Descending, true, 500, false);

        async Task<ExchangeWebResult<SharedTrade[]>> ITradeHistoryRestClient.GetTradeHistoryAsync(GetTradeHistoryRequest request, INextPageToken? pageToken, CancellationToken ct)
        {
            var validationError = ((ITradeHistoryRestClient)this).GetTradeHistoryOptions.ValidateRequest(Exchange, request, request.Symbol!.TradingMode, SupportedTradingModes);
            if (validationError != null)
                return new ExchangeWebResult<SharedTrade[]>(Exchange, validationError);

            string? cursor = null;
            if (pageToken is CursorToken token)
                cursor = token.Cursor;

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetTradeHistoryAsync(
                symbol,
                endTime: cursor != null ? null : request.EndTime,
                limit: request.Limit ?? 500,
                cursor: cursor,
                ct: ct).ConfigureAwait(false);
            if (!result)
                return result.AsExchangeResult<SharedTrade[]>(Exchange, null, default);

            CursorToken? nextToken = null;
            if (result.Data.Count() == 500)
                nextToken = new CursorToken(result.Data.Min(x => x.SequentialId).ToString());

            // Return
            return result.AsExchangeResult(Exchange, TradingMode.Spot, result.Data.Where(x => x.Timestamp >= request.StartTime).Select(x => 
            new SharedTrade(request.Symbol, symbol, x.Quantity, x.Price, x.Timestamp)
            {
                Side = x.OrderSide == Enums.OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
            }).ToArray(), nextToken);
        }
        #endregion
    }
}
