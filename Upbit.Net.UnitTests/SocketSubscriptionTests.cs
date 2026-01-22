using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;
using Upbit.Net.Clients;
using Upbit.Net.Objects.Models;
using Upbit.Net.Objects.Options;

namespace Upbit.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateSpotExchangeDataSubscriptions()
        {
            var factory = new LoggerFactory();
            factory.AddProvider(new TraceLoggerProvider());

            var client = new UpbitSocketClient(Options.Create(new Objects.Options.UpbitSocketOptions
            {
            }), factory);
            var tester = new SocketSubscriptionValidator<UpbitSocketClient>(client, "Subscriptions/Spot", "wss://api.upbit.com");
            await tester.ValidateAsync<UpbitTradeUpdate>((client, handler) => client.SpotApi.SubscribeToTradeUpdatesAsync("KRW-ETH", handler), "Trades", ignoreProperties: ["trade_date", "trade_time"]);
            await tester.ValidateAsync<UpbitTickerUpdate>((client, handler) => client.SpotApi.SubscribeToTickerUpdatesAsync("KRW-ETH", handler), "Tickers", ignoreProperties: ["ask_bid", "delisting_date"]);
            await tester.ValidateAsync<UpbitOrderBookUpdate>((client, handler) => client.SpotApi.SubscribeToOrderBookUpdatesAsync("KRW-ETH", 5, handler), "OrderBook", ignoreProperties: []);
            await tester.ValidateAsync<UpbitKlineUpdate>((client, handler) => client.SpotApi.SubscribeToKlineUpdatesAsync("KRW-ETH", Enums.KlineInterval.OneMinute, handler), "Kline", ignoreProperties: []);
        }
    }
}
