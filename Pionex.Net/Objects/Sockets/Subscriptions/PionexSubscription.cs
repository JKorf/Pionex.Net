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
    internal class PionexSubscription<T> : Subscription
    {
        private readonly Action<DateTime, string?, PionexSocketUpdate<T>> _handler;
        private readonly string _topic;
        private readonly string? _symbol;
        private readonly int? _depth;

        /// <summary>
        /// ctor
        /// </summary>
        public PionexSubscription(ILogger logger, string topic, string? symbol, Action<DateTime, string?, PionexSocketUpdate<T>> handler, bool auth, int? depth = null) : base(logger, auth)
        {
            _handler = handler;
            _topic = topic;
            _symbol = symbol;
            _depth = depth;

            MessageRouter = MessageRouter.CreateForEvent<PionexSocketUpdate<T>>(topic, symbol, DoHandleMessage);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new PionexQuery(new Internal.PionexSocketRequest
            {
                Operation = "SUBSCRIBE",
                Topic = _topic,
                Symbol = _symbol,
                Depth = _depth
            }, false);
        }

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new PionexQuery(new Internal.PionexSocketRequest
            {
                Operation = "UNSUBSCRIBE",
                Topic = _topic,
                Symbol = _symbol,
                Depth = _depth
            }, false);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, PionexSocketUpdate<T> message)
        {
            _handler.Invoke(receiveTime, originalData, message);
            return CallResult.Ok();
        }
    }
}
