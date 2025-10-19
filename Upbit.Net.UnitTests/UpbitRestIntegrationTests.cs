using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Upbit.Net.Clients;
using Upbit.Net.Objects.Options;

namespace Upbit.Net.UnitTests
{
    [NonParallelizable]
    public class UpbitRestIntegrationTests : RestIntegrationTest<UpbitRestClient>
    {
        public override bool Run { get; set; } = false;

        public override UpbitRestClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new UpbitRestClient(null, loggerFactory, Options.Create(new UpbitRestOptions
            {
                AutoTimestamp = false,
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new CryptoExchange.Net.Authentication.ApiCredentials(key, sec) : null
            }));
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

#warning Implement error response
            //var result = await CreateClient().SpotApi.ExchangeData.GetTickerAsync("TSTTST", default);

            //Assert.That(result.Success, Is.False);
            //Assert.That(result.Error.Code, Is.EqualTo(-1121));
        }

        [Test]
        public async Task TestSpotExchangeData()
        {
            //await RunAndCheckResult(client => client.SpotApi.ExchangeData.PingAsync(CancellationToken.None), false);
        }
    }
}
