using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Text;
using Upbit.Net.Interfaces.Clients.SpotApi;

namespace Upbit.Net.Clients.SpotApi
{
    internal partial class UpbitRestClientSpotApi : IUpbitRestClientSpotApiShared
    {
        private const string _topicId = "UpbitSpot";
        public string Exchange => "Upbit";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
    }
}
