using CryptoExchange.Net.Objects;
using Pionex.Net.Interfaces.Clients.SpotApi;
using Pionex.Net.Objects.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pionex.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class PionexRestClientSpotApiAccount : IPionexRestClientSpotApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly PionexRestClientSpotApi _baseClient;

        internal PionexRestClientSpotApiAccount(PionexRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Balances

        /// <inheritdoc />
        public async Task<HttpResult<PionexBalance[]>> GetBalancesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/account/balances", PionexExchange.RateLimiter.Pionex, 1, true);
            var result = await _baseClient.SendAsync<PionexBalanceWrapper>(request, null, ct).ConfigureAwait(false);
            return !result.Success ? HttpResult.Fail<PionexBalance[]>(result) : HttpResult.Ok(result, result.Data.Balances);
        }

        #endregion

        #region Get Full Balances

        /// <inheritdoc />
        public async Task<HttpResult<PionexBalanceDetails>> GetFullBalancesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/api/v1/wallet/balancesFull", PionexExchange.RateLimiter.Pionex, 1, true);
            var result = await _baseClient.SendAsync<PionexBalanceDetails>(request, null, ct).ConfigureAwait(false);
            return result;
        }

        #endregion

    }
}
