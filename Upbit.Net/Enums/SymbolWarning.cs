using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Upbit.Net.Enums
{
    /// <summary>
    /// Warning status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolWarning>))]
    public enum SymbolWarning
    {
        /// <summary>
        /// No warning
        /// </summary>
        [Map("NONE")]
        None,
        /// <summary>
        /// Caution
        /// </summary>
        [Map("CAUTION")]
        Caution
    }
}
