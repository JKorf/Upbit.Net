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
        /// ["<c>PREVIEW</c>"] Preview
        /// </summary>
        [Map("PREVIEW")]
        Preview,
        /// <summary>
        /// ["<c>ACTIVE</c>"] Active
        /// </summary>
        [Map("ACTIVE")]
        Active,
        /// <summary>
        /// ["<c>PREDELISTING</c>"] Pre delisting
        /// </summary>
        [Map("PREDELISTING")]
        PreDelisting,
        /// <summary>
        /// ["<c>DELISTED</c>"] Delisted
        /// </summary>
        [Map("DELISTED")]
        Delisted
    }
}
