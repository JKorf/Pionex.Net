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
    internal partial class PionexSocketClientSpotApi : IPionexSocketClientSpotApiShared
    {
        private const string _exchangeName = "Pionex";
        private const string _topicId = "PionexSpot";
        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
        public SharedClientInfo Discover() => SharedUtils.GetClientInfo(PionexExchange.Metadata, this);

        #region Trade client

        SubscribeTradeOptions ITradeSocketClient.SubscribeTradeOptions { get; }
            = new SubscribeTradeOptions(_exchangeName, false);
        async Task<WebSocketResult<UpdateSubscription>> ITradeSocketClient.SubscribeToTradeUpdatesAsync(SubscribeTradeRequest request, Action<DataEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = ((ITradeSocketClient)this).SubscribeTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbol = request.SymbolName(FormatSymbol);
            var result = await SubscribeToTradeUpdatesAsync(symbol, update => handler(update.ToType(update.Data.Select(x =>
                new SharedTrade(
                    ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    x.Quantity,
                    x.Price, 
                    x.Timestamp)
            {
                Side = x.Side == Enums.OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
            }).ToArray())), ct).ConfigureAwait(false);

            return result;
        }

        #endregion

        #region Book Ticker client
        SubscribeBookTickerOptions IBookTickerSocketClient.SubscribeBookTickerOptions { get; } = new SubscribeBookTickerOptions(_exchangeName, false);
        async Task<WebSocketResult<UpdateSubscription>> IBookTickerSocketClient.SubscribeToBookTickerUpdatesAsync(SubscribeBookTickerRequest request, Action<DataEvent<SharedBookTicker>> handler, CancellationToken ct)
        {
            var validationError = ((IBookTickerSocketClient)this).SubscribeBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbol = request.SymbolName(FormatSymbol);
            var result = await SubscribeToOrderBookUpdatesAsync(
                symbol,
                1,
                update => handler(update.ToType(
                    new SharedBookTicker(
                        request.Symbol,
                        symbol,
                        update.Data.Asks[0].Price,
                        update.Data.Asks[0].Quantity,
                        update.Data.Bids[0].Price,
                        update.Data.Bids[0].Quantity
                        ))), ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Order Book client
        SubscribeOrderBookOptions IOrderBookSocketClient.SubscribeOrderBookOptions { get; } = new SubscribeOrderBookOptions(_exchangeName, false, [1, 5, 10, 20, 50, 100]);
        async Task<WebSocketResult<UpdateSubscription>> IOrderBookSocketClient.SubscribeToOrderBookUpdatesAsync(SubscribeOrderBookRequest request, Action<DataEvent<SharedOrderBook>> handler, CancellationToken ct)
        {
            var validationError = ((IOrderBookSocketClient)this).SubscribeOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbol = request.SymbolName(FormatSymbol);
            var result = await SubscribeToOrderBookUpdatesAsync(
                symbol,
                request.Limit ?? 20, 
                update => handler(update.ToType(new SharedOrderBook(update.Data.Asks, update.Data.Bids))), ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Spot Order client

        SubscribeSpotOrderOptions ISpotOrderSocketClient.SubscribeSpotOrderOptions { get; }
            = new SubscribeSpotOrderOptions(_exchangeName, true)
            {
                RequiredExchangeParameters = [
                    new ParameterDescription("Symbol", typeof(SharedSymbol), "Symbol to subscribe to open orders for", "ETH_USDT")
                    ]
            };
        async Task<WebSocketResult<UpdateSubscription>> ISpotOrderSocketClient.SubscribeToSpotOrderUpdatesAsync(SubscribeSpotOrderRequest request, Action<DataEvent<SharedSpotOrder[]>> handler, CancellationToken ct)
        {
            var validationError = ((ISpotOrderSocketClient)this).SubscribeSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbol = request.GetParamValue<SharedSymbol>(_exchangeName, "Symbol");
            var result = await SubscribeToOrderUpdatesAsync(
                symbol!.GetSymbol(FormatSymbol),
                update => handler(update.ToType(new[] {
                    new SharedSpotOrder(
                    ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, update.Data!.Symbol),
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
                Fee = update.Data.Fee,
                FeeAsset = update.Data.FeeAsset
            } })),
                ct: ct).ConfigureAwait(false);

            return result;
        }

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
        #endregion

        #region User Trade client

        SubscribeUserTradeOptions IUserTradeSocketClient.SubscribeUserTradeOptions { get; } = new SubscribeUserTradeOptions(_exchangeName, true)
        {
            RequiredExchangeParameters = [
                    new ParameterDescription("Symbol", typeof(SharedSymbol), "Symbol to subscribe to user trades for", "ETH_USDT")
                    ]
        };
        async Task<WebSocketResult<UpdateSubscription>> IUserTradeSocketClient.SubscribeToUserTradeUpdatesAsync(SubscribeUserTradeRequest request, Action<DataEvent<SharedUserTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeUserTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.GetParamValue<SharedSymbol>(_exchangeName, "Symbol");
            var result = await SubscribeToUserTradeUpdatesAsync(
                symbol!.GetSymbol(FormatSymbol),
                update => handler(update.ToType<SharedUserTrade[]>([
                    new SharedUserTrade(
                        ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, update.Data.Symbol),
                        update.Data.Symbol,
                        update.Data.OrderId.ToString(),
                        update.Data.Id.ToString(),
                        update.Data.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        update.Data.Quantity,
                        update.Data.Price,
                        update.Data.Timestamp)
                    {
                        Fee = update.Data.Fee,
                        FeeAsset = update.Data.FeeAsset,
                        Role = update.Data.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
                    }
                ])),
                ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion

        #region Balance client
        SubscribeBalanceOptions IBalanceSocketClient.SubscribeBalanceOptions { get; } = new SubscribeBalanceOptions(_exchangeName, false);
        async Task<WebSocketResult<UpdateSubscription>> IBalanceSocketClient.SubscribeToBalanceUpdatesAsync(SubscribeBalancesRequest request, Action<DataEvent<SharedBalance[]>> handler, CancellationToken ct)
        {
            var validationError = SharedClient.SubscribeBalanceOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);
            var result = await SubscribeToBalanceUpdatesAsync(
                update =>
                {
                    handler(update.ToType<SharedBalance[]>(update.Data.Select(x =>
                        new SharedBalance(
                            SupportedTradingModes,
                            x.Asset,
                            x.Free,
                            x.Free + x.Frozen)).ToArray()));
                },
                ct: ct).ConfigureAwait(false);

            return result;
        }
        #endregion

    }
}
