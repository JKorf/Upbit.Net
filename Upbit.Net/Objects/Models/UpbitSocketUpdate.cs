using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Upbit.Net.Enums;

namespace Upbit.Net.Objects.Models
{
    /// <summary>
    /// Socket update
    /// </summary>
    public record UpbitSocketUpdate
    {
        /// <summary>
        /// Event type
        /// </summary>
        [JsonPropertyName("type")]
        public string EventType { get; set; } = string.Empty;
        /// <summary>
        /// Event type
        /// </summary>
        [JsonPropertyName("stream_type")]
        public StreamType StreamType { get; set; }
    }
}
