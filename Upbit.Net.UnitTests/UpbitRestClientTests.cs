using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using Upbit.Net.Clients;

namespace Upbit.Net.UnitTests
{
    [TestFixture()]
    public class UpbitRestClientTests
    {
        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<UpbitRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<UpbitSocketClient>();
        }
    }
}
