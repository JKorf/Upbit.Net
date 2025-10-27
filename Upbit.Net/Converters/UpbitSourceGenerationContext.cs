using System;
using System.Text.Json.Serialization;
using Upbit.Net.Objects.Internal;
using Upbit.Net.Objects.Models;

namespace Upbit.Net.Converters
{
    [JsonSerializable(typeof(SocketTicket))]
    [JsonSerializable(typeof(SocketRequest))]
    [JsonSerializable(typeof(SocketError))]
    [JsonSerializable(typeof(UpbitTradeUpdate))]
    [JsonSerializable(typeof(UpbitTickerUpdate))]
    [JsonSerializable(typeof(UpbitOrderBookUpdate))]
    [JsonSerializable(typeof(UpbitKlineUpdate))]

    [JsonSerializable(typeof(UpbitSymbol[]))]
    [JsonSerializable(typeof(UpbitTrade[]))]
    [JsonSerializable(typeof(UpbitTicker[]))]
    [JsonSerializable(typeof(UpbitOrderBook[]))]
    [JsonSerializable(typeof(UpbitKline[]))]
    [JsonSerializable(typeof(UpbitSymbolConfig[]))]

    [JsonSerializable(typeof(object[]))]
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(int?))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(long?))]
    [JsonSerializable(typeof(long))]
    [JsonSerializable(typeof(decimal))]
    [JsonSerializable(typeof(decimal?))]
    [JsonSerializable(typeof(DateTime))]
    [JsonSerializable(typeof(DateTime?))]
    internal partial class UpbitSourceGenerationContext : JsonSerializerContext
    {
    }
}
