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
        #region Book Ticker client
        public SubscribeBookTickerOptions SubscribeBookTickerOptions { get; } = new SubscribeBookTickerOptions(_exchangeName, false);
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(SubscribeBookTickerRequest request, Action<DataEvent<SharedBookTicker>> handler, CancellationToken ct)
        {
            var validationError = SubscribeBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbol = request.SymbolName(FormatSymbol);
            var result = await _api.SubscribeToOrderBookUpdatesAsync(
                symbol,
                1,
                update => handler(update.ToType(
                    new SharedBookTicker(
                        request.Symbol,
                        symbol,
                        update.Data.Asks[0].Price,
                        new SharedOrderQuantity(update.Data.Asks[0].Quantity),
                        update.Data.Bids[0].Price,
                        new SharedOrderQuantity(update.Data.Bids[0].Quantity)
                        ))), ct).ConfigureAwait(false);

            return result;
        }
        #endregion
    }
}
