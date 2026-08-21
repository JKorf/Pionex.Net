# ![Pionex.Net](https://raw.githubusercontent.com/JKorf/Pionex.Net/main/Pionex.Net/Icon/icon.png) Pionex.Net  

[![.NET](https://img.shields.io/github/actions/workflow/status/JKorf/Pionex.Net/dotnet.yml?style=for-the-badge)](https://github.com/JKorf/Pionex.Net/actions/workflows/dotnet.yml) ![License](https://img.shields.io/github/license/JKorf/Pionex.Net?style=for-the-badge)
![Since](https://img.shields.io/badge/since-2026-brightgreen?style=for-the-badge)

[![Docs](https://img.shields.io/badge/Docs-Pionex.Net-1b7f50?style=for-the-badge)](https://cryptoexchange.jkorf.dev/docs/exchange-clients?library=Pionex.Net)

Pionex.Net is a client library for accessing the [Pionex REST and Websocket API](https://www.pionex.com/docs/api-docs). 

## Features
* Response data is mapped to descriptive models
* Input parameters and response values are mapped to discriptive enum values where possible
* High performance
* Automatic websocket (re)connection management 
* Client side rate limiting 
* Client side order book implementation
* Support for managing different accounts
* Extensive logging
* Support for different environments
* Easy integration with other exchange clients based on the CryptoExchange.Net base library
* Native AOT support

## Documentation

The [Pionex.Net documentation](https://cryptoexchange.jkorf.dev/docs/exchange-clients?library=Pionex.Net) is the main resource for installing, configuring, and using the library.

| Resource | Description |
|--|--|
| [Client guide](https://cryptoexchange.jkorf.dev/docs/exchange-clients?library=Pionex.Net) | Installation, REST and WebSocket clients, authentication, dependency injection, error handling, and advanced features |
| [Examples](https://cryptoexchange.jkorf.dev/docs/exchange-clients/examples?library=Pionex.Net) | Common REST and WebSocket operations |
| [API reference](https://cryptoexchange.jkorf.dev/docs/exchange-clients/reference?library=Pionex.Net) | Client interfaces, methods, and properties |
| [Shared API guide](https://cryptoexchange.jkorf.dev/docs/shared-api) | Common interfaces and models for working with multiple exchanges |

## Supported Frameworks
The library is targeting both `.NET Standard 2.0` and `.NET Standard 2.1` for optimal compatibility, as well as the latest dotnet versions to use the latest framework features.

|.NET implementation|Version Support|
|--|--|
|.NET Core|`2.0` and higher|
|.NET Framework|`4.6.1` and higher|
|Mono|`5.4` and higher|
|Xamarin.iOS|`10.14` and higher|
|Xamarin.Android|`8.0` and higher|
|UWP|`10.0.16299` and higher|
|Unity|`2018.1` and higher|

## Install the library

### NuGet 
[![NuGet version](https://img.shields.io/nuget/v/Pionex.net.svg?style=for-the-badge)](https://www.nuget.org/packages/Pionex.Net)  [![Nuget downloads](https://img.shields.io/nuget/dt/Pionex.Net.svg?style=for-the-badge)](https://www.nuget.org/packages/Pionex.Net)

	dotnet add package Pionex.Net
	
### GitHub packages
Pionex.Net is available on [GitHub packages](https://github.com/JKorf/Pionex.Net/pkgs/nuget/Pionex.Net). You'll need to add `https://nuget.pkg.github.com/JKorf/index.json` as a NuGet package source.

### Download release
[![GitHub Release](https://img.shields.io/github/v/release/JKorf/Pionex.Net?style=for-the-badge&label=GitHub)](https://github.com/JKorf/Pionex.Net/releases)

The NuGet package files are added along side the source with the latest GitHub release which can found [here](https://github.com/JKorf/Pionex.Net/releases).

## How to use
*Basic request:* 
```csharp
// Get the ETH/USDT ticker via rest request
var restClient = new PionexRestClient();
var tickerResult = await restClient.SpotApi.ExchangeData.GetTickersAsync("ETH_USDT");
var lastPrice = tickerResult.Data.Single().ClosePrice;
```

*Place order:*
```csharp
var restClient = new PionexRestClient(opts => {
	opts.ApiCredentials = new PionexCredentials("APIKEY", "APISECRET");
});

// Place Limit order to buy 0.1 ETH at 2000
var orderResult = await restClient.SpotApi.Trading.PlaceOrderAsync(
    "ETH_USDT",
    OrderSide.Buy,
    OrderType.Limit,
    0.1m,
    price: 2000
    );
```

*WebSocket subscription:* 
```csharp
// Subscribe to ETH/USDT ticker updates via the websocket API
var socketClient = new PionexSocketClient();
var tickerSubscriptionResult = socketClient.SpotApi.SubscribeToTradeUpdatesAsync("ETH_USDT", (update) =>
{
    var tradePrice = update.Data.First().Price;
});
```

For more examples and explanations, continue with the [Pionex.Net documentation](https://cryptoexchange.jkorf.dev/docs/exchange-clients?library=Pionex.Net) or browse the [compilable repository examples](https://github.com/JKorf/Pionex.Net/tree/main/Examples).

## AI / LLM documentation

Pionex.Net includes AI-oriented documentation and examples for code generation tools:

|File|Purpose|
|--|--|
|[`AGENTS.md`](AGENTS.md)|Assistant skill with core Pionex.Net patterns, pitfalls, and examples|
|[`llms.txt`](llms.txt)|Short LLM index with links to docs, examples, and critical usage rules|
|[`llms-full.txt`](llms-full.txt)|Detailed LLM context with endpoint routing, authentication levels, code patterns, and anti-hallucination checks|
|[`docs/ai-api-map.md`](docs/ai-api-map.md)|Table-style intent-to-method map|
|[`Examples/ai-friendly`](Examples/ai-friendly)|Compilable single-file examples for common REST, authentication/trading, WebSocket, DI/order book, and error handling workflows|

See [cryptoexchange-skills-hub](https://github.com/JKorf/cryptoexchange-skills-hub) for installable skills.

## Shared / unified API

The CryptoExchange.Net [Shared APIs](https://cryptoexchange.jkorf.dev/docs/shared-api) provide exchange-agnostic, unified interfaces for common operations such as retrieving tickers, order books and balances, placing orders, and subscribing to market updates.

This allows the same application code to work with different exchange libraries. The supported Pionex API surfaces expose their shared functionality through a `SharedClient` property. Because support differs between exchanges and API surfaces, call `Discover()` to inspect the available trading modes, environments, endpoints, and subscriptions at runtime.

### Supported shared interfaces

| API | Type | Supported interfaces |
|--|--|--|
| `SpotApi` | REST | `IBalanceRestClient`, `IBookTickerRestClient`, `IKlineRestClient`, `IOrderBookRestClient`, `IRecentTradeRestClient`, `ISpotOrderRestClient`, `ISpotSymbolRestClient`, `ISpotTickerRestClient` |
| `SpotApi` | WebSocket | `IBalanceSocketClient`, `IBookTickerSocketClient`, `IOrderBookSocketClient`, `ISpotOrderSocketClient`, `ITradeSocketClient`, `IUserTradeSocketClient` |

### Discover supported functionality

```csharp
var sharedClient = new PionexRestClient().SpotApi.SharedClient;
var clientInfo = sharedClient.Discover();

Console.WriteLine(clientInfo);
```

### Example

```csharp
using Pionex.Net.Clients;
using CryptoExchange.Net.SharedApis;

var sharedClient = new PionexRestClient().SpotApi.SharedClient;
ISpotTickerRestClient tickerClient = sharedClient;

var symbol = new SharedSymbol(TradingMode.Spot, "ETH", "USDT");
var result = await tickerClient.GetSpotTickerAsync(
    new GetTickerRequest(symbol));

if (!result.Success)
{
    Console.WriteLine(result.Error);
    return;
}

Console.WriteLine(result.Data.LastPrice);
```

The request and response models belong to `CryptoExchange.Net.SharedApis`, so the same pattern can be used with another exchange's `SharedClient`.

## CryptoExchange.Net
Pionex.Net is based on the [CryptoExchange.Net](https://github.com/JKorf/CryptoExchange.Net) base library. Other exchange API implementations based on the CryptoExchange.Net base library are available and follow the same logic.

CryptoExchange.Net also provides [shared access to different exchange APIs](https://cryptoexchange.jkorf.dev/docs/shared-api).

|Exchange|Repository|Nuget|
|--|--|--|
|Aster|[JKorf/Aster.Net](https://github.com/JKorf/Aster.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Aster.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Aster.Net)|
|Binance|[JKorf/Binance.Net](https://github.com/JKorf/Binance.Net)|[![Nuget version](https://img.shields.io/nuget/v/Binance.net.svg?style=flat-square)](https://www.nuget.org/packages/Binance.Net)|
|BingX|[JKorf/BingX.Net](https://github.com/JKorf/BingX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.BingX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.BingX.Net)|
|Bitfinex|[JKorf/Bitfinex.Net](https://github.com/JKorf/Bitfinex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitfinex.net.svg?style=flat-square)](https://www.nuget.org/packages/Bitfinex.Net)|
|Bitget|[JKorf/Bitget.Net](https://github.com/JKorf/Bitget.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Bitget.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Bitget.Net)|
|BitMart|[JKorf/BitMart.Net](https://github.com/JKorf/BitMart.Net)|[![Nuget version](https://img.shields.io/nuget/v/BitMart.net.svg?style=flat-square)](https://www.nuget.org/packages/BitMart.Net)|
|BitMEX|[JKorf/BitMEX.Net](https://github.com/JKorf/BitMEX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.BitMEX.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.BitMEX.Net)|
|Bitstamp|[JKorf/Bitstamp.Net](https://github.com/JKorf/Bitstamp.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bitstamp.Net.svg?style=flat-square)](https://www.nuget.org/packages/Bitstamp.Net)|
|BloFin|[JKorf/BloFin.Net](https://github.com/JKorf/BloFin.Net)|[![Nuget version](https://img.shields.io/nuget/v/BloFin.net.svg?style=flat-square)](https://www.nuget.org/packages/BloFin.Net)|
|Bybit|[JKorf/Bybit.Net](https://github.com/JKorf/Bybit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Bybit.net.svg?style=flat-square)](https://www.nuget.org/packages/Bybit.Net)|
|Coinbase|[JKorf/Coinbase.Net](https://github.com/JKorf/Coinbase.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Coinbase.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Coinbase.Net)|
|CoinEx|[JKorf/CoinEx.Net](https://github.com/JKorf/CoinEx.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinEx.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinEx.Net)|
|CoinGecko|[JKorf/CoinGecko.Net](https://github.com/JKorf/CoinGecko.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinGecko.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinGecko.Net)|
|CoinW|[JKorf/CoinW.Net](https://github.com/JKorf/CoinW.Net)|[![Nuget version](https://img.shields.io/nuget/v/CoinW.net.svg?style=flat-square)](https://www.nuget.org/packages/CoinW.Net)|
|Crypto.com|[JKorf/CryptoCom.Net](https://github.com/JKorf/CryptoCom.Net)|[![Nuget version](https://img.shields.io/nuget/v/CryptoCom.net.svg?style=flat-square)](https://www.nuget.org/packages/CryptoCom.Net)|
|DeepCoin|[JKorf/DeepCoin.Net](https://github.com/JKorf/DeepCoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/DeepCoin.net.svg?style=flat-square)](https://www.nuget.org/packages/DeepCoin.Net)|
|Gate.io|[JKorf/GateIo.Net](https://github.com/JKorf/GateIo.Net)|[![Nuget version](https://img.shields.io/nuget/v/GateIo.net.svg?style=flat-square)](https://www.nuget.org/packages/GateIo.Net)|
|HTX|[JKorf/HTX.Net](https://github.com/JKorf/HTX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.HTX.net.svg?style=flat-square)](https://www.nuget.org/packages/Jkorf.HTX.Net)|
|HyperLiquid|[JKorf/HyperLiquid.Net](https://github.com/JKorf/HyperLiquid.Net)|[![Nuget version](https://img.shields.io/nuget/v/HyperLiquid.Net.svg?style=flat-square)](https://www.nuget.org/packages/HyperLiquid.Net)|
|Kraken|[JKorf/Kraken.Net](https://github.com/JKorf/Kraken.Net)|[![Nuget version](https://img.shields.io/nuget/v/KrakenExchange.net.svg?style=flat-square)](https://www.nuget.org/packages/KrakenExchange.Net)|
|Kucoin|[JKorf/Kucoin.Net](https://github.com/JKorf/Kucoin.Net)|[![Nuget version](https://img.shields.io/nuget/v/Kucoin.net.svg?style=flat-square)](https://www.nuget.org/packages/Kucoin.Net)|
|LBank|[JKorf/LBank.Net](https://github.com/JKorf/LBank.Net)|[![Nuget version](https://img.shields.io/nuget/v/LBank.net.svg?style=flat-square)](https://www.nuget.org/packages/LBank.Net)|
|Lighter|[JKorf/Lighter.Net](https://github.com/JKorf/Lighter.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Lighter.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Lighter.Net)|
|Mexc|[JKorf/Mexc.Net](https://github.com/JKorf/Mexc.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.Mexc.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.Mexc.Net)|
|OKX|[JKorf/OKX.Net](https://github.com/JKorf/OKX.Net)|[![Nuget version](https://img.shields.io/nuget/v/JK.OKX.net.svg?style=flat-square)](https://www.nuget.org/packages/JK.OKX.Net)|
|Toobit|[JKorf/Toobit.Net](https://github.com/JKorf/Toobit.Net)|[![Nuget version](https://img.shields.io/nuget/v/Toobit.net.svg?style=flat-square)](https://www.nuget.org/packages/Toobit.Net)|
|Upbit|[JKorf/Upbit.Net](https://github.com/JKorf/Upbit.Net)|[![Nuget version](https://img.shields.io/nuget/v/JKorf.Upbit.net.svg?style=flat-square)](https://www.nuget.org/packages/JKorf.Upbit.Net)|
|Weex|[JKorf/Weex.Net](https://github.com/JKorf/Weex.Net)|[![Nuget version](https://img.shields.io/nuget/v/Weex.net.svg?style=flat-square)](https://www.nuget.org/packages/Weex.Net)|
|WhiteBit|[JKorf/WhiteBit.Net](https://github.com/JKorf/WhiteBit.Net)|[![Nuget version](https://img.shields.io/nuget/v/WhiteBit.net.svg?style=flat-square)](https://www.nuget.org/packages/WhiteBit.Net)|
|XT|[JKorf/XT.Net](https://github.com/JKorf/XT.Net)|[![Nuget version](https://img.shields.io/nuget/v/XT.net.svg?style=flat-square)](https://www.nuget.org/packages/XT.Net)|

When using multiple of these API's the [CryptoClients.Net](https://github.com/JKorf/CryptoClients.Net) package can be used which combines this and the other packages and allows easy access to all exchange API's.

## Discord
[![Nuget version](https://img.shields.io/discord/847020490588422145?style=for-the-badge)](https://discord.gg/MSpeEtSY8t)  
A Discord server is available [here](https://discord.gg/MSpeEtSY8t). For discussion and/or questions around the CryptoExchange.Net and implementation libraries, feel free to join.

## Supported functionality

### Spot
|API|Supported|Location|
|--|--:|--|
|REST Common|✓|`restClient.SpotApi.ExchangeData`|
|REST Market|✓|`restClient.SpotApi.ExchangeData`|
|REST Account|✓|`restClient.SpotApi.Account`|
|REST Trades|✓|`restClient.SpotApi.Trading`|
|REST Wallet|✓|`restClient.SpotApi.Account`|
|WebSocket Public Stream|✓|`socketClient.SpotApi`|
|WebSocket Private Stream|✓|`socketClient.SpotApi`|

### Futures
**The futures API is not currently publicly available and therefor not implemented at this moment**

## Support the project
Any support is greatly appreciated.

### Donate
Make a one time donation in a crypto currency of your choice. If you prefer to donate in a different currency or network send me a message.
   
**USDT (TRX)**  TKigKeJPXZYyMVDgMyXxMf17MWYia92Rjd 

### Sponsor
Alternatively, sponsor me on Github using [Github Sponsors](https://github.com/sponsors/JKorf). 

## Release notes
* Version 1.2.0 - 21 Aug 2026
    * Updated to CryptoExchange.Net v12.5.0
    * Added LowerPriceLimitPercentage, UpperPriceLimitPercentage to SharedSpotSymbol mapping
    * Fixed SharedSpotSymbol MinTradeQuantity mapping

* Version 1.1.0 - 29 Jul 2026
    * Updated CryptoExchange.Net to version 12.4.0
    * Added calculation of AveragePrice on Shared order models if data is available and AveragePrice is not set
    * Added DebuggerDisplay attributes to Result models
    * Added AveragePrice property to SharedQuantity model
    * Updated SharedFuturesTicker, SharedSpotTicker, SharedTrade and SharedKline to use SharedOrderQuantity for volumes/quantities

* Version 1.0.0 - 23 Jul 2026
    * Initial release

