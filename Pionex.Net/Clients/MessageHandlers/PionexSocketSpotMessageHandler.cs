using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using Pionex.Net.Objects.Internal;
using Pionex.Net.Objects.Models;
using System.Text.Json;

namespace Pionex.Net.Clients.MessageHandlers
{
    internal class PionexSocketSpotMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = PionexExchange._serializerContext;

        public PionexSocketSpotMessageHandler()
        {
            AddTopicMapping<PionexSocketUpdate<PionexTrade[]>>(x => x.Symbol);
            AddTopicMapping<PionexSocketUpdate<PionexOrderBook>>(x => x.Symbol);
            AddTopicMapping<PionexSocketUpdate<PionexOrder>>(x => x.Symbol);
            AddTopicMapping<PionexSocketUpdate<PionexUserTrade>>(x => x.Symbol);
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("type"),
                    new PropertyFieldReference("topic"),
                    new PropertyFieldReference("symbol"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("type")! + x.FieldValue("topic")! + x.FieldValue("symbol")!,
            },
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("type"),
                    new PropertyFieldReference("topic"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("type")! + x.FieldValue("topic")!,
            },
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("topic")
                ],
                TypeIdentifierCallback = x => x.FieldValue("topic")!,
            },
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("op").WithEqualConstraint("PING"),
                ],
                StaticIdentifier = "PING"
            }
        ];
    }
}
