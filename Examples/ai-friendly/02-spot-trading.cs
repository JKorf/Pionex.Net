// 02-spot-trading.cs
//
// Demonstrates: authenticated spot order placement, lookup, and cancellation.
// Pionex.Net currently exposes the Pionex spot API; it has no derivatives API.
//
// Setup: dotnet add package Pionex.Net

using Pionex.Net;
using Pionex.Net.Clients;
using Pionex.Net.Enums;

var client = new PionexRestClient(options =>
{
    options.ApiCredentials = new PionexCredentials("API_KEY", "API_SECRET");
});

const string symbol = "ETH_USDT";

// Check symbol metadata, balances, and current prices before placing a real
// order. This deliberately low example price is not a safety guarantee.
var orderResult = await client.SpotApi.Trading.PlaceOrderAsync(
    symbol: symbol,
    side: OrderSide.Buy,
    type: OrderType.Limit,
    quantity: 0.01m,
    price: 100m,
    clientOrderId: $"example-{Guid.NewGuid():N}");

if (!orderResult.Success)
{
    Console.WriteLine($"Order placement failed: {orderResult.Error}");
    return;
}

Console.WriteLine($"Placed order id: {orderResult.Data.OrderId}");
Console.WriteLine($"Client order id: {orderResult.Data.ClientOrderId}");

var orderStatus = await client.SpotApi.Trading.GetOrderAsync(orderResult.Data.OrderId);
if (!orderStatus.Success)
{
    Console.WriteLine($"Order lookup failed: {orderStatus.Error}");
    return;
}

Console.WriteLine(
    $"Status: {orderStatus.Data.Status}, filled: {orderStatus.Data.QuantityFilled}");

var cancelResult = await client.SpotApi.Trading.CancelOrderAsync(
    symbol,
    orderResult.Data.OrderId);

if (!cancelResult.Success)
{
    Console.WriteLine($"Order cancellation failed: {cancelResult.Error}");
    return;
}

Console.WriteLine("Cancellation request accepted.");
