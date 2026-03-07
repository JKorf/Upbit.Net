using System;
using System.Text.Json.Serialization;
using Upbit.Net.Enums;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Trade info
    /// </summary>
    public record UpbitTrade
    {
        /// <summary>
        /// ["<c>market</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>trade_date_utc</c>"] Date
        /// </summary>
        [JsonPropertyName("trade_date_utc")]
        public string TimestampDate { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>trade_time_utc</c>"] Time
        /// </summary>
        [JsonPropertyName("trade_time_utc")]
        public string TimestampTime { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>trade_price</c>"] Trade price
        /// </summary>
        [JsonPropertyName("trade_price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>trade_volume</c>"] Trade quantity
        /// </summary>
        [JsonPropertyName("trade_volume")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>prev_closing_price</c>"] Previous closing price
        /// </summary>
        [JsonPropertyName("prev_closing_price")]
        public decimal PrevClosingPrice { get; set; }
        /// <summary>
        /// ["<c>change_price</c>"] Change price
        /// </summary>
        [JsonPropertyName("change_price")]
        public decimal ChangePrice { get; set; }
        /// <summary>
        /// ["<c>ask_bid</c>"] Order side
        /// </summary>
        [JsonPropertyName("ask_bid")]
        public OrderSide OrderSide { get; set; }
        /// <summary>
        /// ["<c>sequential_id</c>"] Sequential id
        /// </summary>
        [JsonPropertyName("sequential_id")]
        public long SequentialId { get; set; }
    }


}
