using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
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
        public void TestSpotSharedApiDiscoveryMatchesAggregate()
        {
            var client = new PionexRestClient();
            var sharedApi = client.SpotApi.SharedApi;

            var expectedOptions = typeof(IPionexRestClientSpotSharedApi)
                .GetInterfaces()
                .Append(typeof(IPionexRestClientSpotApiShared))
                .SelectMany(x => x.GetProperties())
                .Where(x => typeof(EndpointOptions)
                    .IsAssignableFrom(x.PropertyType))
                .Select(x => (EndpointOptions)x.GetValue(sharedApi)!)
                .Distinct()
                .ToArray();

            var providedOptions = sharedApi.EndpointOptions.ToArray();

            CollectionAssert.AreEquivalent(
                expectedOptions,
                providedOptions);

            Assert.That(
                providedOptions,
                Has.All.Property(nameof(EndpointOptions.Supported))
                    .EqualTo(true));
        }
    }
}
