using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Upbit.Net.Interfaces.Clients.SpotApi;
using Upbit.Net.Objects.Models;

namespace Upbit.Net.Clients.SpotApi
{
    internal partial class UpbitRestClientSpotSharedApi
    {
        #region Get Spot Symbols

        async Task<ICallResult<SharedSpotSymbol[]>> IGetSpotSymbols.GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
            => await GetSpotSymbolsAsync(request, ct).ConfigureAwait(false);

        public SharedSymbolCatalog? SpotSymbolCatalog => ExchangeSymbolCache.GetSymbolCatalog(_exchange, _topicId, _api.EnvironmentName, null);
        public GetSpotSymbolsOptions GetSpotSymbolsOptions { get; }
            = new GetSpotSymbolsOptions(_exchange, false);

        public async Task<HttpResult<SharedSpotSymbol[]>> GetSpotSymbolsAsync(GetSymbolsRequest request, CancellationToken ct)
        {
            var validationError = GetSpotSymbolsOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedSpotSymbol[]>(Exchange, validationError);

            var result = await _api.ExchangeData.GetSymbolsAsync(true, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedSpotSymbol[]>(result);

            // Need to request in multiple request or the server returns an error for too long URI
            var batchSize = 500;
            var batches = Math.Ceiling(result.Data.Length / (decimal)batchSize);
            var resultConfigs = new List<UpbitSymbolConfig>();
            for(var batch = 0; batch < batches; batch++)
            {
                var batchSymbols = result.Data.Skip(batch * batchSize).Take(batchSize).Select(x => x.Symbol).ToArray();
                var batchResultConfig = await _api.ExchangeData.GetSymbolConfigAsync(string.Join(",", batchSymbols), ct: ct).ConfigureAwait(false);
                if (!batchResultConfig.Success)
                    return HttpResult.Fail<SharedSpotSymbol[]>(batchResultConfig);

                resultConfigs.AddRange(batchResultConfig.Data);
            }

            var resultData =
                 result.Data.Select(x => ParseSymbol(x, resultConfigs))
                .ToArray();

            ExchangeSymbolCache.UpdateSymbolInfo(_topicId, _api.EnvironmentName, null, resultData);
            return HttpResult.Ok(result, SharedUtils.ApplySymbolFilter(resultData, request));
        }

        #endregion

        private SharedSpotSymbol ParseSymbol(UpbitSymbol s, List<UpbitSymbolConfig> resultConfigs)
        {
            var split = s.Symbol.Split('-');
            var config = resultConfigs.SingleOrDefault(x => x.Symbol == s.Symbol);
            var result = new SharedSpotSymbol(split[1], split[0], s.Symbol, true)
            {
                DisplayName = s.Name,
                PriceStep = config?.TickQuantity,
            };

            if (_fiatCurrencies.Contains(result.QuoteAsset))
            {
                result.QuoteAssetType = SharedAssetType.Fiat;
            }
            else if(LibraryHelpers.IsStableCoin(result.QuoteAsset))
            {
                result.QuoteAssetType = SharedAssetType.Crypto;
                result.QuoteAssetSubType = SharedAssetSubType.StableCoin;
            }
            else
            {
                result.QuoteAssetType = SharedAssetType.Crypto;
            }

            if (_fiatCurrencies.Contains(result.BaseAsset))
            {
                result.BaseAssetType = SharedAssetType.Fiat;
            }
            else if (LibraryHelpers.IsStableCoin(result.BaseAsset))
            {
                result.BaseAssetType = SharedAssetType.Crypto;
                result.BaseAssetSubType = SharedAssetSubType.StableCoin;
            }
            else
            {
                result.BaseAssetType = SharedAssetType.Crypto;
            }

            return result;
        }

        public async Task<ExchangeCallResult<SharedSymbol[]>> GetSpotSymbolsForBaseAssetAsync(string baseAsset)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<SharedSymbol[]>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<SharedSymbol[]>.Ok(Exchange, ExchangeSymbolCache.GetSymbolsForBaseAsset(_topicId, _api.EnvironmentName, null, baseAsset));
        }

        public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(SharedSymbol symbol)
        {
            if (symbol.TradingMode != TradingMode.Spot)
                throw new ArgumentException(nameof(symbol), "Only Spot symbols allowed");

            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbol));
        }

        public async Task<ExchangeCallResult<bool>> SupportsSpotSymbolAsync(string symbolName)
        {
            if (!ExchangeSymbolCache.HasCached(_topicId, _api.EnvironmentName, null))
            {
                var symbols = await GetSpotSymbolsAsync(new GetSymbolsRequest(), default).ConfigureAwait(false);
                if (!symbols.Success)
                    return ExchangeCallResult<bool>.Fail(Exchange, symbols.Error!);
            }

            return ExchangeCallResult<bool>.Ok(Exchange, ExchangeSymbolCache.SupportsSymbol(_topicId, _api.EnvironmentName, null, symbolName));
        }
    }
}
