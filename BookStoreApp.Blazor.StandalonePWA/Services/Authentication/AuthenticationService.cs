using Blazored.LocalStorage;
using BookStoreApp.Blazor.StandalonePWA.Providers;
using BookStoreApp.Blazor.StandalonePWA.Services.Base;
using BookStoreApp.Blazor.StandalonePWA.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.StandalonePWA.Services.Authentication
{
    public class AuthenticationService : BaseHttpService, IAuthenticationService
    {
        private readonly IClient httpClient;
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public AuthenticationService(IClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider) 
            : base(httpClient, localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<Response<AuthResponse>> AuthenticateAsync(LoginUserDto loginModel)
        {
            Response<AuthResponse> response;
            try
            {
                var result = await httpClient.LoginAsync(loginModel);
                response = new Response<AuthResponse>
                {
                    Data = result,
                    Success = true,
                };
                //Store Token
                await localStorage.SetItemAsync("accessToken", result.Token);

                //Change auth state of app
                await ((ApiAuthenticationStateProvider)authenticationStateProvider).LoggedIn();
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<AuthResponse>(exception);
            }

            return response;
        }

        public async Task Logout()
        {
            await ((ApiAuthenticationStateProvider)authenticationStateProvider).LoggedOut();
        }
    }

}
