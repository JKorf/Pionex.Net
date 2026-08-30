using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;
using Pionex.Net.Objects.Internal;
using System;

namespace Pionex.Net.Objects.Sockets.Subscriptions
{
    /// <summary>
    /// Handles the Pionex socket close message and triggers a reconnect
    /// </summary>
    internal class PionexCloseSubscription : SystemSubscription
    {
        /// <summary>
        /// ctor
        /// </summary>
        public PionexCloseSubscription(ILogger logger) : base(logger)
        {
            MessageRouter = MessageRouter.CreateForEvent<PionexSocketCloseMessage>("CLOSE", DoHandleMessage);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PionexSocketCloseMessage message)
        {
            _logger.LogWarning("Server requested socket reconnect. Note: {Note}", message.Note);
            _ = connection.TriggerReconnectAsync();
            return CallResult.Ok();
        }
    }
}
