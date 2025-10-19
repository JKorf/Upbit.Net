using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Upbit.Net.Enums
{
    /// <summary>
    /// Stream type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<StreamType>))]
    public enum StreamType
    {
        /// <summary>
        /// Snapshot
        /// </summary>
        [Map("SNAPSHOT")]
        Snapshot,
        /// <summary>
        /// Update
        /// </summary>
        [Map("REALTIME")]
        Update
    }
}
