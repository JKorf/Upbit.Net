using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Upbit.Net.Clients;

namespace Upbit.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateExchangeDataAccountCalls()
        {
            var client = new UpbitRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new RestRequestValidator<UpbitRestClient>(client, "Endpoints/Spot/ExchangeData", "https://api.upbit.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTradeHistoryAsync("123"), "GetTradeHistory");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTickersAsync("123"), "GetTickers");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetOrderBookAsync("123"), "GetOrderBook");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetSymbolsAsync(true), "GetSymbols");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetKlinesAsync("123", Enums.KlineInterval.OneSecond), "GetKlines");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetSymbolConfigAsync("123"), "GetSymbolConfig");
        }

        private bool IsAuthenticated(WebCallResult result)
        {
            return result.RequestUrl?.Contains("signature") == true || result.RequestBody?.Contains("signature=") == true;
        }
    }
}
