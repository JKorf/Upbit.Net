using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;

namespace Upbit.Net
{
    internal class UpbitAuthenticationProvider : AuthenticationProvider
    {
        public override ApiCredentialsType[] SupportedCredentialTypes => [];

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
