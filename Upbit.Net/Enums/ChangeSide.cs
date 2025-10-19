using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Upbit.Net.Enums
{
    /// <summary>
    /// Change side
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ChangeSide>))]
    public enum ChangeSide
    {
        /// <summary>
        /// Rise
        /// </summary>
        [Map("RISE")]
        Rise,
        /// <summary>
        /// Fall
        /// </summary>
        [Map("FALL")]
        Fall
    }
}
