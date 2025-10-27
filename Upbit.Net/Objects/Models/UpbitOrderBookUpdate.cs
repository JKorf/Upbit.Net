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
        /// Symbol name
        /// </summary>
        [JsonPropertyName("code")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Total ask quantity
        /// </summary>
        [JsonPropertyName("total_ask_size")]
        public decimal TotalAskQuantity { get; set; }
        /// <summary>
        /// Total bid quantity
        /// </summary>
        [JsonPropertyName("total_bid_size")]
        public decimal TotalBidQuantity { get; set; }
        /// <summary>
        /// Entries
        /// </summary>
        [JsonPropertyName("orderbook_units")]
        public UpbitOrderBookEntry[] Entries { get; set; } = [];
        /// <summary>
        /// Level
        /// </summary>
        [JsonPropertyName("level")]
        public decimal Level { get; set; }
    }
}
