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
    internal partial class UpbitRestClientSpotSharedApi :
        SharedApiBase,
        IUpbitRestClientSpotApiShared,
        IUpbitRestClientSpotSharedApi
    {
        private readonly UpbitRestClientSpotApi _api;

        private const string _exchange = "Upbit";
        private const string _topicId = "UpbitSpot";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(UpbitExchange.Metadata, this);

        private static readonly HashSet<string> _fiatCurrencies = ["KRW", "SGD", "IDR", "THB"];


        public UpbitRestClientSpotSharedApi(UpbitRestClientSpotApi api)
            : base(
                  api.Exchange,
                  [TradingMode.Spot],
                  () => false,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetKlinesOptions,
                GetSpotSymbolsOptions,
                GetOrderBookOptions,
                GetRecentTradesOptions,
                GetSpotTickerOptions,
                GetAllSpotTickersOptions,
                GetBookTickerOptions,
                GetTradeHistoryOptions
                );
        }

    }
}
