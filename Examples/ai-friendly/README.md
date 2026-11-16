# Pionex.Net examples

Each file is an independent console program. Install `Pionex.Net` in a console project and copy one program to `Program.cs`.

| Program | Workflow |
|---|---|
| [01-spot-quickstart.cs](01-spot-quickstart.cs) | Spot market data, balances and symbol rules |
| [02-spot-trading.cs](02-spot-trading.cs) | Live order, lookup and cancellation |
| [03-websocket.cs](03-websocket.cs) | Public/private streams and cleanup |
| [04-multi-exchange.cs](04-multi-exchange.cs) | Shared API V2 ticker, catalog and trade subscription |
| [05-error-handling.cs](05-error-handling.cs) | Results and bounded public-read retry |

Replace key/secret placeholders for intended private operations. Trading calls change live account state and cancellation can race with fills. Read [operational considerations](../../.claude/skills/pionex-net/references/safety.md) first. See [usage snippets](../../.claude/skills/pionex-net/references/usage.md) for focused patterns.

`AiExampleCompileTests` compiles these programs without executing exchange operations.
