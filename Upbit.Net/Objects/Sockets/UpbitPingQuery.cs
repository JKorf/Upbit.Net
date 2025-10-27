using CryptoExchange.Net.Sockets;

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
