using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.OrderBook;
using Microsoft.Extensions.Logging;
using Upbit.Net.Clients;
using Upbit.Net.Interfaces.Clients;
using Upbit.Net.Objects.Models;
using Upbit.Net.Objects.Options;

namespace Upbit.Net.SymbolOrderBooks
{
    /// <summary>
    /// Implementation for a synchronized order book. After calling Start the order book will sync itself and keep up to date with new data. It will automatically try to reconnect and resync in case of a lost/interrupted connection.
    /// Make sure to check the State property to see if the order book is synced.
    /// </summary>
    public class UpbitSpotSymbolOrderBook : SymbolOrderBook
    {
        private readonly bool _clientOwner;
        private readonly IUpbitSocketClient _socketClient;
        private readonly TimeSpan _initialDataTimeout;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public UpbitSpotSymbolOrderBook(string symbol, Action<UpbitOrderBookOptions>? optionsDelegate = null)
            : this(symbol, optionsDelegate, null, null)
        {
            _clientOwner = true;
        }

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        /// <param name="logger">Logger</param>
        /// <param name="socketClient">Socket client instance</param>
        public UpbitSpotSymbolOrderBook(
            string symbol,
            Action<UpbitOrderBookOptions>? optionsDelegate,
            ILoggerFactory? logger,
            IUpbitSocketClient? socketClient) : base(logger, "Upbit", "Spot", symbol)
        {
            var options = UpbitOrderBookOptions.Default.Copy();
            if (optionsDelegate != null)
                optionsDelegate(options);
            Initialize(options);

            _strictLevels = false;
            _sequencesAreConsecutive = options?.Limit == null;

            Levels = options?.Limit;
            _initialDataTimeout = options?.InitialDataTimeout ?? TimeSpan.FromSeconds(30);
            _clientOwner = socketClient == null;
            _socketClient = socketClient ?? new UpbitSocketClient();
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStartAsync(CancellationToken ct)
        {
            var subResult = await _socketClient.SpotApi.SubscribeToOrderBookUpdatesAsync(Symbol, Levels ?? 15, HandleUpdate).ConfigureAwait(false);
            if (!subResult)
                return new CallResult<UpdateSubscription>(subResult.Error!);

            Status = OrderBookStatus.Syncing;

            var setResult = await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
            if (!setResult)
                await subResult.Data.CloseAsync().ConfigureAwait(false);

            return setResult ? subResult : new CallResult<UpdateSubscription>(setResult.Error!);
        }

        private void HandleUpdate(DataEvent<UpbitOrderBookUpdate> @event)
        {
            var bids = @event.Data.Entries.Select(x => new UpbitOrderBookItem { Price = x.BidPrice, Quantity = x.BidQuantity }).ToArray();
            var asks = @event.Data.Entries.Select(x => new UpbitOrderBookItem { Price = x.AskPrice, Quantity = x.AskQuantity }).ToArray();
            SetInitialOrderBook(@event.Data.Timestamp.Ticks, bids, asks);
        }

        /// <inheritdoc />
        protected override void DoReset()
        {
        }

        /// <inheritdoc />
        protected override async Task<CallResult<bool>> DoResyncAsync(CancellationToken ct)
        {
            return await WaitForSetOrderBookAsync(_initialDataTimeout, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (_clientOwner)            
                _socketClient?.Dispose();

            base.Dispose(disposing);
        }
    }
}
