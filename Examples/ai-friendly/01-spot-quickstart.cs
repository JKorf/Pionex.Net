// 01-spot-quickstart.cs
//
// Demonstrates: public market data, authenticated client setup, and balances.
//
// Setup: dotnet add package Pionex.Net

using Pionex.Net;
using Pionex.Net.Clients;
using Pionex.Net.Enums;

const string symbol = "BTC_USDT";

// Public endpoints do not require API credentials. Reuse client instances in
// production instead of creating a new client for every request.
var publicClient = new PionexRestClient();

var tickerResult = await publicClient.SpotApi.ExchangeData.GetTickersAsync(
    symbol,
    SymbolType.Spot);

if (!tickerResult.Success)
{
    Console.WriteLine($"Ticker request failed: {tickerResult.Error}");
    return;
}

// GetTickersAsync always returns an array, even when filtering by one symbol.
var ticker = tickerResult.Data.Single();
Console.WriteLine($"{ticker.Symbol} close price: {ticker.ClosePrice}");

var orderBookResult = await publicClient.SpotApi.ExchangeData.GetOrderBookAsync(
    symbol,
    limit: 20);

if (!orderBookResult.Success)
{
    Console.WriteLine($"Order book request failed: {orderBookResult.Error}");
    return;
}

Console.WriteLine($"Best bid: {orderBookResult.Data.Bids.FirstOrDefault()?.Price}");
Console.WriteLine($"Best ask: {orderBookResult.Data.Asks.FirstOrDefault()?.Price}");

// Account and trading endpoints require an API key and secret.
var privateClient = new PionexRestClient(options =>
{
    options.ApiCredentials = new PionexCredentials("API_KEY", "API_SECRET");
});

var balancesResult = await privateClient.SpotApi.Account.GetBalancesAsync();
if (!balancesResult.Success)
{
    Console.WriteLine($"Balance request failed: {balancesResult.Error}");
    return;
}

foreach (var balance in balancesResult.Data.Where(x => x.Free != 0 || x.Frozen != 0))
    Console.WriteLine($"{balance.Asset}: free={balance.Free}, frozen={balance.Frozen}");
