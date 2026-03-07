using System;
using System.Text.Json.Serialization;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Order book info
    /// </summary>
    public record UpbitOrderBookUpdate: UpbitSocketUpdate
    {
        /// <summary>
        /// ["<c>code</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("code")]
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
}
