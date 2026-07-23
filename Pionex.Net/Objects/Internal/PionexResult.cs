using System;
using System.Text.Json.Serialization;

namespace Pionex.Net.Objects.Internal
{
    internal record PionexResult
    {
        [JsonPropertyName("result")]
        public bool Success { get; set; }
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        [JsonPropertyName("code")]
        public string? ErrorCode { get; set; }
        [JsonPropertyName("message")]
        public string? ErrorMessage { get; set; }
    }

    internal record PionexResult<T> : PionexResult
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
