using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Upbit.Net.Enums
{
    /// <summary>
    /// Symbol status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolStatus>))]
    public enum SymbolStatus
    {
        /// <summary>
        /// Preview
        /// </summary>
        [Map("PREVIEW")]
        Preview,
        /// <summary>
        /// Active
        /// </summary>
        [Map("ACTIVE")]
        Active,
        /// <summary>
        /// Delisted
        /// </summary>
        [Map("DELISTED")]
        Delisted
    }
}
