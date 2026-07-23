using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;
using Pionex.Net.Clients;
using Pionex.Net.Objects.Models;
using Pionex.Net.Objects.Options;

namespace Pionex.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateSubscriptions()
        {
            var client = new PionexSocketClient(opts =>
            {
                opts.ApiCredentials = new PionexCredentials("123", "456");
            });
            var tester = new SocketSubscriptionValidator<PionexSocketClient>(client, "Subscriptions/Spot", "wss://ws.pionex.com", "data");
            await tester.ValidateAsync<PionexTrade[]>((client, handler) => client.SpotApi.SubscribeToTradeUpdatesAsync("ETH_USDT", handler), "Trades");
            await tester.ValidateAsync<PionexOrderBook>((client, handler) => client.SpotApi.SubscribeToOrderBookUpdatesAsync("ETH_USDT", 5, handler), "Book");
            await tester.ValidateAsync<PionexBalance[]>((client, handler) => client.SpotApi.SubscribeToBalanceUpdatesAsync(handler), "Balances", nestedJsonProperty: "data.balances", ignoreProperties: ["timestamp"]);
            await tester.ValidateAsync<PionexOrder>((client, handler) => client.SpotApi.SubscribeToOrderUpdatesAsync("ETH_USDT", handler), "Order");
            await tester.ValidateAsync<PionexUserTrade>((client, handler) => client.SpotApi.SubscribeToUserTradeUpdatesAsync("ETH_USDT", handler), "UserTrade");
        }

        [TestCase]
        public async Task ValidateConcurrentSpotSubscriptions()
        {
            var logger = new LoggerFactory();
            logger.AddProvider(new TraceLoggerProvider());

            var client = new PionexSocketClient(Options.Create(new PionexSocketOptions
            {
                ApiCredentials = new PionexCredentials("123", "456"),
                OutputOriginalData = true
            }), logger);

            var tester = new SocketSubscriptionValidator<PionexSocketClient>(client, "Subscriptions/Spot", "wss://ws.pionex.com/wsPub", "data");
            await tester.ValidateConcurrentAsync<PionexTrade[]>(
                (client, handler) => client.SpotApi.SubscribeToTradeUpdatesAsync("BTC_USDT", handler),
                (client, handler) => client.SpotApi.SubscribeToTradeUpdatesAsync("ETH_USDT", handler),
                "Concurrent");
        }
    }
}
