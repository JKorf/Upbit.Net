using System;
using System.Text.Json.Serialization;
using Upbit.Net.Enums;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Price ticker info
    /// </summary>
    public record UpbitTicker
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Trade date
        /// </summary>
        [JsonPropertyName("trade_date")]
        public string? TradeDate { get; set; }
        /// <summary>
        /// Trade time
        /// </summary>
        [JsonPropertyName("trade_time")]
        public string? TradeTime { get; set; }
        /// <summary>
        /// Trade date kst
        /// </summary>
        [JsonPropertyName("trade_date_kst")]
        public string? TradeDateKst { get; set; }
        /// <summary>
        /// Trade time kst
        /// </summary>
        [JsonPropertyName("trade_time_kst")]
        public string? TradeTimeKst { get; set; }
        /// <summary>
        /// Trade timestamp
        /// </summary>
        [JsonPropertyName("trade_timestamp")]
        public DateTime? TradeTimestamp { get; set; }
        /// <summary>
        /// Open price 24h ago
        /// </summary>
        [JsonPropertyName("opening_price")]
        public decimal? OpenPrice { get; set; }
        /// <summary>
        /// High price last 24h
        /// </summary>
        [JsonPropertyName("high_price")]
        public decimal? HighPrice { get; set; }
        /// <summary>
        /// Low price last 24h
        /// </summary>
        [JsonPropertyName("low_price")]
        public decimal? LowPrice { get; set; }
        /// <summary>
        /// Last trade price
        /// </summary>
        [JsonPropertyName("trade_price")]
        public decimal? LastPrice { get; set; }
        /// <summary>
        /// Prev closing price
        /// </summary>
        [JsonPropertyName("prev_closing_price")]
        public decimal? PrevClosingPrice { get; set; }
        /// <summary>
        /// Change
        /// </summary>
        [JsonPropertyName("change")]
        public ChangeSide Change { get; set; }
        /// <summary>
        /// Price change
        /// </summary>
        [JsonPropertyName("change_price")]
        public decimal ChangePriceAbs { get; set; }
        /// <summary>
        /// Absolute change rate since 24h
        /// </summary>
        [JsonPropertyName("change_rate")]
        public decimal ChangeRateAbs { get; set; }
        /// <summary>
        /// Change rate since 24h
        /// </summary>
        [JsonPropertyName("signed_change_price")]
        public decimal ChangePrice { get; set; }
        /// <summary>
        /// Signed change rate
        /// </summary>
        [JsonPropertyName("signed_change_rate")]
        public decimal ChangeRate { get; set; }
        /// <summary>
        /// Last trade volume
        /// </summary>
        [JsonPropertyName("trade_volume")]
        public decimal? LastVolume { get; set; }
        /// <summary>
        /// Accumulated trade volume in quote asset since UTC 00:00
        /// </summary>
        [JsonPropertyName("acc_trade_price")]
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// Accumulated trade volume in quote asset over the past 24 hours.
        /// </summary>
        [JsonPropertyName("acc_trade_price_24h")]
        public decimal QuoteVolume24h { get; set; }
        /// <summary>
        /// Accumulated trade volume in base asset since UTC 00:00
        /// </summary>
        [JsonPropertyName("acc_trade_volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// Accumulated trade volume in base asset over last 24 hours
        /// </summary>
        [JsonPropertyName("acc_trade_volume_24h")]
        public decimal Volume24h { get; set; }
        /// <summary>
        /// Highest price last 52 weeks
        /// </summary>
        [JsonPropertyName("highest_52_week_price")]
        public decimal? HighPrice52Weeks { get; set; }
        /// <summary>
        /// Highest price 52 week date
        /// </summary>
        [JsonPropertyName("highest_52_week_date")]
        public string? HighPrice52WeeksDate { get; set; }
        /// <summary>
        /// Lowest price last 52 weeks
        /// </summary>
        [JsonPropertyName("lowest_52_week_price")]
        public decimal? LowPrice52Weeks { get; set; }
        /// <summary>
        /// Lowest price 52 week date
        /// </summary>
        [JsonPropertyName("lowest_52_week_date")]
        public string? LowPrice52WeeksDate { get; set; }
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }


}
