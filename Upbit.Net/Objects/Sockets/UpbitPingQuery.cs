using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using Upbit.Net.Objects.Models;

namespace Upbit.Net.Objects.Sockets
{
    internal class UpbitPingQuery : Query<string>
    {
        public UpbitPingQuery() : base("PING", false, 1)
        {
            ExpectsResponse = false;
        }
    }
}
