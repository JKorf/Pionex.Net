using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Pionex.Net.Interfaces.Clients.SpotApi;
using Pionex.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using Pionex.Net.Clients.MessageHandlers;
using Pionex.Net.Objects.Internal;

namespace Pionex.Net.Clients.SpotApi
{
    /// <inheritdoc cref="IPionexRestClientSpotApi" />
    internal partial class PionexRestClientSpotApi : RestApiClient<PionexEnvironment, PionexAuthenticationProvider, PionexCredentials>, IPionexRestClientSpotApi
    {
        #region fields 
        protected override ErrorMapping ErrorMapping => PionexErrors.Errors;

        /// <inheritdoc />
        protected override IRestMessageHandler MessageHandler { get; } = new PionexRestMessageHandler(PionexErrors.Errors);
        #endregion

        #region Api clients

        /// <inheritdoc />
        public IPionexRestClientSpotApiAccount Account { get; }
        /// <inheritdoc />
        public IPionexRestClientSpotApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IPionexRestClientSpotApiTrading Trading { get; }

        #endregion

        #region constructor/destructor
        internal PionexRestClientSpotApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, PionexRestOptions options)
            : base(loggerFactory, PionexExchange.Metadata.Id, httpClient, options.Environment.RestClientAddress, options, options.SpotOptions)
        {
            Account = new PionexRestClientSpotApiAccount(this);
            ExchangeData = new PionexRestClientSpotApiExchangeData(_logger, this);
            Trading = new PionexRestClientSpotApiTrading(_logger, this);

        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PionexExchange._serializerContext);


        /// <inheritdoc />
        protected override PionexAuthenticationProvider CreateAuthenticationProvider(PionexCredentials credentials)
            => new PionexAuthenticationProvider(credentials);

        internal async Task<HttpResult> SendAsync(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<PionexResult>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail(result);

            if (!result.Data.Success)
                return HttpResult.Fail(result, new ServerError(result.Data.ErrorCode!, GetErrorInfo(result.Data.ErrorCode!, result.Data.ErrorMessage)));

            return result;
        }

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<PionexResult<T>>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<T>(result);

            if (!result.Data.Success)
                return HttpResult.Fail<T>(result, new ServerError(result.Data.ErrorCode!, GetErrorInfo(result.Data.ErrorCode!, result.Data.ErrorMessage)));

            return HttpResult.Ok(result, result.Data.Data!);
        }

        internal async Task<HttpResult<T>> SendRawAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<T>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        /// <inheritdoc />
        protected override Task<HttpResult<DateTime>> GetServerTimestampAsync()
            => ExchangeData.GetServerTimeAsync();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => PionexExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        public IPionexRestClientSpotApiShared SharedClient => this;
    }
}
