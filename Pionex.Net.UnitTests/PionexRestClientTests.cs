using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using Pionex.Net.Clients;

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
    }
}
