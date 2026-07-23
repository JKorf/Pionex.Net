using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Pionex.Net.Enums;
using Pionex.Net.Objects.Models;

namespace Pionex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Pionex Spot exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IPionexRestClientSpotApiExchangeData
    {
        /// <summary>
        /// Retrieve the server time<br/>
        /// Note, this uses the GetSymbolsAsync endpoint and only uses the returned server time
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get symbols supported by the exchange
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/common" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/common/symbols<br />
        /// </para>
        /// </summary>
        /// <param name="symbols">["<c>symbols</c>"] Filter by symbols</param>
        /// <param name="symbolType">["<c>type</c>"] Filter by symbol type</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexSymbol[]>> GetSymbolsAsync(
            string? symbols = null,
            SymbolType? symbolType = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get recent trades for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/market" /><br />
        /// Endpoint:<br />
        /// GET api/v1/market/trades<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `ETH_USDT`</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 500</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexTrade[]>> GetRecentTradesAsync(
            string symbol,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get order book snapshot for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/market#get-api-v1-market-depth" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/market/depth<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `ETH_USDT`</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 1000</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexOrderBook>> GetOrderBookAsync(
            string symbol,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get 24h price statistics
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/market#get-api-v1-market-tickers" /><br />
        /// Endpoint:<br />
        /// GET  /api/v1/market/tickers<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol, for example `ETHUSDT`</param>
        /// <param name="type">["<c>type</c>"] Filter by symbol type</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexTicker[]>> GetTickersAsync(
            string? symbol = null,
            SymbolType? type = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get book tickers
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/market#get-api-v1-market-booktickers" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/market/bookTickers<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol, for example `ETH_USDT`</param>
        /// <param name="type">["<c>type</c>"] Filter by type</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexBookTicker[]>> GetBookTickersAsync(
            string? symbol = null,
            SymbolType? type = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get klines
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/market#get-api-v1-market-klines" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/market/klines<br />
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `ETH_USDT`</param>
        /// <param name="interval">["<c>interval</c>"] Kline interval</param>
        /// <param name="endTime">["<c>endTime</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 500</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexKline[]>> GetKlinesAsync(
            string symbol,
            KlineInterval interval,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

    }
}
