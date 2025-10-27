using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Upbit.Net.Enums
{
    /// <summary>
    /// Kline interval
    /// </summary>
    [JsonConverter(typeof(EnumConverter<KlineInterval>))]
    public enum KlineInterval
    {
        /// <summary>
        /// One second
        /// </summary>
        [Map("1s")]
        OneSecond = 1,
        /// <summary>
        /// One minute
        /// </summary>
        [Map("1m")]
        OneMinute = 60,
        /// <summary>
        /// Three minutes
        /// </summary>
        [Map("3m")]
        ThreeMinutes = 60 * 3,
        /// <summary>
        /// Five minutes
        /// </summary>
        [Map("5m")]
        FiveMinutes = 60 * 5,
        /// <summary>
        /// Ten minutes
        /// </summary>
        [Map("10m")]
        TenMinutes = 60 * 10,
        /// <summary>
        /// Fifteen minutes
        /// </summary>
        [Map("15m")]
        FifteenMinutes = 60 * 15,
        /// <summary>
        /// Thirty minutes
        /// </summary>
        [Map("30m")]
        ThirtyMinutes = 60 * 30,
        /// <summary>
        /// One hour
        /// </summary>
        [Map("60m")]
        OneHour = 60 * 60,
        /// <summary>
        /// Four hours
        /// </summary>
        [Map("240m")]
        FourHours = 60 * 60 * 4,
        /// <summary>
        /// One day
        /// </summary>
        OneDay = 60 * 60 * 24,
        /// <summary>
        /// One week
        /// </summary>
        OneWeek = 60 * 60 * 24 * 7,
        /// <summary>
        /// One month
        /// </summary>
        OneMonth = 60 * 60 * 24 * 30,
        /// <summary>
        /// One year
        /// </summary>
        OneYear = 60 * 60 * 24 * 365
    }
}
