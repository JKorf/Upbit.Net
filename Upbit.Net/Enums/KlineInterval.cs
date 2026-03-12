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
        /// ["<c>1s</c>"] One second
        /// </summary>
        [Map("1s")]
        OneSecond = 1,
        /// <summary>
        /// ["<c>1m</c>"] One minute
        /// </summary>
        [Map("1m")]
        OneMinute = 60,
        /// <summary>
        /// ["<c>3m</c>"] Three minutes
        /// </summary>
        [Map("3m")]
        ThreeMinutes = 60 * 3,
        /// <summary>
        /// ["<c>5m</c>"] Five minutes
        /// </summary>
        [Map("5m")]
        FiveMinutes = 60 * 5,
        /// <summary>
        /// ["<c>10m</c>"] Ten minutes
        /// </summary>
        [Map("10m")]
        TenMinutes = 60 * 10,
        /// <summary>
        /// ["<c>15m</c>"] Fifteen minutes
        /// </summary>
        [Map("15m")]
        FifteenMinutes = 60 * 15,
        /// <summary>
        /// ["<c>30m</c>"] Thirty minutes
        /// </summary>
        [Map("30m")]
        ThirtyMinutes = 60 * 30,
        /// <summary>
        /// ["<c>60m</c>"] One hour
        /// </summary>
        [Map("60m")]
        OneHour = 60 * 60,
        /// <summary>
        /// ["<c>240m</c>"] Four hours
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
