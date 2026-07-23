using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Pionex.Net.Enums;
using Pionex.Net.Interfaces.Clients.SpotApi;
using Pionex.Net.Objects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pionex.Net.Clients.SpotApi
{
    internal partial class PionexRestClientSpotApi : IPionexRestClientSpotApiShared
    {
        private const string _topicId = "PionexSpot";
        private const string _exchangeName = "Pionex";

        public TradingMode[] SupportedTradingModes => new[] { TradingMode.Spot };

        public void SetDefaultExchangeParameter(string key, object value) => ExchangeParameters.SetStaticParameter(Exchange, key, value);
        public void ResetDefaultExchangeParameters() => ExchangeParameters.ResetStaticParameters();
        public SharedClientInfo Discover() => SharedUtils.GetClientInfo(PionexExchange.Metadata, this);

        private readonly HashSet<string> _knownCommodities = [ 
            "BNOX",  // Brent crude oil
            "CPERX", // Copper
            "GSGX",  // Broad commodity index
            "PALLX", // Palladium
            "PAXG",  // Gold
            "PPLTX", // Platinum
            "SLVX",  // Silver
            "UNGX",  // Natural gas
            "USOX",  // Crude oil
            "XAUT_BTC",   // Gold
            "XAUT",  // Gold
            "XAUT"   // Gold
        ];
        private readonly HashSet<string> _knownEquities = [
            "AAOIX", "AAPLX", "AAX", "ADBEX", "ADIX", "ALBX", "AMATX", "AMDX", "AMKRX", "AMZNX",
            "ANETX", "APLDX", "APPX", "ARMX", "ASMLX", "ASTSX", "AVGOX", "AXTIX", "BABAX", "BAX",
            "BBAIX", "BEX", "BITFX", "BLKX", "BLSHX", "BMNRX", "BOTX", "BRKBX", "BXDCX", "CARX",
            "CATX", "CBRSX", "CCJX", "CEGX", "CF", "CIFRX", "CLSKX", "COHRX", "COINX", "COPXX",
            "COPX", "COSTX", "CRCLX", "CRDOX", "CRWDX", "CRWVX", "CSCOX", "CVXX", "DBAX", "DELLX",
            "DIAX", "DJTX", "DRAMX", "DXYZX", "ENPHX", "EQTX", "ETNX", "EUVX", "EWGX", "EWJX",
            "EWTX", "EWUX", "EWYX", "FCXX", "FEZX", "FLNCX", "FLYX", "FNX", "FRVOX", "GEMIX",
            "GEVX", "GEX", "GLWX", "GMEX", "GOOGLX", "GSX", "HIMSX", "HOODX", "IBMX", "IGVX",
            "INTCX", "INTWX", "IONQX", "IRENX", "ISRGX", "ITAX", "IWMX", "KEYSX", "KOPNX", "KSTRX",
            "LACX", "LCLNX", "LITEX", "LLYX", "LMTX", "LNGX", "LRCXX", "LWLGX", "MARAX", "MCDX",
            "METAX", "MOOX", "MOSX", "MPX", "MRVLX", "MSFTX", "MSTRX", "MUUX", "MUX", "MVLLX",
            "NASAX", "NBISX", "NEEX", "NFLXX", "NIOX", "NKEX", "NLRX", "NOCX", "NOKX", "NTRX",
            "NVDAX", "NVOX", "OKLOX", "ONDSX", "ONX", "OPENX", "ORCLX", "OXYX", "PAYPX", "PDDX",
            "PLTRX", "PYPLX", "QCOMX", "QQQX", "RAMX", "RBLXX", "RDWX", "REMXX", "RGTIX", "RIOTX",
            "RKLBX", "RTXX", "SAPX", "SATSX", "SBETX", "SEX", "SHLDX", "SITMX", "SKDDX", "SKHX",
            "SKHY", "SKUUX", "SLBX", "SMCIX", "SMHX", "SMRX", "SMSN", "SNDKX", "SNOWX", "SNPSX",
            "SOFIX", "SOXLX", "SOXSX", "SOXXX", "SPCX", "SPYX", "SQQQX", "SSOX", "STM", "STRCX",
            "STXX", "SWMRX", "TCOMX", "TELX", "TERX", "TQQQX", "TSEMX", "TSLAX", "TSLLX", "TSMX",
            "TTEX", "TXNX", "UAMYX", "UBERX", "UFOX", "UNHX", "URAX", "URNMX", "USARX", "VCXX",
            "VGKX", "VNQX", "VOLTX", "VRTX", "VSHX", "VSTX", "VTIX", "VXXX", "WDCX", "WULFX",
            "XEX", "XLBX", "XLEX", "XLKX", "XMEX", "XOMX", "XOVRX", "XYZ"
            ];

        #region Balance Client
        GetBalancesOptions IBalanceRestClient.GetBalancesOptions { get; } = new GetBalancesOptions(_exchangeName, AccountTypeFilter.Spot);

        async Task<HttpResult<SharedBalance[]>> IBalanceRestClient.GetBalancesAsync(GetBalancesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetBalancesOptions.ValidateRequest(request, this);
            if (validationError != null)
            return HttpResult.Fail<SharedBalance[]>(Exchange, validationError);

            var result = await Account.GetBalancesAsync(ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedBalance[]>(result);

            return HttpResult.Ok(result, result.Data!.Select(x =>
                new SharedBalance(
                    SupportedTradingModes,
                    x.Asset,
                    x.Free,
                    x.Frozen + x.Free)).ToArray());
        }

        #endregion

        #region Book Ticker client

        GetBookTickerOptions IBookTickerRestClient.GetBookTickerOptions { get; }
            = new GetBookTickerOptions(_exchangeName, false);
        async Task<HttpResult<SharedBookTicker>> IBookTickerRestClient.GetBookTickerAsync(GetBookTickerRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedBookTicker>(Exchange, validationError);

            var resultTicker = await ExchangeData.GetBookTickersAsync(request.Symbol!.GetSymbol(FormatSymbol), ct: ct).ConfigureAwait(false);
            if (!resultTicker.Success)
                return HttpResult.Fail<SharedBookTicker>(resultTicker);

            var symbol = resultTicker.Data.SingleOrDefault();
            if (symbol == null)
                return HttpResult.Fail<SharedBookTicker>(resultTicker, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Symbol not found")));

            return HttpResult.Ok(resultTicker, new SharedBookTicker(
                request.Symbol,
                symbol.Symbol,
                symbol.AskPrice,
                symbol.AskQuantity,
                symbol.BidPrice,
                symbol.BidQuantity));

        }

        #endregion

        #region Klines Client

        GetKlinesOptions IKlineRestClient.GetKlinesOptions { get; } = new GetKlinesOptions(_exchangeName, false, true, true, 500, false, [
            SharedKlineInterval.OneMinute,
            SharedKlineInterval.FiveMinutes,
            SharedKlineInterval.FifteenMinutes,
            SharedKlineInterval.ThirtyMinutes,
            SharedKlineInterval.OneHour,
            SharedKlineInterval.FourHours,
            SharedKlineInterval.EightHours,
            SharedKlineInterval.TwelveHours,
            SharedKlineInterval.OneDay
            ]);
        async Task<HttpResult<SharedKline[]>> IKlineRestClient.GetKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetKlinesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedKline[]>(Exchange, validationError);

            var direction = DataDirection.Descending;
            var symbol = request.SymbolName(FormatSymbol);
            var limit = request.Limit ?? 500;
            var pageParams = Pagination.GetPaginationParameters(direction, limit, request.StartTime, request.EndTime ?? DateTime.UtcNow, pageRequest, false);

            // Get data
            var result = await ExchangeData.GetKlinesAsync(
                symbol,
                (Enums.KlineInterval)request.Interval,
                pageParams.EndTime,
                limit,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedKline[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                    () => Pagination.NextPageFromTime(pageParams, result.Data!.Min(x => x.OpenTime)),
                    result.Data!.Length,
                    result.Data.Select(x => x.OpenTime),
                    request.StartTime,
                    request.EndTime ?? DateTime.UtcNow,
                    pageParams);

            // Return
            return HttpResult.Ok(result,
                ExchangeHelpers.ApplyFilter(result.Data, x => x.OpenTime, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedKline(request.Symbol, symbol, x.OpenTime, x.ClosePrice, x.HighPrice, x.LowPrice, x.OpenPrice, x.Volume))
                    .ToArray(), nextPageRequest);

        }

        #endregion

        #region Order Book client
        GetOrderBookOptions IOrderBookRestClient.GetOrderBookOptions { get; } = new GetOrderBookOptions(_exchangeName, 1, 1000, false);
        async Task<HttpResult<SharedOrderBook>> IOrderBookRestClient.GetOrderBookAsync(GetOrderBookRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedOrderBook>(Exchange, validationError);

            var result = await ExchangeData.GetOrderBookAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedOrderBook>(result);

            return HttpResult.Ok(result, new SharedOrderBook(result.Data!.Asks, result.Data.Bids));

        }

        #endregion

        #region Recent Trades client
        GetRecentTradesOptions IRecentTradeRestClient.GetRecentTradesOptions { get; } = new GetRecentTradesOptions(_exchangeName, 500, false);

        async Task<HttpResult<SharedTrade[]>> IRecentTradeRestClient.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetRecentTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

            // Get data
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await ExchangeData.GetRecentTradesAsync(
                symbol,
                limit: request.Limit,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTrade[]>(result);

            // Return
            return HttpResult.Ok(result, result.Data!.Select(x =>
                new SharedTrade(request.Symbol, symbol, x.Quantity, x.Price, x.Timestamp)
                {
                    Side = x.Side == Enums.OrderSide.Sell ? SharedOrderSide.Sell : SharedOrderSide.Buy,
                }).ToArray());

        }
        #endregion

        #region Spot Symbol client
        SharedSymbolCatalog? ISpotSymbolRestClient.SpotSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_exchangeName, _topicId, EnvironmentName, null);

        GetSpotSymbolsOptions ISpotSymbolRestClient.GetSpotSymbolsOptions { get; }
            = new GetSpotSymbolsOptions(_exchangeName, false);

        async Task<HttpResult<SharedSpotSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotSymbol[]>(Exchange, validationError);

            var symbols = await ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!symbols.Success)
                return HttpResult.Fail<SharedSpotSymbol[]>(symbols);

            var data = symbols.Data
                .Select(x => ParseSymbol(x)!)
                .Where(x => x != null)
                .ToArray();

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, EnvironmentName, null, data);
            return HttpResult.Ok(symbols, SharedUtils.ApplySymbolFilter(data, request));
        }

        private SharedSpotSymbol ParseSymbol(PionexSymbol symbol)
        {
            var result = new SharedSpotSymbol(symbol.BaseAsset, symbol.QuoteAsset, symbol.Symbol, symbol.Enable)
            {
                MinTradeQuantity = symbol.MinTradeQuantity,
                MaxTradeQuantity = symbol.MaxTradeQuantity,
                QuantityDecimals = symbol.BasePrecision,
                PriceDecimals = symbol.QuotePrecision,
                DisplayName = symbol.Symbol,
                QuoteAssetType = SharedAssetType.Crypto
            };

            if (LibraryHelpers.IsStableCoin(result.QuoteAsset))
                result.QuoteAssetSubType = SharedAssetSubType.StableCoin;

            if (LibraryHelpers.IsCommodity(symbol.BaseAsset, _knownCommodities))
            {
                result.BaseAssetType = SharedAssetType.TradFi;
                result.BaseAssetSubType = SharedAssetSubType.Commodity;
            }
            else if (LibraryHelpers.IsEquity(symbol.BaseAsset, ["X"], _knownEquities))
            {
                result.BaseAssetType = SharedAssetType.TradFi;
                result.BaseAssetSubType = SharedAssetSubType.Equity;
            }
            else
            {
                result.BaseAssetType = SharedAssetType.Crypto;
                if (LibraryHelpers.IsStableCoin(result.BaseAsset))
                    result.BaseAssetSubType = SharedAssetSubType.StableCoin;
            }

            return result;
        }

        async Task<ExchangeCallResult<SharedSymbol[]>> ISpotSymbolRestClient.GetSpotSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, EnvironmentName, null, baseAsset));
        }

        async Task<ExchangeCallResult<bool>> ISpotSymbolRestClient.SupportsSpotSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Only Spot symbols allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, EnvironmentName, null, symbol));
        }

        async Task<ExchangeCallResult<bool>> ISpotSymbolRestClient.SupportsSpotSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, EnvironmentName, null))
            {
                var symbols = await ((ISpotSymbolRestClient)this).GetSpotSymbolsAsync(new GetSymbolsRequest()).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, EnvironmentName, null, symbolName));
        }
        #endregion

        #region Ticker client

        GetSpotTickerOptions ISpotTickerRestClient.GetSpotTickerOptions { get; } = new GetSpotTickerOptions(_exchangeName);
        async Task<HttpResult<SharedSpotTicker>> ISpotTickerRestClient.GetSpotTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker>(Exchange, validationError);

            var result = await ExchangeData.GetTickersAsync(request.SymbolName(FormatSymbol), Enums.SymbolType.Spot, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker>(result);

            var symbol = result.Data.SingleOrDefault();
            if (symbol == null)
                return HttpResult.Fail<SharedSpotTicker>(result, new ServerError(new ErrorInfo(ErrorType.UnknownSymbol, "Symbol not found")));

            return HttpResult.Ok(result, new SharedSpotTicker(
                    ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, symbol.Symbol),
                    symbol.Symbol,
                    symbol.ClosePrice,
                    symbol.HighPrice,
                    symbol.LowPrice,
                    symbol.Volume,
                    symbol.OpenPrice > 0 && symbol.ClosePrice > 0 ? Math.Round(symbol.ClosePrice / symbol.OpenPrice * 100 - 100, 4) : null)
            {
                QuoteVolume = symbol.VolumeQuote
            });

        }

        GetSpotTickersOptions ISpotTickerRestClient.GetSpotTickersOptions { get; } = new GetSpotTickersOptions(_exchangeName);
        async Task<HttpResult<SharedSpotTicker[]>> ISpotTickerRestClient.GetSpotTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotTicker[]>(Exchange, validationError);

            var result = await ExchangeData.GetTickersAsync(type: Enums.SymbolType.Spot, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotTicker[]>(result);

            return HttpResult.Ok(result, result.Data!.Select(x =>
                    new SharedSpotTicker(
                        ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol),
                        x.Symbol,
                        x.ClosePrice,
                        x.HighPrice,
                        x.LowPrice,
                        x.Volume,
                        x.OpenPrice > 0 && x.ClosePrice > 0 ? Math.Round(x.ClosePrice / x.OpenPrice * 100 - 100, 4) : null)
                    {
                        QuoteVolume = x.VolumeQuote
                    }).ToArray());

        }

        #endregion

        #region Spot Order Client

        SharedFeeDeductionType ISpotOrderRestClient.SpotFeeDeductionType => SharedFeeDeductionType.DeductFromOutput;
        SharedFeeAssetType ISpotOrderRestClient.SpotFeeAssetType => SharedFeeAssetType.OutputAsset;
        SharedOrderType[] ISpotOrderRestClient.SpotSupportedOrderTypes { get; } = new[] { SharedOrderType.Limit, SharedOrderType.Market };
        SharedTimeInForce[] ISpotOrderRestClient.SpotSupportedTimeInForce { get; } = new[] { SharedTimeInForce.GoodTillCanceled, SharedTimeInForce.ImmediateOrCancel };
        SharedQuantitySupport ISpotOrderRestClient.SpotSupportedOrderQuantity { get; } = new SharedQuantitySupport(
                SharedQuantityType.BaseAsset,
                SharedQuantityType.BaseAsset,
                SharedQuantityType.QuoteAsset,
                SharedQuantityType.BaseAsset);

        string ISpotOrderRestClient.GenerateClientOrderId() => ExchangeHelpers.RandomString(20);

        PlaceSpotOrderOptions ISpotOrderRestClient.PlaceSpotOrderOptions { get; } = new PlaceSpotOrderOptions(_exchangeName);
        async Task<HttpResult<SharedId>> ISpotOrderRestClient.PlaceSpotOrderAsync(PlaceSpotOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.PlaceSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await Trading.PlaceOrderAsync(
                request.Symbol!.GetSymbol(FormatSymbol),
                request.Side == SharedOrderSide.Buy ? Enums.OrderSide.Buy : Enums.OrderSide.Sell,
                request.OrderType == SharedOrderType.Limit ? Enums.OrderType.Limit : Enums.OrderType.Market,
                quantity: request.Quantity?.QuantityInBaseAsset,
                quoteQuantity: request.Quantity?.QuantityInQuoteAsset,
                price: request.Price,
                immediateOrCancel: request.TimeInForce == SharedTimeInForce.ImmediateOrCancel ? true : null,
                clientOrderId: request.ClientOrderId,
                ct: ct).ConfigureAwait(false);

            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data!.OrderId.ToString()));

        }

        GetSpotOrderOptions ISpotOrderRestClient.GetSpotOrderOptions { get; } = new GetSpotOrderOptions(_exchangeName, true);
        async Task<HttpResult<SharedSpotOrder>> ISpotOrderRestClient.GetSpotOrderAsync(GetOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedSpotOrder>(Exchange, ArgumentError.Invalid(nameof(GetOrderRequest.OrderId), "Invalid order id"));

            var order = await Trading.GetOrderAsync(orderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedSpotOrder>(order);

            return HttpResult.Ok(order, new SharedSpotOrder(
                    ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, order.Data!.Symbol),
                    order.Data.Symbol,
                    order.Data.OrderId.ToString(),
                    order.Data.OrderType == OrderType.Market ? SharedOrderType.Market : SharedOrderType.Limit,
                    order.Data.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                    ParseOrderStatus(order.Data),
                    order.Data.CreateTime)
            {
                ClientOrderId = order.Data.ClientOrderId,
                OrderPrice = order.Data.Price,
                OrderQuantity = new SharedOrderQuantity(order.Data.Quantity, order.Data.QuoteQuantity == 0 ? null : order.Data.QuoteQuantity),
                QuantityFilled = new SharedOrderQuantity(order.Data.QuantityFilled, order.Data.QuoteQuantityFilled),
                TimeInForce = order.Data.IOC ? SharedTimeInForce.ImmediateOrCancel : SharedTimeInForce.GoodTillCanceled,
                UpdateTime = order.Data.UpdateTime,
                Fee = order.Data.Fee,
                FeeAsset = order.Data.FeeAsset                
            });

        }

        GetOpenSpotOrdersOptions ISpotOrderRestClient.GetOpenSpotOrdersOptions { get; }
            = new GetOpenSpotOrdersOptions(_exchangeName, true)
            {
                RequiredOptionalParameters = [
                    new ParameterDescription(nameof(GetOpenOrdersRequest.Symbol), typeof(SharedSymbol), "Symbol to get open orders for", "ETH_USDT")
                    ]
            };
        async Task<HttpResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetOpenSpotOrdersAsync(GetOpenOrdersRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetOpenSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var orders = await Trading.GetOpenOrdersAsync(symbol, ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(orders);

            return HttpResult.Ok(orders, orders.Data!.Select(x => new SharedSpotOrder(
                    ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol),
                    x.Symbol,
                    x.OrderId.ToString(),
                    x.OrderType == OrderType.Market ? SharedOrderType.Market : SharedOrderType.Limit,
                    x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                    ParseOrderStatus(x),
                    x.CreateTime)
            {
                ClientOrderId = x.ClientOrderId,
                OrderPrice = x.Price,
                OrderQuantity = new SharedOrderQuantity(x.Quantity, x.QuoteQuantity == 0 ? null : x.QuoteQuantity),
                QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.QuoteQuantityFilled),
                TimeInForce = x.IOC ? SharedTimeInForce.ImmediateOrCancel : SharedTimeInForce.GoodTillCanceled,
                UpdateTime = x.UpdateTime,
                Fee = x.Fee,
                FeeAsset = x.FeeAsset
            }).ToArray());

        }

        GetSpotClosedOrdersOptions ISpotOrderRestClient.GetClosedSpotOrdersOptions { get; } = new GetSpotClosedOrdersOptions(_exchangeName, false, true, true, 200);
        async Task<HttpResult<SharedSpotOrder[]>> ISpotOrderRestClient.GetClosedSpotOrdersAsync(GetClosedOrdersRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetClosedSpotOrdersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotOrder[]>(Exchange, validationError);

            var direction = DataDirection.Descending;
            var limit = request.Limit ?? 200;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var pageParams = Pagination.GetPaginationParameters(
                direction, limit, 
                request.StartTime ?? request.EndTime?.AddDays(-30) ?? DateTime.UtcNow.AddDays(-30),
                request.EndTime ?? DateTime.UtcNow,
                pageRequest,
                true);

            // Get data
            var result = await Trading.GetOrdersAsync(
                symbol,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                limit: limit,
                ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotOrder[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                   () => Pagination.NextPageFromTime(pageParams, result.Data!.Min(x => x.CreateTime)),
                   result.Data!.Length,
                   result.Data.Select(x => x.CreateTime),
                   request.StartTime,
                   request.EndTime ?? DateTime.UtcNow,
                   pageParams);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.CreateTime, request.StartTime, request.EndTime, direction)
                    .Where(x => x.Status == OrderStatus.Closed)
                    .Select(x => new SharedSpotOrder(
                        ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol),
                        x.Symbol,
                        x.OrderId.ToString(),
                        x.OrderType == OrderType.Market ? SharedOrderType.Market : SharedOrderType.Limit,
                        x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                        ParseOrderStatus(x),
                        x.CreateTime)
                        {
                            ClientOrderId = x.ClientOrderId,
                            OrderPrice = x.Price,
                            OrderQuantity = new SharedOrderQuantity(x.Quantity, x.QuoteQuantity == 0 ? null : x.QuoteQuantity),
                            QuantityFilled = new SharedOrderQuantity(x.QuantityFilled, x.QuoteQuantityFilled),
                            TimeInForce = x.IOC ? SharedTimeInForce.ImmediateOrCancel : SharedTimeInForce.GoodTillCanceled,
                            UpdateTime = x.UpdateTime,
                            Fee = x.Fee,
                            FeeAsset = x.FeeAsset
                        })
                        .ToArray(), nextPageRequest);

        }

        GetSpotOrderTradesOptions ISpotOrderRestClient.GetSpotOrderTradesOptions { get; }
            = new GetSpotOrderTradesOptions(_exchangeName, true);
        async Task<HttpResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotOrderTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, ArgumentError.Invalid(nameof(GetOrderTradesRequest.OrderId), "Invalid order id"));

            var orders = await Trading.GetOrderTradesAsync(orderId: orderId, ct: ct).ConfigureAwait(false);
            if (!orders.Success)
                return HttpResult.Fail<SharedUserTrade[]>(orders);

            return HttpResult.Ok(orders, orders.Data!.Select(x => new SharedUserTrade(
                ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol),
                x.Symbol,
                x.OrderId.ToString(),
                x.Id.ToString(),
                x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                x.Quantity,
                x.Price,
                x.Timestamp)
            {
                Fee = x.Fee,
                FeeAsset = x.FeeAsset,
                Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
            }).ToArray());

        }

        GetSpotUserTradesOptions ISpotOrderRestClient.GetSpotUserTradesOptions { get; } = new GetSpotUserTradesOptions(_exchangeName, false, true, true, 100);
        async Task<HttpResult<SharedUserTrade[]>> ISpotOrderRestClient.GetSpotUserTradesAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = SharedClient.GetSpotUserTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            var direction = DataDirection.Descending;
            var limit = request.Limit ?? 100;
            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var pageParams = Pagination.GetPaginationParameters(
                direction, limit, request.StartTime,
                request.EndTime ?? DateTime.UtcNow,
                pageRequest,
                false);

            // Get data
            var result = await Trading.GetUserTradesAsync(
                symbol,
                startTime: pageParams.StartTime,
                endTime: pageParams.EndTime,
                ct: ct
                ).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedUserTrade[]>(result);

            var nextPageRequest = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromTime(pageParams, result.Data!.Min(x => x.Timestamp), false),
                result.Data!.Length,
                result.Data.Select(x => x.Timestamp),
                request.StartTime,
                request.EndTime ?? DateTime.UtcNow,
                pageParams,
                pageRequest?.FromId != null ? null : TimeSpan.FromDays(1));

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, direction)
                    .Select(x =>
                        new SharedUserTrade(
                            ExchangeSymbolCache.ParseSymbol(_topicId, EnvironmentName, null, x.Symbol),
                            x.Symbol,
                            x.OrderId.ToString(),
                            x.Id.ToString(),
                            x.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                            x.Quantity,
                            x.Price,
                            x.Timestamp)
                        {
                            Fee = x.Fee,
                            FeeAsset = x.FeeAsset,
                            Role = x.Role == TradeRole.Maker ? SharedRole.Maker : SharedRole.Taker
                        })
                    .ToArray(), nextPageRequest);

        }

        CancelSpotOrderOptions ISpotOrderRestClient.CancelSpotOrderOptions { get; }
            = new CancelSpotOrderOptions(_exchangeName, true);
        async Task<HttpResult<SharedId>> ISpotOrderRestClient.CancelSpotOrderAsync(CancelOrderRequest request, CancellationToken ct)
        {
            var validationError = SharedClient.CancelSpotOrderOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            if (!long.TryParse(request.OrderId, out var orderId))
                return HttpResult.Fail<SharedId>(Exchange, ArgumentError.Invalid(nameof(CancelOrderRequest.OrderId), "Invalid order id"));

            var order = await Trading.CancelOrderAsync(request.Symbol!.GetSymbol(FormatSymbol), orderId, ct: ct).ConfigureAwait(false);
            if (!order.Success)
                return HttpResult.Fail<SharedId>(order);

            return HttpResult.Ok(order, new SharedId(request.OrderId));

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
    }
}
