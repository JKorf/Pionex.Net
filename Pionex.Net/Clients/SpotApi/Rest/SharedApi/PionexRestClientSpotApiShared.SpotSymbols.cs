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
    internal partial class PionexRestClientSpotSharedApi
    {
        #region Spot Symbol client
        public SharedSymbolCatalog? SpotSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_exchangeName, _topicId, _api.EnvironmentName, null);

        public GetSpotSymbolsOptions GetSpotSymbolsOptions { get; }
            = new GetSpotSymbolsOptions(_exchangeName, false);

        public async Task<HttpResult<SharedSpotSymbol[]>> GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = GetSpotSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotSymbol[]>(_exchangeName,  validationError);

            var symbols = await _api.ExchangeData.GetSymbolsAsync(ct: ct).ConfigureAwait(false);
            if (!symbols.Success)
                return HttpResult.Fail<SharedSpotSymbol[]>(symbols);

            var data = symbols.Data
                .Select(x => ParseSymbol(x)!)
                .Where(x => x != null)
                .ToArray();

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, _api.EnvironmentName, null, data);
            return HttpResult.Ok(symbols, SharedUtils.ApplySymbolFilter(data, request));
        }

        private SharedSpotSymbol ParseSymbol(PionexSymbol symbol)
        {
            var result = new SharedSpotSymbol(symbol.BaseAsset, symbol.QuoteAsset, symbol.Symbol, symbol.Enable)
            {
                MinTradeQuantity = symbol.MinSpotQuantity,
                MaxTradeQuantity = symbol.MaxTradeQuantity,
                QuantityDecimals = symbol.BasePrecision,
                PriceDecimals = symbol.QuotePrecision,
                DisplayName = symbol.Symbol,
                QuoteAssetType = SharedAssetType.Crypto,
                UpperPriceLimitPercentage = Math.Abs(100 - symbol.BuyCeiling * 100),
                LowerPriceLimitPercentage = -(100 - symbol.SellFloor * 100)
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

        public async Task<ExchangeCallResult<SharedSymbol[]>> GetSpotSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(_exchangeName,  symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(_exchangeName,  ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, _api.EnvironmentName, null, baseAsset));
        }

        public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Only Spot symbols allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(_exchangeName,  symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(_exchangeName,  ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbol));
        }

        public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(_exchangeName,  symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(_exchangeName,  ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbolName));
        }
        #endregion
    }
}
