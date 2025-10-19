using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using System.Collections.Generic;
using System.Text;
using CryptoExchange.Net.SharedApis;
using System.Text.Json.Serialization;
using Upbit.Net.Converters;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters;

namespace Upbit.Net
{
    /// <summary>
    /// Upbit exchange information and configuration
    /// </summary>
    public static class UpbitExchange
    {
        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "Upbit";

        /// <summary>
        /// Display name
        /// </summary>
        public static string DisplayName => "Upbit";

        /// <summary>
        /// Url to exchange image
        /// </summary>
        public static string ImageUrl { get; } = "TODO";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://www.upbit.com";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "https://docs.upbit.com/"
            };

        /// <summary>
        /// Type of exchange
        /// </summary>
        public static ExchangeType Type { get; } = ExchangeType.CEX;

        internal static JsonSerializerOptions _serializerContext = SerializerOptions.WithConverters(JsonSerializerContextCache.GetOrCreate<UpbitSourceGenerationContext>());

        /// <summary>
        /// Format a base and quote asset to an Upbit recognized symbol 
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            return $"{quoteAsset}-{baseAsset}";
        }

        /// <summary>
        /// Rate limiter configuration for the Upbit API
        /// </summary>
        public static UpbitRateLimiters RateLimiter { get; } = new UpbitRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the Upbit API
    /// </summary>
    public class UpbitRateLimiters
    {
        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent> RateLimitTriggered;
        /// <summary>
        /// Event when the rate limit is updated. Note that it's only updated when a request is send, so there are no specific updates when the current usage is decaying.
        /// </summary>
        public event Action<RateLimitUpdateEvent> RateLimitUpdated;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        internal UpbitRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        private void Initialize()
        {
            Upbit = new RateLimitGate("Upbit");
            Upbit.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            Upbit.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }


        internal IRateLimitGate Upbit { get; private set; }

    }
}
