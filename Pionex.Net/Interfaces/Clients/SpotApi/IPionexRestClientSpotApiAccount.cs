using CryptoExchange.Net.Objects;
using Pionex.Net.Objects.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Pionex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Pionex Spot account endpoints. Account endpoints include balance info, withdraw/deposit info and requesting and account settings
    /// </summary>
    public interface IPionexRestClientSpotApiAccount
    {
        /// <summary>
        /// Get account balances
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/account" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/account/balances<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexBalance[]>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get account details
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/wallet-api/wallet" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/wallet/balancesFull<br />
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexBalanceDetails>> GetFullBalancesAsync(CancellationToken ct = default);

    }
}
