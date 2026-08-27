using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Pionex.Net.Clients;
using Pionex.Net.Interfaces.Clients.SpotApi;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Pionex.Net.UnitTests
{
    [TestFixture()]
    public class PionexRestClientTests
    {
        [Test]
        public void CheckSignatureExample1()
        {
            var authProvider = new PionexAuthenticationProvider(new PionexCredentials("XXX", "XXX"));
            var client = (RestApiClient)new PionexRestClient().SpotApi;

            CryptoExchange.Net.Testing.TestHelpers.CheckSignature(
                client,
                authProvider,
                HttpMethod.Post,
                "/api/v3/order",
                (uriParams, bodyParams, headers) =>
                {
                    return headers["PIONEX-SIGNATURE"].ToString();
                },
                "5B033D1BA9CB1240A51D289DFC34E6843E70420E4E335247AB581E2D8DCE5269",
                new Parameters(PionexExchange._parameterSerializationSettings)
                {
                    { "symbol", "LTCBTC" },
                },
                DateTimeConverter.ParseFromDouble(1499827319559),
                false);
        }

        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<PionexRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<PionexSocketClient>();
        }

        [Test]
        public void TestSpotRestSharedApiDiscoveryMatchesAggregate()
        {
            var (missingOptions, missingInterfaces) = TestHelpers.ValidateSharedApi(new PionexRestClient().SpotApi.SharedApi);

            Assert.That(missingOptions, Is.Empty);
            Assert.That(missingInterfaces, Is.Empty);
        }
        [Test]
        public void TestSpotSocketSharedApiDiscoveryMatchesAggregate()
        {
            var (missingOptions, missingInterfaces) = TestHelpers.ValidateSharedApi(new PionexSocketClient().SpotApi.SharedApi);

            Assert.That(missingOptions, Is.Empty);
            Assert.That(missingInterfaces, Is.Empty);
        }
    }
}
