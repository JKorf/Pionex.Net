using Pionex.Net.Clients;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Pionex.Net.Objects.Options;
using Microsoft.Extensions.Options;

// REST
var restClient = new PionexRestClient();
var ticker = await restClient.SpotApi.ExchangeData.GetTickersAsync("ETH_USDT");
if (!ticker.Success)
{
    Console.WriteLine($"Failed to get ticker: {ticker.Error}");
    return;
}

Console.WriteLine($"Rest client ticker price for ETH_USDT: {ticker.Data.Single().ClosePrice}");

Console.WriteLine();
Console.WriteLine("Press enter to start websocket subscription");
Console.ReadLine();

// Websocket
// Optional, manually add logging
var logFactory = new LoggerFactory();
logFactory.AddProvider(new TraceLoggerProvider());

var socketClient = new PionexSocketClient(Options.Create(new PionexSocketOptions { }), logFactory);
var subscription = await socketClient.SpotApi.SubscribeToTradeUpdatesAsync("ETH_USDT", update =>
{
    Console.WriteLine($"Websocket client trade for ETH_USDT: {update.Data.First().Price}");
});

if (!subscription.Success)
{
    Console.WriteLine($"Failed to subscribe to trade updates: {subscription.Error}");
    return;
}

Console.ReadLine();
