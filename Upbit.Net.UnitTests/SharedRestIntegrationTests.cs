using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using Upbit.Net.Interfaces.Clients;
using Upbit.Net.Interfaces.Clients.SpotApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Upbit.Net.UnitTests
{
    [TestFixture]
    internal class SharedRestIntegrationTests
    {
        private bool ManualRun { get; } = false;
        private static SharedSymbol _spotSymbol = new SharedSymbol(TradingMode.Spot, "ETH", "USDT");

        private bool ShouldRun()
        {
            var integrationTests = Environment.GetEnvironmentVariable("INTEGRATION");
            if (!ManualRun && integrationTests != "1")
                return false;

            return true;
        }

        private IUpbitRestClientSpotApiShared GetSpotRestClient()
        {
            var collection = new ServiceCollection();
            collection.AddUpbit(x => x.Rest.OutputOriginalData = true);
            collection.AddLogging(x =>
            {
                x.SetMinimumLevel(LogLevel.Trace);
                x.AddProvider(new TraceLoggerProvider());
            });
            var sp = collection.BuildServiceProvider();
            return sp.GetRequiredService<IUpbitRestClient>().SpotApi.SharedClient;
        }

        [Test]
        public async Task TestSpotKlinesRequests()
        {
            if (!ShouldRun())
                return;

            var client = GetSpotRestClient();
            var result1 = await client.GetKlinesAsync(new GetKlinesRequest(_spotSymbol, SharedKlineInterval.OneDay));
            var result3 = await client.GetKlinesAsync(new GetKlinesRequest(_spotSymbol, SharedKlineInterval.OneDay, DateTime.UtcNow.AddDays(-5), DateTime.UtcNow));
            CheckResults([
                ("SpotKlines", result1),
                ("SpotKlinesTimed", result3),
                ]);
        }

        [Test]
        public async Task TestSpotBookTickersRequests()
        {
            if (!ShouldRun())
                return;

            var client = GetSpotRestClient();
            var result1 = await client.GetBookTickerAsync(new GetBookTickerRequest(_spotSymbol));
            CheckResults("SpotBookTicker", result1);
        }

        [Test]
        public async Task TestSpotOrderBookRequests()
        {
            if (!ShouldRun())
                return;

            var client = GetSpotRestClient();
            var result1 = await client.GetOrderBookAsync(new GetOrderBookRequest(_spotSymbol));
            CheckResults("SpotOrderBook", result1);
        }

        [Test]
        public async Task TestSpotTickerRequests()
        {
            if (!ShouldRun())
                return;

            var client = GetSpotRestClient();
            var result1 = await client.GetSpotTickerAsync(new GetTickerRequest(_spotSymbol));
            var result2 = await client.GetSpotTickersAsync(new GetTickersRequest());
            CheckResults([
                ("SpotTicker", result1),
                ("SpotTickers", result2)
                ]);
        }

        [Test]
        public async Task TestSpotSymbolRequests()
        {
            if (!ShouldRun())
                return;

            var client = GetSpotRestClient();
            var result1 = await client.GetSpotSymbolsAsync(new GetSymbolsRequest());
            CheckResults([
                ("SpotSymbols", result1)
                ]);
        }

        [Test]
        public async Task TestSpotTradesRequests()
        {
            if (!ShouldRun())
                return;

            var client = GetSpotRestClient();
            var result1 = await client.GetRecentTradesAsync(new GetRecentTradesRequest(_spotSymbol));
            CheckResults([
                ("SpotTrades", result1)
                ]);
        }

        private void CheckResults(string name, ICallResult result)
            => CheckResults([(name, result)]);
        private void CheckResults((string, ICallResult result)[] results)
        {
            foreach (var item in results)
            {
                if (!item.result.Success)
                    throw new Exception($"Failed to get {item.Item1}: " + item.result.Error);
            }
        }

        private void CheckResults<T>(string name, ICallResult<T[]> result)
            => CheckResults([(name, result)]);
        private void CheckResults<T>((string, ICallResult<T[]> result)[] results)
        {
            foreach (var item in results)
            {
                if (!item.result.Success)
                    throw new Exception($"Failed to get {item.Item1}: " + item.result.Error);

                if (item.result.Data.Length == 0)
                    throw new Exception($"No response data for {item.Item1}");
            }
        }
    }
}
