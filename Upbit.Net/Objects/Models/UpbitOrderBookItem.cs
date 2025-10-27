using CryptoExchange.Net.Interfaces;

namespace Upbit.Net.Objects.Models
{
    internal class UpbitOrderBookItem : ISymbolOrderBookEntry
    {
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
