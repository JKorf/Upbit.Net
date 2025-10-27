using System.Text.Json.Serialization;

namespace Upbit.Net.Objects.Internal
{
    internal class SocketRequest
    {
        [JsonPropertyName("type")]
        public string Topic { get; set; } = string.Empty;
        [JsonPropertyName("codes")]
        public string[] Codes { get; set; } = [];
        [JsonPropertyName("level"), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Level { get; set; }
    }
}
