using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Linq;
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

        protected override MessageEvaluator[] TypeEvaluators { get; } = [ 
            new MessageEvaluator {
                Priority = 1,
                Fields = [
                    new PropertyFieldReference("type"),
                ],
                IdentifyMessageCallback = x => x.FieldValue("type")!
            },

             new MessageEvaluator {
                Priority = 2,
                Fields = [
                    new PropertyFieldReference("status"),
                ],
                StaticIdentifier = "status"
            },

             new MessageEvaluator {
                Priority = 3,
                Fields = [
                    new PropertyFieldReference("name") { Depth = 2 },
                ],
                StaticIdentifier = "error"
            },
        ];
    }
}
