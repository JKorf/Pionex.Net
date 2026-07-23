using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Pionex.Net.Enums;
using Pionex.Net.Interfaces.Clients.SpotApi;
using Pionex.Net.Objects.Models;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pionex.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class PionexRestClientSpotApiTrading : IPionexRestClientSpotApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly PionexRestClientSpotApi _baseClient;
        private readonly ILogger _logger;

        internal PionexRestClientSpotApiTrading(ILogger logger, PionexRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        #region Place Order

        /// <inheritdoc />
        public async Task<HttpResult<PionexOrderResult>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            OrderType type,
            decimal? quantity = null,
            decimal? quoteQuantity = null,
            decimal? price = null,
            bool? immediateOrCancel = null,
            string? clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("side", side);
            parameters.Add("type", type);
            parameters.Add("size", quantity);
            parameters.Add("price", price);
            parameters.Add("amount", quoteQuantity);
            parameters.Add("IOC", immediateOrCancel);
            parameters.Add("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/api/v1/trade/order", PionexExchange.RateLimiter.Pionex, 1, true);
            var result = await _baseClient.SendAsync<PionexOrderResult>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<HttpResult<PionexOrder>> GetOrderAsync(long orderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("orderId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/trade/order", PionexExchange.RateLimiter.Pionex, 1, true);
            var result = await _baseClient.SendAsync<PionexOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Order By Client Order Id

        /// <inheritdoc />
        public async Task<HttpResult<PionexOrder>> GetOrderByClientOrderIdAsync(string clientOrderId, CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("clientOrderId", clientOrderId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/trade/orderByClientOrderId", PionexExchange.RateLimiter.Pionex, 1, true);
            var result = await _baseClient.SendAsync<PionexOrder>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<HttpResult> CancelOrderAsync(
            string symbol,
            long orderId,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("orderId", orderId);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/api/v1/trade/order", PionexExchange.RateLimiter.Pionex, 1, true);
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<HttpResult<PionexOrder[]>> GetOpenOrdersAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/trade/openOrders", PionexExchange.RateLimiter.Pionex, 5, true);
            var result = await _baseClient.SendAsync<PionexOrderWrapper>(request, parameters, ct).ConfigureAwait(false);
            return !result.Success ? HttpResult.Fail<PionexOrder[]>(result) : HttpResult.Ok(result, result.Data.Orders);
        }

        #endregion

        #region Get Orders

        /// <inheritdoc />
        public async Task<HttpResult<PionexOrder[]>> GetOrdersAsync(
            string symbol,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/trade/allOrders", PionexExchange.RateLimiter.Pionex, 5, true);
            var result = await _baseClient.SendAsync<PionexOrderWrapper>(request, parameters, ct).ConfigureAwait(false);
            return !result.Success ? HttpResult.Fail<PionexOrder[]>(result) : HttpResult.Ok(result, result.Data.Orders);
        }

        #endregion

        #region Cancel All Orders

        /// <inheritdoc />
        public async Task<HttpResult> CancelAllOrdersAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/api/v1/trade/allOrders", PionexExchange.RateLimiter.Pionex, 1, true);
            var result = await _baseClient.SendAsync(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<HttpResult<PionexUserTrade[]>> GetUserTradesAsync(
            string symbol,
            DateTime? startTime = null,
            DateTime? endTime = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "api/v1/trade/fills", PionexExchange.RateLimiter.Pionex, 5, true);
            var result = await _baseClient.SendAsync<PionexUserTradeWrapper>(request, parameters, ct).ConfigureAwait(false);
            return !result.Success ? HttpResult.Fail<PionexUserTrade[]>(result) : HttpResult.Ok(result, result.Data.Fills);
        }

        #endregion

        #region Get Order Trades

        /// <inheritdoc />
        public async Task<HttpResult<PionexUserTrade[]>> GetOrderTradesAsync(
            long orderId,
            long? fromId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("orderId", orderId);
            parameters.Add("fromId", fromId);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/trade/fillsByOrderId", PionexExchange.RateLimiter.Pionex, 5, true);
            var result = await _baseClient.SendAsync<PionexUserTradeWrapper>(request, parameters, ct).ConfigureAwait(false);
            return !result.Success ? HttpResult.Fail<PionexUserTrade[]>(result) : HttpResult.Ok(result, result.Data.Fills);
        }

        #endregion

    }
}
