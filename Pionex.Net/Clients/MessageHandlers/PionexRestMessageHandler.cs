using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pionex.Net.Clients.MessageHandlers
{
    internal class PionexRestMessageHandler : JsonRestMessageHandler
    {
        private readonly ErrorMapping _errorMapping;

        public override bool RequiresSeekableStream => true;

        public override JsonSerializerOptions Options { get; } = PionexExchange._serializerContext;

        public PionexRestMessageHandler(ErrorMapping errorMapping)
        {
            _errorMapping = errorMapping;
        }

        public override async ValueTask<Error?> CheckForErrorResponse(RequestDefinition request, HttpResponseHeaders responseHeaders, Stream responseStream)
        {
            var (parseError, document) = await GetJsonDocument(responseStream).ConfigureAwait(false);
            if (parseError != null)
                return parseError;

            if (document!.RootElement.ValueKind is JsonValueKind.Array)
                return null;

            var result = document!.RootElement.TryGetProperty("result", out var resultProp) ? resultProp.GetBoolean() : (bool?)null;
            if (result == true)
                return null;

            var code = document!.RootElement.TryGetProperty("code", out var codeProp) && codeProp.ValueKind == JsonValueKind.String ? codeProp.GetString() : null;
            var error = document!.RootElement.TryGetProperty("message", out var errorProp) ? errorProp.GetString() : null;
            if (code?.Length > 0)
            {
                var errorInfo = _errorMapping.GetErrorInfo(code, error);
                return new ServerError(code, errorInfo);
            }

            return await base.CheckForErrorResponse(request, responseHeaders, responseStream).ConfigureAwait(false);
        }

        public override ValueTask<Error> ParseErrorResponse(int httpStatusCode, HttpResponseHeaders responseHeaders, Stream responseStream)
            => new ValueTask<Error>(new ServerError(ErrorInfo.Unknown));
    }
}
