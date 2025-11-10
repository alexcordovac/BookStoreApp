using BookStoreApp.Blazor.StandalonePWA.Services.Base;

namespace BookStoreApp.Blazor.StandalonePWA.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<Response<AuthResponse>> AuthenticateAsync(LoginUserDto loginModel);
        public Task Logout();
    }
}
