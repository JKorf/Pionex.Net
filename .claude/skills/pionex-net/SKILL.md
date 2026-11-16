---
name: pionex-net
description: Generate and troubleshoot C#/.NET code consuming Pionex.Net for Spot market data, balances, orders, public/private WebSocket streams, local books, and Shared API V2 integrations.
---

# Pionex.Net

Install the consumer package:

```sh
dotnet add package Pionex.Net
```

Use `PionexRestClient.SpotApi.{ExchangeData|Account|Trading}` and `PionexSocketClient.SpotApi`. This library exposes Spot; do not invent futures, margin, bot-management, transfer or withdrawal endpoints. Reuse and dispose clients, await calls, and retain successful subscriptions for unsubscribe cleanup.

Credentials are `PionexCredentials(key, secret)` with no passphrase. Public market data needs no credentials; private socket streams authenticate directly through client credentials without listen keys.

Native symbols use `BASE_QUOTE`, e.g. `BTC_USDT`. Filtered ticker and book-ticker calls return arrays. Native ticker price is `ClosePrice`; Shared ticker price is `LastPrice`. Market buys use quote quantity, while market sells use base quantity.

Check `Success` before `Data`: REST returns `HttpResult`, subscriptions return `WebSocketResult<UpdateSubscription>`, and some Shared helpers return `ExchangeCallResult`, with generic variants where applicable. Placement and cancellation change live account state; there is no test-order endpoint.

Use `.SharedApi` and narrow V2 capabilities for portable operations. `IPionexSharedApiClient` groups `SpotRest` and `SpotSocket`. Socket capabilities cover book tickers, trades, books, orders, balances and user trades; they do not expose ticker/kline subscriptions or order requests. Native public streams provide trades and order books.

## Read when needed

- [API surfaces](references/api-surfaces.md): routes, exact capabilities, symbols, catalogs, quantities and factories.
- [Usage](references/usage.md): setup, authentication, account reads, live-order syntax, subscriptions, Shared API V2 and DI.
- [Operational considerations](references/safety.md): quantity semantics, execution, private streams and uncertain submissions.

For work on the library itself, use [repository instructions](../../../AGENTS.md) (checkout-only).
