# Pionex operational considerations

## Credentials and scope

Use `PionexCredentials(key, secret)` from secret storage. Private account/order/balance streams use socket credentials directly; there is no listen-key lifecycle to create or refresh. Account read permission does not imply trading permission. Only Live is built in; custom addresses do not imply a sandbox.

## Execution and sizing

`PlaceOrderAsync` submits a live order; the interface has no validation-only/test-order method. Market buys spend quote assets via `quoteQuantity`; market sells use base `quantity`. Validate symbol enablement, precision, minimum/maximum quantities, price bounds and balances for the requested operation.

A limit order can fill immediately, including at a price far from a previously observed ticker. Cancellation can race with fills. Reconcile order status, executed quantity and trades to establish remaining exposure. `CancelAllOrdersAsync` affects the selected symbol's open orders; cancellation success is not evidence that no fills occurred.

Retain client/order identifiers for correlation. Use `GetOrderByClientOrderIdAsync` when the exchange order ID is unknown. There is no native batch-placement workflow or futures position-mode control; inspect each result separately when submitting several orders in application code.

## Streams and account details

Public streams expose trades/order books; private streams expose orders, user trades and balances. Subscription success confirms setup, not future delivery or continued synchronization. Depth must respect the documented maximum of 100. Keep the subscription owned for the intended service lifetime and unsubscribe during cleanup. Full balances include bot/trader-account details; this does not authorize or provide bot-management operations. Clear cached user clients after credential changes.

## Uncertain submissions and retries

A timeout or connection loss can leave an order or cancellation accepted but unconfirmed. `Error.IsTransient` alone does not justify repeating it. Reconcile retained client/order IDs, open orders and trades before deciding whether to resubmit; allow for status propagation delays. Bounded backoff is appropriate for transient public reads. Keep a generic read-retry helper away from account-changing calls.
