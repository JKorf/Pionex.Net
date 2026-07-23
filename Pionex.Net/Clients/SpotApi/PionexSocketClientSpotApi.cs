using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Interfaces;
using Microsoft.Extensions.Logging;
using Pionex.Net.Clients.MessageHandlers;
using Pionex.Net.Interfaces.Clients.SpotApi;
using Pionex.Net.Objects.Internal;
using Pionex.Net.Objects.Models;
using Pionex.Net.Objects.Options;
using Pionex.Net.Objects.Sockets.Subscriptions;
using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Pionex.Net.Clients.SpotApi
{
    /// <summary>
    /// Client providing access to the Pionex Spot websocket Api
    /// </summary>
    internal partial class PionexSocketClientSpotApi : SocketApiClient<PionexEnvironment, PionexAuthenticationProvider, PionexCredentials>, IPionexSocketClientSpotApi
    {
        #region fields
        protected override ErrorMapping ErrorMapping => PionexErrors.Errors;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal PionexSocketClientSpotApi(ILoggerFactory? loggerFactory, PionexSocketOptions options) :
            base(loggerFactory, PionexExchange.Metadata.Id, options.Environment.SocketClientAddress!, options, options.SpotOptions)
        {
            AddSystemSubscription(new PionexPingSubscription(_logger));
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PionexExchange._serializerContext);
        /// <inheritdoc />
        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new PionexSocketSpotMessageHandler();

        /// <inheritdoc />
        protected override PionexAuthenticationProvider CreateAuthenticationProvider(PionexCredentials credentials)
            => new PionexAuthenticationProvider(credentials);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<PionexTrade[]>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, PionexSocketUpdate<PionexTrade[]>>((receiveTime, originalData, data) =>
            {
                UpdateTimeOffset(data.Timestamp);

                onMessage(
                    new DataEvent<PionexTrade[]>(PionexExchange.ExchangeName, data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Topic)
                        .WithSymbol(data.Symbol)
                        .WithDataTimestamp(data.Timestamp, GetTimeOffset())
                    );
            });

            var subscription = new PionexSubscription<PionexTrade[]>(_logger, "TRADE", symbol, internalHandler, false);
            return await SubscribeAsync(BaseAddress.AppendPath("/wsPub"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<PionexOrderBook>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, PionexSocketUpdate<PionexOrderBook>>((receiveTime, originalData, data) =>
            {
                UpdateTimeOffset(data.Timestamp);

                onMessage(
                    new DataEvent<PionexOrderBook>(PionexExchange.ExchangeName, data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Topic)
                        .WithSymbol(data.Symbol)
                        .WithDataTimestamp(data.Timestamp, GetTimeOffset())
                    );
            });

            var subscription = new PionexSubscription<PionexOrderBook>(_logger, "DEPTH", symbol, internalHandler, false, depth);
            return await SubscribeAsync(BaseAddress.AppendPath("/wsPub"), subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string symbol, Action<DataEvent<PionexOrder>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, PionexSocketUpdate<PionexOrder>>((receiveTime, originalData, data) =>
            {
                UpdateTimeOffset(data.Timestamp);

                onMessage(
                    new DataEvent<PionexOrder>(PionexExchange.ExchangeName, data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Topic)
                        .WithSymbol(data.Symbol)
                        .WithDataTimestamp(data.Timestamp, GetTimeOffset())
                    );
            });

            var subscription = new PionexSubscription<PionexOrder>(_logger, "ORDER", symbol, internalHandler, true);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(string symbol, Action<DataEvent<PionexUserTrade>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, PionexSocketUpdate<PionexUserTrade>>((receiveTime, originalData, data) =>
            {
                UpdateTimeOffset(data.Timestamp);

                onMessage(
                    new DataEvent<PionexUserTrade>(PionexExchange.ExchangeName, data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Topic)
                        .WithSymbol(data.Symbol)
                        .WithDataTimestamp(data.Timestamp, GetTimeOffset())
                    );
            });

            var subscription = new PionexSubscription<PionexUserTrade>(_logger, "FILL", symbol, internalHandler, true);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<PionexBalance[]>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, PionexSocketUpdate<PionexBalanceWrapper>>((receiveTime, originalData, data) =>
            {
                UpdateTimeOffset(data.Timestamp);

                onMessage(
                    new DataEvent<PionexBalance[]>(PionexExchange.ExchangeName, data.Data.Balances, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Topic)
                        .WithSymbol(data.Symbol)
                        .WithDataTimestamp(data.Timestamp, GetTimeOffset())
                    );
            });

            var subscription = new PionexSubscription<PionexBalanceWrapper>(_logger, "BALANCE", null, internalHandler, true);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        protected override Task<CallResult<string?>> GetConnectionUrlAsync(string address, bool authentication)
        {
            if (!authentication)
                return base.GetConnectionUrlAsync(address, authentication);

            return Task.FromResult(CallResult.Ok<string?>(address + AuthenticationProvider!.GetWebsocketQueryString(this)));
        }

        protected override Task<Uri?> GetReconnectUriAsync(ISocketConnection connection)
        {
            if (!connection.HasAuthenticatedSubscription)
                return base.GetReconnectUriAsync(connection);

            return Task.FromResult<Uri?>(new Uri(BaseAddress + AuthenticationProvider!.GetWebsocketQueryString(this)));
        }

        protected override bool ConnectionCanBeUsedFor(SocketConnection connection, string address, bool authenticated, string? topic = null)
        {
            if (!authenticated)
                return base.ConnectionCanBeUsedFor(connection, address, authenticated, topic);

            // Authenticated subscription can use existing authenticated connection
            return connection.ConnectionUriString.Contains("/ws?key");
        }

        /// <inheritdoc />
        public IPionexSocketClientSpotApiShared SharedClient => this;

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => PionexExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
