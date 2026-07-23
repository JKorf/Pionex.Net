using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;
using Pionex.Net.Objects.Internal;
using System;

namespace Pionex.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class PionexPingSubscription : SystemSubscription
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PionexPingSubscription(ILogger logger) : base(logger)
        {
            MessageRouter = MessageRouter.CreateForEvent<PionexSocketPingMessage>("PING", DoHandleMessage);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PionexSocketPingMessage message)
        {
            connection.SendAsync(ExchangeHelpers.NextId(), message with { Operation = "PONG" });
            return CallResult.Ok();
        }
    }
}
