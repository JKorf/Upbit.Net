using System;
using System.Text.Json.Serialization;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Order book info
    /// </summary>
    public record UpbitOrderBook
    {
        /// <summary>
        /// ["<c>market</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>total_ask_size</c>"] Total ask quantity
        /// </summary>
        [JsonPropertyName("total_ask_size")]
        public decimal TotalAskQuantity { get; set; }
        /// <summary>
        /// ["<c>total_bid_size</c>"] Total bid quantity
        /// </summary>
        [JsonPropertyName("total_bid_size")]
        public decimal TotalBidQuantity { get; set; }
        /// <summary>
        /// ["<c>orderbook_units</c>"] Entries
        /// </summary>
        [JsonPropertyName("orderbook_units")]
        public UpbitOrderBookEntry[] Entries { get; set; } = [];
        /// <summary>
        /// ["<c>level</c>"] Level
        /// </summary>
        [JsonPropertyName("level")]
        public decimal Level { get; set; }
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    public record UpbitOrderBookEntry
    {
        /// <summary>
        /// ["<c>ask_price</c>"] Ask price
        /// </summary>
        [JsonPropertyName("ask_price")]
        public decimal AskPrice { get; set; }
        /// <summary>
        /// ["<c>bid_price</c>"] Bid price
        /// </summary>
        [JsonPropertyName("bid_price")]
        public decimal BidPrice { get; set; }
        /// <summary>
        /// ["<c>ask_size</c>"] Ask quantity
        /// </summary>
        [JsonPropertyName("ask_size")]
        public decimal AskQuantity { get; set; }
        /// <summary>
        /// ["<c>bid_size</c>"] Bid quantity
        /// </summary>
        [JsonPropertyName("bid_size")]
        public decimal BidQuantity { get; set; }
    }


}
