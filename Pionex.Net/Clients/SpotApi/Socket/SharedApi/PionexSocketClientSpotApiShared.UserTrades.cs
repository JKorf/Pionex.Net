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
        #region User Trade client

        public SubscribeUserTradeOptions SubscribeUserTradeOptions { get; } = new SubscribeUserTradeOptions(_exchangeName, true)
        {
            RequiredExchangeParameters = [
                    new ParameterDescription("Symbol", typeof(SharedSymbol), "Symbol to subscribe to user trades for", "ETH_USDT")
                    ]
        };
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(SubscribeUserTradeRequest request, Action<DataEvent<SharedUserTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeUserTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(_exchangeName, validationError);

            var symbol = request.GetParamValue<SharedSymbol>(_exchangeName, "Symbol");
            var result = await _api.SubscribeToUserTradeUpdatesAsync(
                symbol!.GetSymbol(FormatSymbol),
                update => handler(update.ToType<SharedUserTrade[]>([
                    new SharedUserTrade(
                        ExchangeSymbolCache.ParseSymbol(_topicId, _api.EnvironmentName, null, update.Data.Symbol),
                        update.Data.Symbol,
                        update.Data.OrderId.ToString(),
                        update.Data.Id.ToString(),
                        update.Data.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        new SharedOrderQuantity(update.Data.Quantity),
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
    }
}
