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
}
