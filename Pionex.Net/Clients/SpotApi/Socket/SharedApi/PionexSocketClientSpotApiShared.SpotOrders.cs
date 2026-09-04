using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using Pionex.Net.Enums;
using Pionex.Net.Interfaces.Clients.SpotApi;
using Pionex.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Pionex.Net.Clients.SpotApi
{
    internal partial class PionexSocketClientSpotSharedApi
    {

        #region Subscribe Spot Orders

        async Task<WebSocketResult<UpdateSubscription>> ISpotOrderSocketClient.SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<DataEvent<SharedSpotOrder[]>> handler, CancellationToken ct)
            => await SubscribeToSpotOrderUpdatesAsync(request, x => handler(x.ToType<SharedSpotOrder[]>(x.Data)), ct).ConfigureAwait(false);

        public SubscribeSpotOrderOptions SubscribeSpotOrderOptions { get; }
            = new SubscribeSpotOrderOptions(_exchangeName, true)
            {
                RequiredExchangeParameters = [
                    new ParameterDescription("Symbol", typeof(SharedSymbol), "Symbol to subscribe to open orders for", "ETH_USDT")
                    ]
            };
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<DataEvent<SharedSpotOrderUpdate[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbol = request.GetParamValue<SharedSymbol>(_exchangeName, "Symbol");
            var result = await _api.SubscribeToOrderUpdatesAsync(
                symbol!.GetSymbol(FormatSymbol),
                update => handler(update.ToType(new[] {
                    new SharedSpotOrderUpdate(
                    ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Data!.Symbol),
                    update.Data.Symbol,
                    update.Data.OrderId.ToString(),
                    update.Data.OrderType == OrderType.Market ? SharedOrderType.Market : SharedOrderType.Limit,
                    update.Data.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                    ParseOrderStatus(update.Data),
                    update.Data.CreateTime)
            {
                ClientOrderId = update.Data.ClientOrderId,
                OrderPrice = update.Data.Price,
                OrderQuantity = new SharedOrderQuantity(update.Data.Quantity, update.Data.QuoteQuantity == 0 ? null : update.Data.QuoteQuantity),
                QuantityFilled = new SharedOrderQuantity(update.Data.QuantityFilled, update.Data.QuoteQuantityFilled),
                TimeInForce = update.Data.IOC ? SharedTimeInForce.ImmediateOrCancel : SharedTimeInForce.GoodTillCanceled,
                UpdateTime = update.Data.UpdateTime,
#pragma warning disable CS0618 // Type or member is obsolete
                Fee = update.Data.Fee,
                FeeAsset = update.Data.FeeAsset
#pragma warning restore CS0618 // Type or member is obsolete
            } })),
                ct: ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        private SharedOrderStatus ParseOrderStatus(PionexOrder order)
        {
            if (order.Status == OrderStatus.Open)
                return SharedOrderStatus.Open;

            if (order.OrderType == OrderType.Market && order.Side == OrderSide.Buy)
                // Market buy orders are always filled immediately, and the quantity is in quote which makes it hard to check
                // Should be safe to assume it's filled
                return SharedOrderStatus.Filled;

            if (order.QuantityFilled == order.Quantity)
                return SharedOrderStatus.Filled;

            if (!(order.Quantity > 0))
                // If original order quantity is not provided we don't really know the status
                return SharedOrderStatus.Unknown;

            return SharedOrderStatus.Canceled;
        }
    }
}
