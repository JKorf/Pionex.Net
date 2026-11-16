# Pionex.Net usage

## Imports

```csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.SharedApis;
using Pionex.Net;
using Pionex.Net.Clients;
using Pionex.Net.Enums;
using Pionex.Net.Interfaces.Clients;
using Microsoft.Extensions.DependencyInjection;
```

## Public setup and results

```csharp
using var client = new PionexRestClient();
var result = await client.SpotApi.ExchangeData.GetTickersAsync("BTC_USDT", SymbolType.Spot);
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
foreach (var ticker in result.Data)
    Console.WriteLine(ticker.ClosePrice);
```

Filtered ticker calls return arrays; `ClosePrice` is the native current price.

## Credentials and balances

```csharp
var key = Environment.GetEnvironmentVariable("PIONEX_KEY")
    ?? throw new InvalidOperationException("API key is required");
var secret = Environment.GetEnvironmentVariable("PIONEX_SECRET")
    ?? throw new InvalidOperationException("API secret is required");
using var client = new PionexRestClient(options =>
{
    options.ApiCredentials = new PionexCredentials(key, secret);
});
var result = await client.SpotApi.Account.GetBalancesAsync();
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
foreach (var balance in result.Data.Where(x => x.Free != 0 || x.Frozen != 0))
    Console.WriteLine($"{balance.Asset}: free={balance.Free}, frozen={balance.Frozen}");
```

## Symbol metadata

```csharp
var result = await client.SpotApi.ExchangeData.GetSymbolsAsync(
    symbols: "ETH_USDT", symbolType: SymbolType.Spot);
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
foreach (var symbol in result.Data)
    Console.WriteLine($"Enabled={symbol.Enable}, precision={symbol.QuantityPrecision}, min={symbol.MinSpotQuantity}");
```

## Live order lifecycle

This assumes an authenticated client as above. It submits an executable order; see [execution considerations](safety.md#execution-and-sizing) before running and size against current metadata.

```csharp
var order = await client.SpotApi.Trading.PlaceOrderAsync(
    symbol: "ETH_USDT", side: OrderSide.Buy, type: OrderType.Limit,
    quantity: 0.01m, price: 2000m, clientOrderId: Guid.NewGuid().ToString("N"));
if (!order.Success)
{
    Console.WriteLine(order.Error);
    return;
}
var details = await client.SpotApi.Trading.GetOrderAsync(order.Data.OrderId);
if (!details.Success)
    Console.WriteLine(details.Error);
else
    Console.WriteLine(details.Data);
var cancel = await client.SpotApi.Trading.CancelOrderAsync("ETH_USDT", order.Data.OrderId);
if (!cancel.Success)
    Console.WriteLine(cancel.Error);
```

## Public WebSocket lifecycle

```csharp
using var socketClient = new PionexSocketClient();
var subscription = await socketClient.SpotApi.SubscribeToTradeUpdatesAsync(
    "BTC_USDT", update => Console.WriteLine(update.Data.Length));
if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}
try
{
    await Task.Delay(TimeSpan.FromSeconds(30));
}
finally
{
    await socketClient.UnsubscribeAsync(subscription.Data);
}
```

## Private balance stream

```csharp
var key = Environment.GetEnvironmentVariable("PIONEX_KEY")
    ?? throw new InvalidOperationException("API key is required");
var secret = Environment.GetEnvironmentVariable("PIONEX_SECRET")
    ?? throw new InvalidOperationException("API secret is required");
using var socketClient = new PionexSocketClient(options =>
{
    options.ApiCredentials = new PionexCredentials(key, secret);
});
var subscription = await socketClient.SpotApi.SubscribeToBalanceUpdatesAsync(
    update => Console.WriteLine(update.Data.Length));
if (!subscription.Success)
{
    Console.WriteLine(subscription.Error);
    return;
}
try
{
    await Task.Delay(TimeSpan.FromSeconds(30));
}
finally
{
    await socketClient.UnsubscribeAsync(subscription.Data);
}
```

No listen key is needed.

## Shared API V2

```csharp
IGetTickerRest ticker = client.SpotApi.SharedApi;
var result = await ticker.GetTickerAsync(new GetTickerRequest(
    new SharedSymbol(TradingMode.Spot, "BTC", "USDT")));
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.LastPrice);
```

Load symbols before reading the catalog:

```csharp
IGetSpotSymbolsRest symbolsClient = client.SpotApi.SharedApi;
var result = await symbolsClient.GetSpotSymbolsAsync(new GetSymbolsRequest());
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
foreach (var symbol in result.Data.Take(5))
    Console.WriteLine($"{symbol.Name}: {symbol.BaseAsset}/{symbol.QuoteAsset}");
var catalog = symbolsClient.SpotSymbolCatalog;
```

## Dependency injection and dynamic lookup

```csharp
var services = new ServiceCollection();
services.AddPionex();
using var provider = services.BuildServiceProvider();
var aggregate = provider.GetRequiredService<IPionexSharedApiClient>();
var match = aggregate.GetCapability(
    SharedCapabilities.Tickers.GetTicker.Rest, tradingMode: TradingMode.Spot);
if (match is null)
    return;
var result = await match.Capability.GetTickerAsync(new GetTickerRequest(
    new SharedSymbol(TradingMode.Spot, "BTC", "USDT")));
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}
Console.WriteLine(result.Data.LastPrice);
```

Set `options.ApiCredentials` through `services.AddPionex(options => { ... })` for authenticated consumers, or configure REST/socket credentials independently. Inject `IPionexRestClient`, `IPionexSocketClient`, the aggregate or registered narrow capabilities. [API routing](api-surfaces.md#shared-api-v2) describes multiple-exchange selection and supported socket interfaces.

## Complete programs (checkout-only)

[Examples](../../../../Examples/ai-friendly/README.md) contains independent public/account, trading, socket, Shared API V2 and public-read retry programs. These links require a checkout; the snippets above work with the installed package.
