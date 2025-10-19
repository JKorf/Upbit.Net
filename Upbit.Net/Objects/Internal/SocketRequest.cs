using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Upbit.Net.Objects.Internal
{
    internal class SocketRequest
    {
        [JsonPropertyName("type")]
        public string Topic { get; set; } = string.Empty;
        [JsonPropertyName("codes")]
        public string[] Codes { get; set; } = [];
    }
}
