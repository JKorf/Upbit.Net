using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using System.Collections.Generic;
using Upbit.Net.Objects.Models;

namespace Upbit.Net.Objects.Sockets
{
    internal class UpbitQuery : Query<object>
    {
        public UpbitQuery(object[] request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            ExpectsResponse = false;
        }
    }
}
