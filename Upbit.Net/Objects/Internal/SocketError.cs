using System.Text.Json.Serialization;

namespace Upbit.Net.Objects.Internal
{
    internal record SocketError
    {
        [JsonPropertyName("error")]
        public SocketErrorDetails Error { get; set; } = default!;
    }

    internal record SocketErrorDetails
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
