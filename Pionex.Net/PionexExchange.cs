using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using CryptoExchange.Net.SharedApis;
using Pionex.Net.Converters;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Filters;

namespace Pionex.Net
{
    /// <summary>
    /// Pionex exchange information and configuration
    /// </summary>
    public static class PionexExchange
    {
        internal static JsonSerializerOptions _serializerContext = SerializerOptions.WithConverters(JsonSerializerContextCache.GetOrCreate<PionexSourceGenerationContext>());
        internal static ParameterSerializationSettings _parameterSerializationSettings = new ParameterSerializationSettings
        {
            Sort = true,
            Decimal = DecimalSerialization.String,
        };

        /// <summary>
        /// Platform metadata
        /// </summary>
        public static PlatformInfo Metadata { get; } = new PlatformInfo(
                "Pionex",
                "Pionex",
                "https://raw.githubusercontent.com/JKorf/Pionex.Net/main/Pionex.Net/Icon/icon.png",
                "https://www.pionex.com/",
                ["https://www.pionex.com/docs/api-docs"],
                PlatformType.CryptoCurrencyExchange,
                CentralizationType.Centralized,
                PionexEnvironment.All
                );

        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "Pionex";

        /// <summary>
        /// Display name
        /// </summary>
        public static string DisplayName => "Pionex";

        /// <summary>
        /// Url to exchange image
        /// </summary>
        public static string ImageUrl { get; } = "TODO";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://www.pionex.com/";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "https://www.pionex.com/docs/api-docs"
            };

        /// <summary>
        /// Type of exchange
        /// </summary>
        public static ExchangeType Type { get; } = ExchangeType.CEX;

        /// <summary>
        /// Aliases for Pionex assets
        /// </summary>
        public static AssetAliasConfiguration AssetAliases { get; } = new AssetAliasConfiguration
        {
            Aliases = [
                new AssetAlias("USDT", SharedSymbol.UsdOrStable.ToUpperInvariant(), AliasType.OnlyToExchange)
            ]
        };

        /// <summary>
        /// Format a base and quote asset to an Pionex recognized symbol 
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

            return baseAsset + "_" + quoteAsset;
        }

        /// <summary>
        /// Rate limiter configuration for the Pionex API
        /// </summary>
        public static PionexRateLimiters RateLimiter { get; } = new PionexRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the Pionex API
    /// </summary>
    public class PionexRateLimiters
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
        internal PionexRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        private void Initialize()
        {
            Pionex = new RateLimitGate("Pionex")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Request), 10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding)); // 10 per 1 seconds shared per IP
            Pionex.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            Pionex.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }


        internal IRateLimitGate Pionex { get; private set; }

    }
}
