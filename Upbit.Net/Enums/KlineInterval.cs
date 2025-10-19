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
        OneSecond = 1,
        /// <summary>
        /// One minute
        /// </summary>
        OneMinute = 60,
        /// <summary>
        /// Three minutes
        /// </summary>
        ThreeMinutes = 60 * 3,
        /// <summary>
        /// Five minutes
        /// </summary>
        FiveMinutes = 60 * 5,
        /// <summary>
        /// Ten minutes
        /// </summary>
        TenMinutes = 60 * 10,
        /// <summary>
        /// Fifteen minutes
        /// </summary>
        FifteenMinutes = 60 * 15,
        /// <summary>
        /// Thirty minutes
        /// </summary>
        ThirtyMinutes = 60 * 30,
        /// <summary>
        /// One hour
        /// </summary>
        OneHour = 60 * 60,
        /// <summary>
        /// Four hours
        /// </summary>
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
