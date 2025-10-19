using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System.Threading.Tasks;
using Upbit.Net.Clients;
using Upbit.Net.Objects.Models;

namespace Upbit.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateSpotExchangeDataSubscriptions()
        {
            var client = new UpbitSocketClient(opts =>
            {
                opts.ApiCredentials = new CryptoExchange.Net.Authentication.ApiCredentials("123", "456");
            });
            var tester = new SocketSubscriptionValidator<UpbitSocketClient>(client, "Subscriptions/Spot", "XXX");
            //await tester.ValidateAsync<UpbitModel>((client, handler) => client.SpotApi.SubscribeToXXXUpdatesAsync(handler), "XXX");
        }
    }
}
