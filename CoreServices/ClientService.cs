using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CoreServices
{
    public class ClientService : IClientService
    {
        private readonly IPublicClientApplication _client;

        public object Properties { get; }


        public ClientService()
        {

            var appId = OAuth.Appid;
            var authority = "https://login.microsoftonline.com/consumers";
            _client = PublicClientApplicationBuilder.Create(appId).WithAuthority(authority).Build();

        }

        private Task<AuthenticationResult> GetAuthenticationAsync(string loginHint)
        {
            var scopes = OAuth.Scope.Split(' ');
            return _client.AcquireTokenInteractive(scopes).WithParentActivityOrWindow(UiParentProvider.UiParent).WithLoginHint(loginHint).ExecuteAsync();
        }

        public async Task<GraphServiceClient> GraphServiceClient(string loginHint)
        {
            var auth = await GetAuthenticationAsync(loginHint);
            if (auth != null)
            {
                var client = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", auth.AccessToken);
                    return Task.FromResult(0);
                }));
                return client;
            }
            else throw new Exception("Authentication failed");
        }

    }
}
