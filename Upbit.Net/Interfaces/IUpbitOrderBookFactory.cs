using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using System;
using Upbit.Net.Objects.Options;

namespace Upbit.Net.Interfaces
{
    /// <summary>
    /// Upbit local order book factory
    /// </summary>
    public interface IUpbitOrderBookFactory
    {
        
        /// <summary>
        /// Spot order book factory methods
        /// </summary>
        IOrderBookFactory<UpbitOrderBookOptions> Spot { get; }


        /// <summary>
        /// Create a SymbolOrderBook for the symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook Create(SharedSymbol symbol, Action<UpbitOrderBookOptions>? options = null);

        
        /// <summary>
        /// Create a new Spot local order book instance
        /// </summary>
        ISymbolOrderBook CreateSpot(string symbol, Action<UpbitOrderBookOptions>? options = null);

    }
}