// 03-websocket.cs
//
// Demonstrates: public and authenticated subscriptions, result checks, and
// unsubscribe during shutdown.
//
// Setup: dotnet add package Pionex.Net

using Pionex.Net;
using Pionex.Net.Clients;

const string symbol = "BTC_USDT";

var publicSocket = new PionexSocketClient();

var tradeSubscription = await publicSocket.SpotApi.SubscribeToTradeUpdatesAsync(
    symbol,
    update =>
    {
        foreach (var trade in update.Data)
            Console.WriteLine($"{trade.Symbol} {trade.Side}: {trade.Quantity} at {trade.Price}");
    });

if (!tradeSubscription.Success)
{
    Console.WriteLine($"Trade subscription failed: {tradeSubscription.Error}");
    return;
}

var bookSubscription = await publicSocket.SpotApi.SubscribeToOrderBookUpdatesAsync(
    symbol,
    depth: 20,
    update =>
    {
        var bestBid = update.Data.Bids.FirstOrDefault()?.Price;
        var bestAsk = update.Data.Asks.FirstOrDefault()?.Price;
        Console.WriteLine($"Book: bid={bestBid}, ask={bestAsk}");
    });

if (!bookSubscription.Success)
{
    Console.WriteLine($"Order book subscription failed: {bookSubscription.Error}");
    await publicSocket.UnsubscribeAsync(tradeSubscription.Data);
    return;
}

// Private streams authenticate with credentials configured on the socket client.
var privateSocket = new PionexSocketClient(options =>
{
    options.ApiCredentials = new PionexCredentials("API_KEY", "API_SECRET");
});

var balanceSubscription = await privateSocket.SpotApi.SubscribeToBalanceUpdatesAsync(
    update =>
    {
        foreach (var balance in update.Data)
            Console.WriteLine($"{balance.Asset}: free={balance.Free}, frozen={balance.Frozen}");
    });

if (!balanceSubscription.Success)
    Console.WriteLine($"Balance subscription failed: {balanceSubscription.Error}");

await Task.Delay(TimeSpan.FromSeconds(30));

await publicSocket.UnsubscribeAsync(bookSubscription.Data);
await publicSocket.UnsubscribeAsync(tradeSubscription.Data);

if (balanceSubscription.Success)
    await privateSocket.UnsubscribeAsync(balanceSubscription.Data);
