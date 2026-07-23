using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using System.Collections.Generic;

namespace Pionex.Net
{
    internal class PionexAuthenticationProvider : AuthenticationProvider<PionexCredentials, PionexCredentials>
    {
        private static IStringMessageSerializer _serializer = new SystemTextJsonMessageSerializer(PionexExchange._serializerContext);

        public PionexAuthenticationProvider(PionexCredentials credentials) : base(credentials, credentials)
        {
        }


        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration requestConfig)
        {
            if (!requestConfig.RequestDefinition.Authenticated)
                return;

            var timestamp = GetMillisecondTimestamp(apiClient);
            requestConfig.QueryParameters ??= new Parameters(new ParameterSerializationSettings());
            requestConfig.QueryParameters["timestamp"] = timestamp;
            requestConfig.Headers ??= new Dictionary<string, string>();
            requestConfig.Headers.Add("PIONEX-KEY", ApiCredentials.Key);

            var queryString = requestConfig.QueryParameters.CreateParamString(false, requestConfig.ArraySerialization);
            requestConfig.SetQueryString(queryString);

            var signStr = requestConfig.RequestDefinition.Method.ToString() + requestConfig.RequestDefinition.Path + "?" + queryString;
            
            if (requestConfig.BodyParameters != null && !requestConfig.BodyParameters.Empty)
            {
                var bodyContent = GetSerializedBody(_serializer, requestConfig.BodyParameters);
                signStr += bodyContent;
                requestConfig.SetBodyContent(bodyContent);
            }

            var signature = SignHMACSHA256(signStr);
            requestConfig.Headers.Add("PIONEX-SIGNATURE", signature);
        }

        public string GetWebsocketQueryString(SocketApiClient client)
        {
            var timestamp = GetMillisecondTimestamp(client);
            var queryParams = $"/ws?key={Key}&timestamp={timestamp}";
            var signStr = $"{queryParams}websocket_auth";
            var signature = SignHMACSHA256(signStr, outputType: SignOutputType.Hex);
            return queryParams + $"&signature={signature}";
        }
    }
}
