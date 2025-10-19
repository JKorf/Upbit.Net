using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Upbit.Net.Enums;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Trade info
    /// </summary>
    public record UpbitTrade
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Date
        /// </summary>
        [JsonPropertyName("trade_date_utc")]
        public string TimestampDate { get; set; } = string.Empty;
        /// <summary>
        /// Time
        /// </summary>
        [JsonPropertyName("trade_time_utc")]
        public string TimestampTime { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Trade price
        /// </summary>
        [JsonPropertyName("trade_price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Trade quantity
        /// </summary>
        [JsonPropertyName("trade_volume")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// Previous closing price
        /// </summary>
        [JsonPropertyName("prev_closing_price")]
        public decimal PrevClosingPrice { get; set; }
        /// <summary>
        /// Change price
        /// </summary>
        [JsonPropertyName("change_price")]
        public decimal ChangePrice { get; set; }
        /// <summary>
        /// Order side
        /// </summary>
        [JsonPropertyName("ask_bid")]
        public OrderSide OrderSide { get; set; }
        /// <summary>
        /// Sequential id
        /// </summary>
        [JsonPropertyName("sequential_id")]
        public long SequentialId { get; set; }
    }


}
