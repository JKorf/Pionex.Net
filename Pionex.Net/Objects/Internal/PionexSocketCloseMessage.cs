using System;
using System.Text.Json.Serialization;

namespace Pionex.Net.Objects.Internal
{
    internal record PionexSocketCloseMessage
    {
        [JsonPropertyName("op")]
        public string Operation { get; set; } = string.Empty;
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonPropertyName("note")]
        public string? Note { get; set; }
    }
}
