using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using System;
using Upbit.Net.Objects.Internal;

namespace Upbit.Net.Objects.Sockets
{
    internal class UpbitQuery : Query<object>
    {
        private readonly SocketApiClient _client;

        public UpbitQuery(SocketApiClient client, object[] request, bool authenticated, TimeSpan waitForErrorTimeout) : base(request, authenticated, 1)
        {
            _client = client;

            // If there is no response that means it's successful. Wait for x seconds for an error message else assume success
            RequestTimeout = waitForErrorTimeout;
            TimeoutBehavior = TimeoutBehavior.Succeed;

            MessageMatcher = MessageMatcher.Create<SocketError>("error", HandleError);
            MessageRouter = MessageRouter.CreateWithoutTopicFilter<SocketError>("error", HandleError);
        }

        private CallResult HandleError(SocketConnection connection, DateTime receiveTime, string? originalData, SocketError @event)
        {
            return new CallResult(new ServerError(@event.Error.Name, _client.GetErrorInfo(@event.Error.Name, @event.Error.Message)));
        }
    }
}
