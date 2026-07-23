using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Pionex.Net.Clients;
using Pionex.Net.Objects.Options;

namespace Pionex.Net.UnitTests
{
    [NonParallelizable]
    internal class PionexSocketIntegrationTests : SocketIntegrationTest<PionexSocketClient>
    {
        public override bool Run { get; set; } = false;

        public PionexSocketIntegrationTests()
        {
        }

        public override PionexSocketClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new PionexSocketClient(Options.Create(new PionexSocketOptions
            {
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new PionexCredentials(key, sec) : null
            }), loggerFactory);
        }

        [TestCase]
        public async Task TestSubscriptions()
        {
            //await RunAndCheckUpdate<>((client, updateHandler) => client.SpotApi.Account.SubscribeToUserDataUpdatesAsync(default, default, default, default, default, default, default, default), false, true);
            //await RunAndCheckUpdate<>((client, updateHandler) => client.SpotApi.ExchangeData.SubscribeToTickerUpdatesAsync("ETHUSDT", updateHandler, default), true, false);
        } 
    }
}
