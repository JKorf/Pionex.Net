using CryptoExchange.Net.SharedApis;

namespace Pionex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Shared interface for Spot socket API usage
    /// </summary>
    public interface IPionexSocketClientSpotApiShared :
        ITradeSocketClient,
        IOrderBookSocketClient,
        IBookTickerSocketClient,
        ISpotOrderSocketClient,
        IUserTradeSocketClient,
        IBalanceSocketClient
    {
    }

    /// <summary>
    /// Shared API interface. Shared APIs provide a common,
    /// exchange-independent contract for accessing functionality across different
    /// exchange client libraries.
    /// </summary>
    public interface IPionexSocketClientSpotSharedApi :
        ISubscribeTradesOperation,
        ISubscribeOrderBookOperation,
        ISubscribeBookTickerOperation,
        ISubscribeSpotOrdersOperation,
        ISubscribeUserTradesOperation,
        ISubscribeBalancesOperation
    {
    }
}
