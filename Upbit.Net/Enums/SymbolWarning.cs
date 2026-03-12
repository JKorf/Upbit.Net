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
        /// ["<c>NONE</c>"] No warning
        /// </summary>
        [Map("NONE")]
        None,
        /// <summary>
        /// ["<c>CAUTION</c>"] Caution
        /// </summary>
        [Map("CAUTION")]
        Caution
    }
}
