using System;
using System.Text.Json.Serialization;
using Upbit.Net.Enums;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Trade info
    /// </summary>
    public record UpbitTradeUpdate : UpbitSocketUpdate
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
        /// ["<c>trade_timestamp</c>"] Trade timestamp
        /// </summary>
        [JsonPropertyName("trade_timestamp")]
        public DateTime TradeTime { get; set; }
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
        /// ["<c>change</c>"] Change side
        /// </summary>
        [JsonPropertyName("change")]
        public ChangeSide ChangeSide { get; set; }
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
        /// <summary>
        /// ["<c>best_bid_size</c>"] Best bid quantity
        /// </summary>
        [JsonPropertyName("best_bid_size")]
        public decimal BestBidQuantity { get; set; }
        /// <summary>
        /// ["<c>best_bid_price</c>"] Best bid price
        /// </summary>
        [JsonPropertyName("best_bid_price")]
        public decimal BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>best_ask_size</c>"] Best ask quantity
        /// </summary>
        [JsonPropertyName("best_ask_size")]
        public decimal BestAskQuantity { get; set; }
        /// <summary>
        /// ["<c>best_ask_price</c>"] Best ask price
        /// </summary>
        [JsonPropertyName("best_ask_price")]
        public decimal BestAskPrice { get; set; }
    }


}
