using System;
using System.Text.Json.Serialization;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Kline info
    /// </summary>
    public record UpbitKline
    {
        /// <summary>
        /// ["<c>market</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>candle_date_time_utc</c>"] Candle date time utc
        /// </summary>
        [JsonPropertyName("candle_date_time_utc")]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// ["<c>candle_date_time_kst</c>"] Candle date time kst
        /// </summary>
        [JsonPropertyName("candle_date_time_kst")]
        public DateTime OpenTimeKst { get; set; }
        /// <summary>
        /// ["<c>opening_price</c>"] Opening price
        /// </summary>
        [JsonPropertyName("opening_price")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// ["<c>high_price</c>"] High price
        /// </summary>
        [JsonPropertyName("high_price")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// ["<c>low_price</c>"] Low price
        /// </summary>
        [JsonPropertyName("low_price")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// ["<c>trade_price</c>"] Close Price
        /// </summary>
        [JsonPropertyName("trade_price")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>candle_acc_trade_price</c>"] QuoteAssetVolume
        /// </summary>
        [JsonPropertyName("candle_acc_trade_price")]
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// ["<c>candle_acc_trade_volume</c>"] Base asset volume
        /// </summary>
        [JsonPropertyName("candle_acc_trade_volume")]
        public decimal Volume { get; set; }
    }


}
