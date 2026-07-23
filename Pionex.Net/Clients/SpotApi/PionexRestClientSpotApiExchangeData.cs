using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Pionex.Net.Enums;
using Pionex.Net.Interfaces.Clients.SpotApi;
using Pionex.Net.Objects.Internal;
using Pionex.Net.Objects.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pionex.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class PionexRestClientSpotApiExchangeData : IPionexRestClientSpotApiExchangeData
    {
        private readonly PionexRestClientSpotApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal PionexRestClientSpotApiExchangeData(ILogger logger, PionexRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Server Time

        /// <inheritdoc />
        public async Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/common/symbols", PionexExchange.RateLimiter.Pionex, 1, false);
            var result = await _baseClient.SendRawAsync<PionexResult>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DateTime>(result);

            return HttpResult.Ok(result, result.Data.Timestamp);
        }

        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<HttpResult<PionexSymbol[]>> GetSymbolsAsync(
            string? symbols = null,
            SymbolType? symbolType = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("symbols", symbols);
            parameters.Add("type", symbolType);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/common/symbols", PionexExchange.RateLimiter.Pionex, 5, false);
            var result = await _baseClient.SendAsync<PionexSymbolWrapper>(request, parameters, ct).ConfigureAwait(false);
            return !result.Success ? HttpResult.Fail<PionexSymbol[]>(result) : HttpResult.Ok(result, result.Data.Symbols);
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<HttpResult<PionexTrade[]>> GetRecentTradesAsync(
            string symbol,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/trades", PionexExchange.RateLimiter.Pionex, 1, false);
            var result = await _baseClient.SendAsync<PionexTradeWrapper>(request, parameters, ct).ConfigureAwait(false);
            return !result.Success ? HttpResult.Fail<PionexTrade[]>(result) : HttpResult.Ok(result, result.Data.Trades);
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<HttpResult<PionexOrderBook>> GetOrderBookAsync(
            string symbol,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/depth", PionexExchange.RateLimiter.Pionex, 1, false);
            var result = await _baseClient.SendAsync<PionexOrderBook>(request, parameters, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<HttpResult<PionexTicker[]>> GetTickersAsync(
            string? symbol = null,
            SymbolType? type = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("type", type);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/tickers", PionexExchange.RateLimiter.Pionex, 1, false);
            var result = await _baseClient.SendAsync<PionexTickerWrapper>(request, parameters, ct).ConfigureAwait(false);
            return !result.Success ? HttpResult.Fail<PionexTicker[]>(result) : HttpResult.Ok(result, result.Data.Tickers);
        }

        #endregion

        #region Get Book Tickers

        /// <inheritdoc />
        public async Task<HttpResult<PionexBookTicker[]>> GetBookTickersAsync(
            string? symbol = null,
            SymbolType? type = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("type", type ?? SymbolType.Spot);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/bookTickers", PionexExchange.RateLimiter.Pionex, 1, false);
            var result = await _baseClient.SendAsync<PionexBookTickerWrapper>(request, parameters, ct).ConfigureAwait(false);
            return !result.Success ? HttpResult.Fail<PionexBookTicker[]>(result) : HttpResult.Ok(result, result.Data.Tickers);
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<HttpResult<PionexKline[]>> GetKlinesAsync(
            string symbol,
            KlineInterval interval,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(PionexExchange._parameterSerializationSettings);
            parameters.Add("symbol", symbol);
            parameters.Add("interval", interval);
            parameters.Add("endTime", endTime);
            parameters.Add("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/market/klines", PionexExchange.RateLimiter.Pionex, 1, false);
            var result = await _baseClient.SendAsync<PionexKlineWrapper>(request, parameters, ct).ConfigureAwait(false);
            return !result.Success ? HttpResult.Fail<PionexKline[]>(result) : HttpResult.Ok(result, result.Data.Klines);
        }

        #endregion

    }
}
