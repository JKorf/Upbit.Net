using System.Text.Json.Serialization;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Trading symbol
    /// </summary>
    public record UpbitSymbol
    {
        /// <summary>
        /// ["<c>market</c>"] Symbol
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>korean_name</c>"] Name in Korean
        /// </summary>
        [JsonPropertyName("korean_name")]
        public string NameKorean { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>english_name</c>"] Name in English
        /// </summary>
        [JsonPropertyName("english_name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>market_event</c>"] Market event
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
        /// ["<c>warning</c>"] Whether the pair has been designated as an “Investment Caution” item under Upbit’s market alert system
        /// </summary>
        [JsonPropertyName("warning")]
        public bool Warning { get; set; }
        /// <summary>
        /// ["<c>caution</c>"] Alert info
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
        /// ["<c>PRICE_FLUCTUATIONS</c>"] Price Surge/Drop Alert
        /// </summary>
        [JsonPropertyName("PRICE_FLUCTUATIONS")]
        public bool PriceFluctuations { get; set; }
        /// <summary>
        /// ["<c>TRADING_VOLUME_SOARING</c>"] Trading Volume Surge Alert
        /// </summary>
        [JsonPropertyName("TRADING_VOLUME_SOARING")]
        public bool TradingVolumeSoaring { get; set; }
        /// <summary>
        /// ["<c>DEPOSIT_AMOUNT_SOARING</c>"] Deposit Volume Surge Alert
        /// </summary>
        [JsonPropertyName("DEPOSIT_AMOUNT_SOARING")]
        public bool DepositQuantitySoaring { get; set; }
        /// <summary>
        /// ["<c>GLOBAL_PRICE_DIFFERENCES</c>"] Domestic and International Price Difference Alert
        /// </summary>
        [JsonPropertyName("GLOBAL_PRICE_DIFFERENCES")]
        public bool GlobalPriceDifferences { get; set; }
        /// <summary>
        /// ["<c>CONCENTRATION_OF_SMALL_ACCOUNTS</c>"] Concentrated Trading by a Small Number of Accounts Alert
        /// </summary>
        [JsonPropertyName("CONCENTRATION_OF_SMALL_ACCOUNTS")]
        public bool ConcentrationOfSmallAccounts { get; set; }
    }


}
