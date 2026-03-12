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
        /// ["<c>RISE</c>"] Rise
        /// </summary>
        [Map("RISE")]
        Rise,
        /// <summary>
        /// ["<c>EVEN</c>"] Even
        /// </summary>
        [Map("EVEN")]
        Even,
        /// <summary>
        /// ["<c>FALL</c>"] Fall
        /// </summary>
        [Map("FALL")]
        Fall
    }
}
