# Copilot Instructions for Pionex.Net

This repository is **Pionex.Net**, a strongly typed C#/.NET client for the Pionex REST and WebSocket APIs and part of the CryptoExchange.Net ecosystem.

When generating or changing code, treat interfaces under `Pionex.Net/Interfaces/Clients/` as the source of truth.

## Scope

Pionex.Net currently implements Spot only. Do not generate futures, perpetual, margin, options, earn, bot-management, or derivatives client calls.

Use `PionexRestClient` and `PionexSocketClient`, not raw `HttpClient` or hand-written WebSocket protocol code.

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

Reuse clients or register them with `services.AddPionex(...)`.

## Results

REST methods return `HttpResult<T>` or `HttpResult`. WebSocket subscriptions return `WebSocketResult<UpdateSubscription>`. Check `.Success` before accessing `.Data`.

```csharp
var result = await publicClient.SpotApi.ExchangeData.GetTickersAsync("BTC_USDT");
if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}

var closePrice = result.Data.Single().ClosePrice;
```

Only retry failures for which `result.Error?.IsTransient == true`.

## API Structure

- `restClient.SpotApi.ExchangeData`: server time, symbols, recent trades, order books, tickers, book tickers, and klines
- `restClient.SpotApi.Account`: balances and full balance details
- `restClient.SpotApi.Trading`: order placement, lookup, cancellation, order history, and user trades
- `socketClient.SpotApi`: public trade/order-book subscriptions and private order/user-trade/balance subscriptions
- `.SpotApi.SharedClient`: CryptoExchange.Net SharedApis

## Pionex-Specific Rules

- Direct symbols use underscores: `BTC_USDT`.
- `GetTickersAsync` and `GetBookTickersAsync` return arrays even with one symbol.
- Direct `PionexTicker` uses `ClosePrice`, not `LastPrice`.
- Limit orders use `quantity` plus `price`.
- Market buys use `quoteQuantity`; market sells use `quantity`.
- Private socket streams authenticate using `PionexCredentials` configured on `PionexSocketClient`.
- There is no listen-key workflow.
- Direct sockets expose trades and order books publicly; do not invent ticker or kline subscriptions.

## Order Example

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

Validate symbol precision and limits with `GetSymbolsAsync` before real trading.

## WebSocket Example

```csharp
var socketClient = new PionexSocketClient();
var sub = await socketClient.SpotApi.SubscribeToTradeUpdatesAsync(
    "BTC_USDT",
    update =>
    {
        foreach (var trade in update.Data)
            Console.WriteLine(trade.Price);
    });

if (!sub.Success) { Console.WriteLine(sub.Error); return; }
await socketClient.UnsubscribeAsync(sub.Data);
```

Store every successful `UpdateSubscription` and unsubscribe during shutdown.

## Cross-Exchange Code

Use `CryptoExchange.Net.SharedApis` through `.SharedClient`:

```csharp
using CryptoExchange.Net.SharedApis;

var shared = new PionexRestClient().SpotApi.SharedClient;
var ticker = await shared.GetSpotTickerAsync(
    new GetTickerRequest(new SharedSymbol(TradingMode.Spot, "BTC", "USDT")));

if (!ticker.Success) { Console.WriteLine(ticker.Error); return; }
```

Call `shared.Discover()` before dynamically selecting capabilities.

## Avoid

- Raw Pionex HTTP or WebSocket code
- Generic `ApiCredentials`; use `PionexCredentials`
- Symbols such as `BTCUSDT`, `BTC-USDT`, or `BTC/USDT` in direct calls
- Reading `.Data` without checking `.Success`
- `.Result` and `.Wait()`
- Creating clients per request
- Nonexistent futures, derivatives, ticker-stream, kline-stream, or listen-key APIs
- Enums or models copied from other exchange libraries

Detailed guidance is in `AGENTS.md` and `llms-full.txt`. Runnable examples are in `Examples/ai-friendly/`.
