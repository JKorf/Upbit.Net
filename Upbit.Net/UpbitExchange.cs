using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using CryptoExchange.Net.SharedApis;
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
        /// Platform metadata
        /// </summary>
        public static PlatformInfo Metadata { get; } = new PlatformInfo(
                "Upbit",
                "Upbit",
                "https://raw.githubusercontent.com/JKorf/Upbit.Net/main/Upbit.Net/Icon/icon.png",
                "https://www.upbit.com",
                ["https://global-docs.upbit.com/reference"],
                PlatformType.CryptoCurrencyExchange,
                CentralizationType.Centralized
                );

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
        public static string ImageUrl { get; } = "https://raw.githubusercontent.com/JKorf/Upbit.Net/main/Upbit.Net/Icon/icon.png";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://www.upbit.com";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "https://global-docs.upbit.com/reference"
            };

        /// <summary>
        /// Type of exchange
        /// </summary>
        public static ExchangeType Type { get; } = ExchangeType.CEX;

        internal static JsonSerializerOptions _serializerContext = SerializerOptions.WithConverters(JsonSerializerContextCache.GetOrCreate<UpbitSourceGenerationContext>());

        /// <summary>
        /// Aliases for Upbit assets
        /// </summary>
        public static AssetAliasConfiguration AssetAliases { get; } = new AssetAliasConfiguration
        {
            Aliases =
            [
                new AssetAlias("USDT", SharedSymbol.UsdOrStable.ToUpperInvariant(), AliasType.OnlyToExchange)
            ]
        };

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
            baseAsset = AssetAliases.CommonToExchangeName(baseAsset.ToUpperInvariant());
            quoteAsset = AssetAliases.CommonToExchangeName(quoteAsset.ToUpperInvariant());

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
            Upbit = new RateLimitGate("Endpoint");

            RestTicker = new RateLimitGate("Ticker")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Connection), 10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));

            Socket = new RateLimitGate("Socket")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Connection), 4, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding))
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Request), 4, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));

            Upbit.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            Upbit.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            RestTicker.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            RestTicker.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
            Socket.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            Socket.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }


        internal IRateLimitGate Upbit { get; private set; }
        internal IRateLimitGate Socket { get; private set; }
        internal IRateLimitGate RestTicker { get; private set; }

    }
}
