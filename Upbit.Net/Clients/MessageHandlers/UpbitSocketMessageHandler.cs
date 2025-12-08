using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using System.Text.Json;
using Upbit.Net.Objects.Models;

namespace Upbit.Net.Clients.MessageHandlers
{
    internal class UpbitSocketMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = UpbitExchange._serializerContext;

        public UpbitSocketMessageHandler()
        {
            AddTopicMapping<UpbitKlineUpdate>(x => x.Symbol);
            AddTopicMapping<UpbitOrderBookUpdate>(x => x.Symbol);
            AddTopicMapping<UpbitTickerUpdate>(x => x.Symbol);
            AddTopicMapping<UpbitTradeUpdate>(x => x.Symbol);
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [ 
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("type"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("type")!
            },

             new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("status"),
                ],
                StaticIdentifier = "status"
            },

             new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("name") { Depth = 2 },
                ],
                StaticIdentifier = "error"
            },
        ];
    }
}
