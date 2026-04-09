using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default.Routing;

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
