using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using Pionex.Net.Clients;
using Pionex.Net.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pionex.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateSpotAccountCalls()
        {
            var client = new PionexRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new PionexCredentials("123", "456");
            });
            var tester = new RestRequestValidator<PionexRestClient>(client, "Endpoints/Spot/Account", "https://api.pionex.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.SpotApi.Account.GetBalancesAsync(), "GetBalances", nestedJsonProperty: "data.balances");
            await tester.ValidateAsync(client => client.SpotApi.Account.GetFullBalancesAsync(), "GetFullBalances", nestedJsonProperty: "data");
        }

        [Test]
        public async Task ValidateSpotExchangeDataCalls()
        {
            var client = new PionexRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new PionexCredentials("123", "456");
            });
            var tester = new RestRequestValidator<PionexRestClient>(client, "Endpoints/Spot/ExchangeData", "https://api.pionex.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetSymbolsAsync(), "GetSymbols", nestedJsonProperty: "data.symbols");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetRecentTradesAsync("ETH_USDT"), "GetRecentTrades", nestedJsonProperty: "data.trades");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetOrderBookAsync("ETH_USDT"), "GetOrderBook", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetTickersAsync(), "GetTickers", nestedJsonProperty: "data.tickers");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetBookTickersAsync(), "GetBookTickers", nestedJsonProperty: "data.tickers");
            await tester.ValidateAsync(client => client.SpotApi.ExchangeData.GetKlinesAsync("ETH_USDT", KlineInterval.OneMinute), "GetKlines", nestedJsonProperty: "data.klines");
        }

        [Test]
        public async Task ValidateSpotTradingCalls()
        {
            var client = new PionexRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new PionexCredentials("123", "456");
            });
            var tester = new RestRequestValidator<PionexRestClient>(client, "Endpoints/Spot/Trading", "https://api.pionex.com", IsAuthenticated);
            await tester.ValidateAsync(client => client.SpotApi.Trading.PlaceOrderAsync("ETH_USDT", OrderSide.Buy, OrderType.Limit), "PlaceOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOrderAsync(123), "GetOrder", nestedJsonProperty: "data");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelOrderAsync("ETH_USDT", 123), "CancelOrder");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOpenOrdersAsync("ETH_USDT"), "GetOpenOrders", nestedJsonProperty: "data.orders");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOrdersAsync("ETH_USDT"), "GetOrders", nestedJsonProperty: "data.orders");
            await tester.ValidateAsync(client => client.SpotApi.Trading.CancelAllOrdersAsync("ETH_USDT"), "CancelAllOrders");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetUserTradesAsync("ETH_USDT"), "GetUserTrades", nestedJsonProperty: "data.fills");
            await tester.ValidateAsync(client => client.SpotApi.Trading.GetOrderTradesAsync(123), "GetOrderTrades", nestedJsonProperty: "data.fills");
        }


        private bool IsAuthenticated(IHttpResult result)
        {
            return result.RequestHeaders?.Contains("PIONEX-SIGNATURE") == true;
        }
    }
}
