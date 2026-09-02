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
    internal partial class PionexSocketClientSpotSharedApi :
        SharedApiBase,
        IPionexSocketClientSpotApiShared,
        IPionexSocketClientSpotSharedApi

    {
        private readonly PionexSocketClientSpotApi _api;

        private const string _exchangeName = "Pionex";
        private const string _topicId = "PionexSpot";
        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(PionexExchange.Metadata, this);

        public PionexSocketClientSpotSharedApi(PionexSocketClientSpotApi api)
            : base(
                  api.Exchange, 
                  [TradingMode.Spot],
                  () => api.Authenticated, 
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                SubscribeTradeOptions,
                SubscribeBookTickerOptions,
                SubscribeOrderBookOptions,
                SubscribeSpotOrderOptions,
                SubscribeUserTradeOptions,
                SubscribeBalanceOptions
                );
        }

    }
}
