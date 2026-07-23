# AI-Friendly Examples

These self-contained examples are optimized for coding assistants and quick
onboarding. They use the current Pionex.Net client layout, check result objects
before accessing data, and call out Pionex-specific behavior.

Pionex.Net currently exposes the Pionex spot API. Symbols use an underscore
between assets, such as `BTC_USDT`.

## Files

| File | What it shows |
|---|---|
| `01-spot-quickstart.cs` | Public ticker and order book, credentials, and balances |
| `02-spot-trading.cs` | Limit order placement, lookup, and cancellation |
| `03-websocket.cs` | Public trades/order book, private balances, and teardown |
| `04-multi-exchange.cs` | Exchange-agnostic access through `CryptoExchange.Net.SharedApis` |
| `05-error-handling.cs` | `HttpResult<T>`, error metadata, and transient retry |

## Running

```bash
dotnet new console -n MyPionexApp
cd MyPionexApp
dotnet add package Pionex.Net
# Copy one example .cs file into Program.cs
# Replace API_KEY and API_SECRET for private endpoints
dotnet run
```

Private examples can place real orders. Use credentials with only the permissions
you need, validate symbol rules and balances first, and review example values
before running them.
