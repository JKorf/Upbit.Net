using System;
using System.Collections.Generic;
using System.Net.Http;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;

namespace Upbit.Net
{
    internal class UpbitAuthenticationProvider : AuthenticationProvider
    {
        public UpbitAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
        }

        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration requestConfig)
        {
            if (!requestConfig.Authenticated)
                return;

            // Auth
        }
    }
}
