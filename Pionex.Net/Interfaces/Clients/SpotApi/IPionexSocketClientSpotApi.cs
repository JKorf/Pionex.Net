using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects.Sockets;
using Pionex.Net.Objects.Models;
using CryptoExchange.Net.Interfaces.Clients;

namespace Pionex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Pionex Spot streams
    /// </summary>
    public interface IPionexSocketClientSpotApi : ISocketApiClient<PionexCredentials>, IDisposable
    {
        /// <summary>
        /// Subscribe to trade updates for a specific symbol.
        /// <para><a href="https://www.pionex.com/docs/api-docs/trade-websocket/public-stream" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to, for example `ETH_USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<PionexTrade[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to order book snapshot updates
        /// <para><a href="https://www.pionex.com/docs/api-docs/trade-websocket/public-stream" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to, for example `ETH_USDT`</param>
        /// <param name="depth">Order book depth, max 100</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(string symbol, int depth, Action<DataEvent<PionexOrderBook>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user order updates
        /// <para><a href="https://www.pionex.com/docs/api-docs/trade-websocket/private-stream#get-ws-order" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to, for example `ETH_USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(string symbol, Action<DataEvent<PionexOrder>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to user trade updates
        /// <para><a href="https://www.pionex.com/docs/api-docs/trade-websocket/private-stream#get-ws-fill" /></para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe to, for example `ETH_USDT`</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(string symbol, Action<DataEvent<PionexUserTrade>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Subscribe to balance updates
        /// <para><a href="https://www.pionex.com/docs/api-docs/trade-websocket/private-stream#get-ws-balance" /></para>
        /// </summary>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToBalanceUpdatesAsync(Action<DataEvent<PionexBalance[]>> onMessage, CancellationToken ct = default);

        /// <summary>
        /// Get the shared socket requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
        /// </summary>
        public IPionexSocketClientSpotApiShared SharedClient { get; }
    }
}
