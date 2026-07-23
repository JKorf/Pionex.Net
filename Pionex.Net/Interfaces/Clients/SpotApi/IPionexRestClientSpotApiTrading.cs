using CryptoExchange.Net.Objects;
using Pionex.Net.Enums;
using Pionex.Net.Objects.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pionex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Pionex Spot trading endpoints, placing and managing orders.
    /// </summary>
    public interface IPionexRestClientSpotApiTrading
    {
        /// <summary>
        /// Place a new order
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/trade#post-api-v1-trade-order" /><br />
        /// Endpoint:<br />
        /// POST /api/v1/trade/order<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `ETH_USDT`</param>
        /// <param name="side">["<c>side</c>"] Order side</param>
        /// <param name="type">["<c>type</c>"] Order type</param>
        /// <param name="quantity">["<c>size</c>"] Quantity</param>
        /// <param name="price">["<c>price</c>"] Limit price</param>
        /// <param name="quoteQuantity">["<c>amount</c>"] Quote quantity, required for market buy orders</param>
        /// <param name="immediateOrCancel">["<c>IOC</c>"] IOC flag</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexOrderResult>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            OrderType type,
            decimal? quantity = null,
            decimal? quoteQuantity = null,
            decimal? price = null,
            bool? immediateOrCancel = null,
            string? clientOrderId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get order info
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/trade#get-api-v1-trade-order" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/order<br />
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>orderId</c>"] Id of the order</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexOrder>> GetOrderAsync(long orderId, CancellationToken ct = default);

        /// <summary>
        /// Get order info by client order id
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/trade#get-api-v1-trade-orderbyclientorderid" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/orderByClientOrderId<br />
        /// </para>
        /// </summary>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order id of the order</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexOrder>> GetOrderByClientOrderIdAsync(string clientOrderId, CancellationToken ct = default);

        /// <summary>
        /// Cancel an open order
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/trade#delete-api-v1-trade-order" /><br />
        /// Endpoint:<br />
        /// DELETE /api/v1/trade/order<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `ETH_USDT`</param>
        /// <param name="orderId">["<c>orderId</c>"] The order id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult> CancelOrderAsync(
            string symbol,
            long orderId,
            CancellationToken ct = default);

        /// <summary>
        /// Get open orders 
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/trade#get-api-v1-trade-openorders" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/openOrders<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `ETH_USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexOrder[]>> GetOpenOrdersAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get all orders 
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/trade#get-api-v1-trade-allorders" /><br />
        /// Endpoint:<br />
        /// GET api/v1/trade/allOrders<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `ETH_USDT`</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 200</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexOrder[]>> GetOrdersAsync(
            string symbol,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel all orders for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/trade#delete-api-v1-trade-allorders" /><br />
        /// Endpoint:<br />
        /// DELETE /api/v1/trade/allOrders<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `ETH_USDT`</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult> CancelAllOrdersAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get user trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/trade#get-api-v1-trade-fills" /><br />
        /// Endpoint:<br />
        /// GET api/v1/trade/fills<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `ETH_USDT`</param>
        /// <param name="startTime">["<c>startTime</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexUserTrade[]>> GetUserTradesAsync(
            string symbol,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get user trades for a specific order
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/trade#get-api-v1-trade-fillsbyorderid" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/trade/fillsByOrderId<br />
        /// </para>
        /// </summary>
        /// <param name="orderId">["<c>orderId</c>"] The order id</param>
        /// <param name="fromId">["<c>fromId</c>"] Filter by id</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexUserTrade[]>> GetOrderTradesAsync(
            long orderId,
            long? fromId = null,
            CancellationToken ct = default);

    }
}
