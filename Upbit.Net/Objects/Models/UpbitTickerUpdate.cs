using System;
using System.Text.Json.Serialization;
using Upbit.Net.Enums;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Price ticker info
    /// </summary>
    public record UpbitTickerUpdate: UpbitSocketUpdate
    {
        /// <summary>
        /// ["<c>code</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("code")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>trade_date</c>"] Trade date
        /// </summary>
        [JsonPropertyName("trade_date")]
        public string TradeDate { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>trade_time</c>"] Trade time
        /// </summary>
        [JsonPropertyName("trade_time")]
        public string TradeTime { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>trade_timestamp</c>"] Trade timestamp
        /// </summary>
        [JsonPropertyName("trade_timestamp")]
        public DateTime TradeTimestamp { get; set; }
        /// <summary>
        /// ["<c>opening_price</c>"] Open price 24h ago
        /// </summary>
        [JsonPropertyName("opening_price")]
        public decimal OpenPrice { get; set; }
        /// <summary>
        /// ["<c>high_price</c>"] High price last 24h
        /// </summary>
        [JsonPropertyName("high_price")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// ["<c>low_price</c>"] Low price last 24h
        /// </summary>
        [JsonPropertyName("low_price")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// ["<c>trade_price</c>"] Last trade price
        /// </summary>
        [JsonPropertyName("trade_price")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// ["<c>prev_closing_price</c>"] Prev closing price
        /// </summary>
        [JsonPropertyName("prev_closing_price")]
        public decimal PrevClosingPrice { get; set; }
        /// <summary>
        /// ["<c>change</c>"] Change
        /// </summary>
        [JsonPropertyName("change")]
        public string Change { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>change_price</c>"] Price change
        /// </summary>
        [JsonPropertyName("change_price")]
        public decimal ChangePriceAbs { get; set; }
        /// <summary>
        /// ["<c>change_rate</c>"] Absolute change rate since 24h
        /// </summary>
        [JsonPropertyName("change_rate")]
        public decimal ChangeRateAbs { get; set; }
        /// <summary>
        /// ["<c>signed_change_price</c>"] Change rate since 24h
        /// </summary>
        [JsonPropertyName("signed_change_price")]
        public decimal ChangePrice { get; set; }
        /// <summary>
        /// ["<c>signed_change_rate</c>"] Signed change rate
        /// </summary>
        [JsonPropertyName("signed_change_rate")]
        public decimal ChangeRate { get; set; }
        /// <summary>
        /// ["<c>acc_bid_volume</c>"] Sell volume
        /// </summary>
        [JsonPropertyName("acc_bid_volume")]
        public decimal SellVolume { get; set; }
        /// <summary>
        /// ["<c>acc_ask_volume</c>"] Buy volume
        /// </summary>
        [JsonPropertyName("acc_ask_volume")]
        public decimal BuyVolume { get; set; }
        /// <summary>
        /// ["<c>trade_volume</c>"] Last trade volume
        /// </summary>
        [JsonPropertyName("trade_volume")]
        public decimal LastVolume { get; set; }
        /// <summary>
        /// ["<c>acc_trade_price</c>"] Accumulated trade amount since UTC 00:00
        /// </summary>
        [JsonPropertyName("acc_trade_price")]
        public decimal QuoteVolume { get; set; }
        /// <summary>
        /// ["<c>acc_trade_price_24h</c>"] Accumulated trade amount over the past 24 hours.
        /// </summary>
        [JsonPropertyName("acc_trade_price_24h")]
        public decimal QuoteVolume24h { get; set; }
        /// <summary>
        /// ["<c>acc_trade_volume</c>"] Accumulated trade volume since UTC 00:00
        /// </summary>
        [JsonPropertyName("acc_trade_volume")]
        public decimal Volume { get; set; }
        /// <summary>
        /// ["<c>acc_trade_volume_24h</c>"] Accumulated trade volume over last 24 hours
        /// </summary>
        [JsonPropertyName("acc_trade_volume_24h")]
        public decimal Volume24h { get; set; }
        /// <summary>
        /// ["<c>highest_52_week_price</c>"] Highest price last 52 weeks
        /// </summary>
        [JsonPropertyName("highest_52_week_price")]
        public decimal HighPrice52Weeks { get; set; }
        /// <summary>
        /// ["<c>highest_52_week_date</c>"] Highest price 52 week date
        /// </summary>
        [JsonPropertyName("highest_52_week_date")]
        public string HighPrice52WeeksDate { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>lowest_52_week_price</c>"] Lowest price last 52 weeks
        /// </summary>
        [JsonPropertyName("lowest_52_week_price")]
        public decimal LowPrice52Weeks { get; set; }
        /// <summary>
        /// ["<c>lowest_52_week_date</c>"] Lowest price 52 week date
        /// </summary>
        [JsonPropertyName("lowest_52_week_date")]
        public string LowPrice52WeeksDate { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>timestamp</c>"] Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>market_state</c>"] Symbol status
        /// </summary>
        [JsonPropertyName("market_state")]
        public SymbolStatus SymbolStatus { get; set; }
        /// <summary>
        /// ["<c>is_trading_suspended</c>"] Is trading suspended
        /// </summary>
        [JsonPropertyName("is_trading_suspended")]
        public bool TradingSuspended { get; set; }
        /// <summary>
        /// ["<c>market_warning</c>"] Warning
        /// </summary>
        [JsonPropertyName("market_warning")]
        public SymbolWarning Warning { get; set; }

    }


}
