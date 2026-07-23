using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using System;
using Pionex.Net.Objects.Models;
using Pionex.Net.Objects.Internal;

namespace Pionex.Net.Objects.Sockets
{
    internal class PionexQuery : Query<PionexSocketResponse>
    {
        public PionexQuery(PionexSocketRequest request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            var id = request.Operation == "SUBSCRIBE" ? "SUBSCRIBED" : "UNSUBSCRIBED";
            MessageRouter = MessageRouter.CreateForQuery<PionexSocketResponse>(id + request.Topic + request.Symbol, HandleMessage);
        }

        public CallResult<PionexSocketResponse> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PionexSocketResponse message)
        {
            return CallResult.Ok(message, originalData);
        }
    }
}
