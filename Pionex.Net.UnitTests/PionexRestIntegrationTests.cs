using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Pionex.Net.Clients;
using Pionex.Net.Objects.Options;
using CryptoExchange.Net.Objects.Errors;
using System.Threading;
using System.Collections.Generic;

namespace Pionex.Net.UnitTests
{
    [NonParallelizable]
    public class PionexRestIntegrationTests : RestIntegrationTest<PionexRestClient>
    {
        public override bool Run { get; set; } = false;

        public override PionexRestClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new PionexRestClient(null, loggerFactory, Options.Create(new PionexRestOptions
            {
                AutoTimestamp = false,
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new PionexCredentials(key, sec) : null
            }));
        }

        [Test]
        public async Task TestErrorResponseParsing()
        {
            if (!ShouldRun())
                return;

            var result = await CreateClient().SpotApi.ExchangeData.GetTickersAsync("TSTTST", default);

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error.ErrorType, Is.EqualTo(ErrorType.UnknownSymbol));
            Assert.That(result.Error.ErrorCode, Is.EqualTo("MARKET_INVALID_SYMBOL"));
        }

        [Test]
        public async Task TestSpotAccount()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.SpotApi.Account.GetBalancesAsync(CancellationToken.None), true, "data.balances");
            await RunAndCheckResult(client => client.SpotApi.Account.GetFullBalancesAsync(CancellationToken.None), true);
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestSpotExchangeData()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetSymbolsAsync(default, default, CancellationToken.None), false, "data.symbols");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetRecentTradesAsync("ETH_USDC", default, CancellationToken.None), false, "data.trades");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetOrderBookAsync("ETH_USDC", default, CancellationToken.None), false, "data");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetTickersAsync(default, default, CancellationToken.None), false, "data.tickers");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetBookTickersAsync(default, default, CancellationToken.None), false, "data.tickers");
            await RunAndCheckResult(warnings, client => client.SpotApi.ExchangeData.GetKlinesAsync("ETH_USDC", Enums.KlineInterval.OneHour, default, default, CancellationToken.None), false, "data.klines");
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }

        [Test]
        public async Task TestSpotTrading()
        {
            var warnings = new List<Exception>();
            await RunAndCheckResult(warnings, client => client.SpotApi.Trading.GetOpenOrdersAsync(default, CancellationToken.None), true, "data.orders");
            await RunAndCheckResult(warnings, client => client.SpotApi.Trading.GetOrdersAsync("ETH_USDC", default, default, default, CancellationToken.None), true, "data.orders");
            await RunAndCheckResult(warnings, client => client.SpotApi.Trading.GetUserTradesAsync("ETH_USDC", default, default, CancellationToken.None), true, "data.fills", ignoreProperties: ["feeType"]);
            foreach (var warning in warnings)
                Assert.Warn(warning.Message);
        }
    }
}
