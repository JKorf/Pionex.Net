---
name: pionex-net
description: Use Pionex.Net when generating C#/.NET code for the Pionex cryptocurrency exchange, including Spot REST endpoints, public or private WebSocket subscriptions, account balances, market data, order placement, local order books, or CryptoExchange.Net SharedApis.
---

# Pionex.Net Skill

## Quick Decision

For Pionex API access in C#/.NET, use `Pionex.Net`. Do not generate raw `HttpClient` or hand-written WebSocket code. The library handles signing, serialization, result models, rate limiting integration, and connection management.

Pionex.Net currently implements Spot APIs only. Do not invent futures, perpetual, margin, options, earn, grid-bot, or derivatives clients.

For exchange-agnostic code, use `CryptoExchange.Net.SharedApis` through `.SpotApi.SharedClient`. Call `Discover()` when runtime capability or request-option metadata is needed.

## Installation

```bash
dotnet add package Pionex.Net
```

Targets: netstandard2.0, netstandard2.1, net8.0, net9.0, and net10.0. Native AOT is supported on compatible targets.

## Client Setup

```csharp
using Pionex.Net;
using Pionex.Net.Clients;

var publicClient = new PionexRestClient();

var privateClient = new PionexRestClient(options =>
{
    options.ApiCredentials = new PionexCredentials("API_KEY", "API_SECRET");
});
```

Reuse clients. For applications with dependency injection, prefer `AddPionex`.

## Result Handling

REST methods return `HttpResult<T>` or `HttpResult`. WebSocket subscription methods return `WebSocketResult<UpdateSubscription>`. Always check `.Success` before accessing `.Data`.

```csharp
var result = await publicClient.SpotApi.ExchangeData.GetTickersAsync("BTC_USDT");
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}

var price = result.Data.Single().ClosePrice;
```

Normal API failures are represented by `.Error`; do not assume they are thrown. Retry only when `result.Error?.IsTransient == true`.

## API Surface

```text
restClient.SpotApi.ExchangeData  public server time, symbols, trades, books,
                                 tickers, book tickers, and klines
restClient.SpotApi.Account       balances and full wallet/bot/trader balances
restClient.SpotApi.Trading       place, query, cancel orders and query fills
socketClient.SpotApi             public trades/order books and private order,
                                 fill, and balance subscriptions
restClient.SpotApi.SharedClient  shared REST interfaces
socketClient.SpotApi.SharedClient shared socket interfaces
```

There is no separate general, wallet, futures, margin, or options API root.

## Symbols and Enums

Direct Pionex methods use exchange symbols with an underscore:

```csharp
const string symbol = "BTC_USDT";
```

Do not use `BTCUSDT`, `BTC-USDT`, or `BTC/USDT` for direct calls. SharedApis uses a structured symbol:

```csharp
var symbol = new SharedSymbol(TradingMode.Spot, "BTC", "USDT");
```

Use Pionex enums such as `OrderSide`, `OrderType`, `SymbolType`, and `KlineInterval`. Do not substitute enums from another exchange library.

## Market Data

`GetTickersAsync` returns an array even with a symbol filter:

```csharp
using Pionex.Net.Enums;

var tickerResult = await publicClient.SpotApi.ExchangeData.GetTickersAsync(
    "BTC_USDT",
    SymbolType.Spot);

if (!tickerResult.Success) { Console.WriteLine(tickerResult.Error); return; }
var ticker = tickerResult.Data.Single();
Console.WriteLine(ticker.ClosePrice);
```

Use `ClosePrice`, not an invented `LastPrice`, on `PionexTicker`.

## Order Placement

```csharp
using Pionex.Net.Enums;

var order = await privateClient.SpotApi.Trading.PlaceOrderAsync(
    symbol: "ETH_USDT",
    side: OrderSide.Buy,
    type: OrderType.Limit,
    quantity: 0.01m,
    price: 2000m);

if (!order.Success) { Console.WriteLine(order.Error); return; }
Console.WriteLine(order.Data.OrderId);
```

Order parameter rules:

- Limit orders use `quantity` and `price`.
- Market buys use `quoteQuantity` because the amount is denominated in the quote asset.
- Market sells use `quantity` because the amount is denominated in the base asset.
- `immediateOrCancel` and `clientOrderId` are optional.
- Validate symbol precision, size limits, price bounds, and balances before placing real orders.

Order lookup is `GetOrderAsync(long orderId)`. Cancellation requires both symbol and order ID:

```csharp
await privateClient.SpotApi.Trading.CancelOrderAsync("ETH_USDT", order.Data.OrderId);
```

## WebSocket Subscriptions

Public trade and order-book streams do not require credentials:

```csharp
var socketClient = new PionexSocketClient();

var subscription = await socketClient.SpotApi.SubscribeToTradeUpdatesAsync(
    "BTC_USDT",
    update =>
    {
        foreach (var trade in update.Data)
            Console.WriteLine(trade.Price);
    });

if (!subscription.Success) { Console.WriteLine(subscription.Error); return; }
await socketClient.UnsubscribeAsync(subscription.Data);
```

Direct public subscriptions are `SubscribeToTradeUpdatesAsync` and `SubscribeToOrderBookUpdatesAsync`. There are no direct ticker or kline subscription methods.

Private subscriptions authenticate from the socket client credentials; Pionex.Net does not use REST-created listen keys:

```csharp
var privateSocket = new PionexSocketClient(options =>
{
    options.ApiCredentials = new PionexCredentials("API_KEY", "API_SECRET");
});

var balanceSub = await privateSocket.SpotApi.SubscribeToBalanceUpdatesAsync(
    update => Console.WriteLine(update.Data.Length));
```

Other private streams are `SubscribeToOrderUpdatesAsync(symbol, handler)` and `SubscribeToUserTradeUpdatesAsync(symbol, handler)`.

## SharedApis

```csharp
using CryptoExchange.Net.SharedApis;

ISpotTickerRestClient shared = new PionexRestClient().SpotApi.SharedClient;
var symbol = new SharedSymbol(TradingMode.Spot, "BTC", "USDT");

var ticker = await shared.GetSpotTickerAsync(new GetTickerRequest(symbol));
if (!ticker.Success) { Console.WriteLine(ticker.Error); return; }
Console.WriteLine(ticker.Data.LastPrice);
```

Pionex shared REST support includes balances, book tickers, klines, order books, recent trades, Spot symbols, Spot tickers, and Spot orders. Shared socket support includes trades, book tickers, order books, Spot orders, user trades, and balances. Use `Discover()` rather than assuming a shared capability exists.

## Dependency Injection

```csharp
using Microsoft.Extensions.DependencyInjection;
using Pionex.Net;

services.AddPionex(options =>
{
    options.ApiCredentials = new PionexCredentials("API_KEY", "API_SECRET");
});
```

Inject `IPionexRestClient` and `IPionexSocketClient`. Registration also exposes supported shared interfaces plus `IPionexOrderBookFactory`, `IPionexTrackerFactory`, and `IPionexUserClientProvider`.

## Environments

Only `PionexEnvironment.Live` is built in. No testnet environment is defined.

```csharp
var custom = PionexEnvironment.CreateCustom(
    "custom",
    "https://example-rest",
    "wss://example-socket");
```

Set `options.Environment` to the custom environment when alternate endpoints are intentionally required.

## Hard Rules

- Never write raw Pionex HTTP signing or WebSocket protocol code when Pionex.Net covers the operation.
- Never use symbols without the underscore for direct API calls.
- Never read `.Data` before checking `.Success`.
- Never use `.Result` or `.Wait()`; keep the flow asynchronous.
- Never instantiate a client per request; reuse it or use DI.
- Never use generic `ApiCredentials`; use `PionexCredentials`.
- Never invent futures or derivatives clients.
- Never invent ticker or kline direct socket subscriptions.
- Never add a listen-key step for private Pionex sockets.
- Never use `LastPrice` on direct `PionexTicker`; use `ClosePrice`.
- Never assume single-symbol ticker calls return a single object; they return arrays.
- Always keep and unsubscribe successful WebSocket subscriptions.
- Check `Pionex.Net/Interfaces/Clients/**` before using an unfamiliar method.

## Reference

- Full context: `llms-full.txt`
- Short index: `llms.txt`
- Compilable examples: `Examples/ai-friendly/`
- Source interfaces: `Pionex.Net/Interfaces/Clients/`
- Repository: https://github.com/JKorf/Pionex.Net
- Pionex API docs: https://www.pionex.com/docs/api-docs
