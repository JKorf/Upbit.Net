using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Order book info
    /// </summary>
    public record UpbitOrderBook
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("market")]
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
        public long Level { get; set; }
    }

    /// <summary>
    /// Order book entry
    /// </summary>
    public record UpbitOrderBookEntry
    {
        /// <summary>
        /// Ask price
        /// </summary>
        [JsonPropertyName("ask_price")]
        public decimal AskPrice { get; set; }
        /// <summary>
        /// Bid price
        /// </summary>
        [JsonPropertyName("bid_price")]
        public decimal BidPrice { get; set; }
        /// <summary>
        /// Ask quantity
        /// </summary>
        [JsonPropertyName("ask_size")]
        public decimal AskQuantity { get; set; }
        /// <summary>
        /// Bid quantity
        /// </summary>
        [JsonPropertyName("bid_size")]
        public decimal BidQuantity { get; set; }
    }


}
