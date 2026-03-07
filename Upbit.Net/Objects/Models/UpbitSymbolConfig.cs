using System.Text.Json.Serialization;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Symbol config
    /// </summary>
    public record UpbitSymbolConfig
    {
        /// <summary>
        /// ["<c>market</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>quote_currency</c>"] Quote asset
        /// </summary>
        [JsonPropertyName("quote_currency")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>tick_size</c>"] Tick quantity/ price step
        /// </summary>
        [JsonPropertyName("tick_size")]
        public decimal TickQuantity { get; set; }
        /// <summary>
        /// ["<c>supported_levels</c>"] Supported book aggregate levels for KRW markets
        /// </summary>
        [JsonPropertyName("supported_levels")]
        public decimal[] SupportedBookLevels { get; set; } = [];
    }


}
