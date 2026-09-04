using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Upbit.Net.Interfaces.Clients.SpotApi;
using Upbit.Net.Objects.Models;

namespace Upbit.Net.Clients.SpotApi
{
    internal partial class UpbitSocketClientSpotSharedApi : 
        SharedApiBase,
        IUpbitSocketClientSpotApiShared,
        IUpbitSocketClientSpotSharedApi
    {
        private readonly UpbitSocketClientSpotApi _api;

        private const string _exchange = "Upbit";
        private const string _topicId = "UpbitSpot";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(UpbitExchange.Metadata, this);

        public UpbitSocketClientSpotSharedApi(UpbitSocketClientSpotApi api)
            : base(
                  SharedTransport.Socket,
                  api.Exchange,
                  [TradingMode.Spot],
                  () => false,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                SubscribeOrderBookOptions,
                SubscribeKlineOptions,
                SubscribeBookTickerOptions,
                SubscribeTradeOptions,
                SubscribeTickerOptions
                );
        }

    }
}
