using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Upbit.Net.Interfaces;
using Upbit.Net.Interfaces.Clients;
using Upbit.Net.Objects.Options;

namespace Upbit.Net.SymbolOrderBooks
{
    /// <summary>
    /// Upbit order book factory
    /// </summary>
    public class UpbitOrderBookFactory : IUpbitOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <inheritdoc />
        public string ExchangeName => UpbitExchange.ExchangeName;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public UpbitOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;            
            
            Spot = new OrderBookFactory<UpbitOrderBookOptions>(CreateSpot, Create);
        }
        
         /// <inheritdoc />
        public IOrderBookFactory<UpbitOrderBookOptions> Spot { get; }

        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<UpbitOrderBookOptions>? options = null)
        {
            var symbolName = symbol.GetSymbol(UpbitExchange.FormatSymbol);
            return CreateSpot(symbolName, options);
        }
                
         /// <inheritdoc />
        public ISymbolOrderBook CreateSpot(string symbol, Action<UpbitOrderBookOptions>? options = null)
            => new UpbitSpotSymbolOrderBook(symbol, options, 
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<IUpbitSocketClient>());


    }
}
