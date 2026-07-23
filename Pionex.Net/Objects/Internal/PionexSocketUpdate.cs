using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pionex.Net.Objects.Internal
{
    internal record PionexSocketUpdate
    {
        [JsonPropertyName("topic")]
        public string Topic { get; set; } = string.Empty;
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    internal record PionexSocketUpdate<T> : PionexSocketUpdate
    {
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}
