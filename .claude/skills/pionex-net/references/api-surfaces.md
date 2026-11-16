# Pionex.Net API surfaces

## Client roots

`PionexRestClient` / `IPionexRestClient` expose `SpotApi.{ExchangeData|Account|Trading}`. `PionexSocketClient` / `IPionexSocketClient` exposes streams directly on `SpotApi`. This library has no futures, margin or bot-management root; full balances may report bot-account details without exposing bot-control APIs.

## REST Exchange Data

`client.SpotApi.ExchangeData`:

- `GetServerTimeAsync()`
- `GetSymbolsAsync(symbols, symbolType)`
- `GetRecentTradesAsync(symbol, limit)`
- `GetOrderBookAsync(symbol, limit)`
- `GetTickersAsync(symbol, type)`
- `GetBookTickersAsync(symbol, type)`
- `GetKlinesAsync(symbol, interval, endTime, limit)`

`GetTickersAsync` and `GetBookTickersAsync` return arrays.

## REST Account

`client.SpotApi.Account`:

- `GetBalancesAsync()`
- `GetFullBalancesAsync()`

`GetBalancesAsync` returns `PionexBalance[]` with `Asset`, `Free`, and `Frozen`.

`GetFullBalancesAsync` returns wallet totals, asset prices, bot-account details, and trader-account details.

## REST Trading

`client.SpotApi.Trading`:

- `PlaceOrderAsync(symbol, side, type, quantity, quoteQuantity, price, immediateOrCancel, clientOrderId)`
- `GetOrderAsync(orderId)`
- `GetOrderByClientOrderIdAsync(clientOrderId)`
- `CancelOrderAsync(symbol, orderId)`
- `GetOpenOrdersAsync(symbol)`
- `GetOrdersAsync(symbol, startTime, endTime, limit)`
- `CancelAllOrdersAsync(symbol)`
- `GetUserTradesAsync(symbol, startTime, endTime)`
- `GetOrderTradesAsync(orderId, fromId)`

There is no test-order method.

## Socket Streams

`socket.SpotApi`:

- `SubscribeToTradeUpdatesAsync(symbol, handler)` returns updates containing `PionexTrade[]`
- `SubscribeToOrderBookUpdatesAsync(symbol, depth, handler)` returns `PionexOrderBook`
- `SubscribeToOrderUpdatesAsync(symbol, handler)` returns `PionexOrder`
- `SubscribeToUserTradeUpdatesAsync(symbol, handler)` returns `PionexUserTrade`
- `SubscribeToBalanceUpdatesAsync(handler)` returns `PionexBalance[]`

The source documents order-book subscription depth as at most 100.

Private streams authenticate through socket client credentials. Do not create a listen key.

## Native Symbols and Models

- Native symbol format: `BASE_QUOTE`
- Examples: `BTC_USDT`, `ETH_USDT`
- Native ticker current/close property: `ClosePrice`
- Native order book sides: `Bids` and `Asks`
- Native order book entry properties: `Price` and `Quantity`

Use `Pionex.Net.Enums.OrderSide`, `OrderType`, `SymbolType`, and `KlineInterval`.

## Shared API V2

`IPionexSharedApiClient` exposes these aggregate properties; native Spot REST/socket surfaces expose the corresponding `.SharedApi`.

- `SpotRest` (`IPionexRestClientSpotSharedApi`): `IGetBalancesRest`, `IGetBookTickerRest`, `IGetKlinesRest`, `IGetOrderBookRest`, `IGetRecentTradesRest`, `IGetSpotSymbolsRest`, `IGetTickerRest`, `IGetAllTickersRest`, `IPlaceSpotOrderRest`, `IGetSpotOrderRest`, `IGetOpenSpotOrdersRest`, `IGetClosedSpotOrdersRest`, `IGetSpotOrderTradesRest`, `IGetSpotUserTradeHistoryRest`, `ICancelSpotOrderRest`.
- `SpotSocket` (`IPionexSocketClientSpotSharedApi`): `ISubscribeTradesSocket`, `ISubscribeOrderBookSocket`, `ISubscribeBookTickerSocket`, `ISubscribeSpotOrdersSocket`, `ISubscribeUserTradesSocket`, `ISubscribeBalancesSocket`.

For known workflows use aggregate properties or inject narrow capabilities. For dynamic selection call `aggregate.GetCapability(SharedCapabilities.Tickers.GetTicker.Rest, tradingMode: TradingMode.Spot)` and check for null before using `Capability` and `Options`. Descriptors identify operations, not guarantees of support. `GetCapabilities` enumerates matches and `Discover` summarizes metadata. With several exchanges registered, inject `IEnumerable<TCapability>` and select by exchange, supported mode and options.

`SpotSocket` has no `ISubscribeTickerSocket` or kline subscription capability; use its book-ticker/trade/book interfaces for supported portable streams. It also has no place/cancel socket request capability.

## Catalogs and quantity representation

Use `SharedSymbol(TradingMode.Spot, "BTC", "USDT")` or `PionexExchange.FormatSymbol` for reusable formatting. Load `IGetSpotSymbolsRest.GetSpotSymbolsAsync(new GetSymbolsRequest())`, check success, then read `SpotSymbolCatalog`. Requests honor base/quote asset type/subtype filters. Returned metadata includes display names, availability, precision, quantity bounds and crypto/stablecoin/TradFi equity/commodity classifications where populated.

Shared books use `SharedQuantityType.BaseAsset`. Shared results use `SharedOrderQuantity`; select base/quote fields according to the operation and supported quantity options. Native market buys require `quoteQuantity`; market sells require `quantity`. Inspect `Enable`, quantity/quote precision, min/max quantity and current price bounds before sizing live orders.

## Factories and environment

`IPionexOrderBookFactory` creates Spot local books from native or Shared symbols; check `StartAsync` and stop the book in cleanup. `IPionexTrackerFactory` provides trackers. `IPionexUserClientProvider` caches per-user clients; clear them when credentials change. `PionexEnvironment.Live` is the only built-in environment; `CreateCustom(name, restAddress, socketAddress)` configures explicit addresses rather than an official testnet.

## Checkout-only sources

- [Public interfaces](../../../../Pionex.Net/Interfaces/Clients/): signatures and cancellation tokens (checkout-only).
- [Client implementations](../../../../Pionex.Net/Clients/): Shared options, conversions and classifications (checkout-only).
- [Complete examples](../../../../Examples/ai-friendly/): independent consumer programs (checkout-only).
