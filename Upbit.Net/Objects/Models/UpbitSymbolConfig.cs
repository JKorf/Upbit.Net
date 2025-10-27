using System.Text.Json.Serialization;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Symbol config
    /// </summary>
    public record UpbitSymbolConfig
    {
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Quote asset
        /// </summary>
        [JsonPropertyName("quote_currency")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// Tick quantity/ price step
        /// </summary>
        [JsonPropertyName("tick_size")]
        public decimal TickQuantity { get; set; }
        /// <summary>
        /// Supported book aggregate levels for KRW markets
        /// </summary>
        [JsonPropertyName("supported_levels")]
        public decimal[] SupportedBookLevels { get; set; } = [];
    }


}
