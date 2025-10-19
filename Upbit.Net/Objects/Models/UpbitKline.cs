using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Kline info
    /// </summary>
    public record UpbitKline
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Candle date time utc
        /// </summary>
        [JsonPropertyName("candle_date_time_utc")]
        public DateTime OpenTime { get; set; }
        /// <summary>
        /// Candle date time kst
        /// </summary>
        [JsonPropertyName("candle_date_time_kst")]
        public DateTime OpenTimeKst { get; set; }
        /// <summary>
        /// Opening price
        /// </summary>
        [JsonPropertyName("opening_price")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// High price
        /// </summary>
        [JsonPropertyName("high_price")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// Low price
        /// </summary>
        [JsonPropertyName("low_price")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// Close Price
        /// </summary>
        [JsonPropertyName("trade_price")]
        public decimal ClosePrice { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// QuoteAssetVolume
        /// </summary>
        [JsonPropertyName("candle_acc_trade_price")]
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// Base asset volume
        /// </summary>
        [JsonPropertyName("candle_acc_trade_volume")]
        public decimal Volume { get; set; }
    }


}
