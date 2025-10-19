using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Trading symbol
    /// </summary>
    public record UpbitSymbol
    {
        /// <summary>
        /// Symbol
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Name in Korean
        /// </summary>
        [JsonPropertyName("korean_name")]
        public string NameKorean { get; set; } = string.Empty;
        /// <summary>
        /// Name in English
        /// </summary>
        [JsonPropertyName("english_name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Market event
        /// </summary>
        [JsonPropertyName("market_event")]
        public UpbitSymbolEvent? Event { get; set; }
    }

    /// <summary>
    /// Event
    /// </summary>
    public record UpbitSymbolEvent
    {
        /// <summary>
        /// Whether the pair has been designated as an “Investment Caution” item under Upbit’s market alert system
        /// </summary>
        [JsonPropertyName("warning")]
        public bool Warning { get; set; }
        /// <summary>
        /// Alert info
        /// </summary>
        [JsonPropertyName("caution")]
        public UpbitSymbolAlerts Caution { get; set; } = null!;
    }

    /// <summary>
    /// Alerts
    /// </summary>
    public record UpbitSymbolAlerts
    {
        /// <summary>
        /// Price Surge/Drop Alert
        /// </summary>
        [JsonPropertyName("PRICE_FLUCTUATIONS")]
        public bool PriceFluctuations { get; set; }
        /// <summary>
        /// Trading Volume Surge Alert
        /// </summary>
        [JsonPropertyName("TRADING_VOLUME_SOARING")]
        public bool TradingVolumeSoaring { get; set; }
        /// <summary>
        /// Deposit Volume Surge Alert
        /// </summary>
        [JsonPropertyName("DEPOSIT_AMOUNT_SOARING")]
        public bool DepositQuantitySoaring { get; set; }
        /// <summary>
        /// Domestic and International Price Difference Alert
        /// </summary>
        [JsonPropertyName("GLOBAL_PRICE_DIFFERENCES")]
        public bool GlobalPriceDifferences { get; set; }
        /// <summary>
        /// Concentrated Trading by a Small Number of Accounts Alert
        /// </summary>
        [JsonPropertyName("CONCENTRATION_OF_SMALL_ACCOUNTS")]
        public bool ConcentrationOfSmallAccounts { get; set; }
    }


}
