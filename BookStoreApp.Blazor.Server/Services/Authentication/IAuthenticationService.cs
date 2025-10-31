using BookStoreApp.Blazor.Server.Services.Base;
using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<Response<AuthResponse>> AuthenticateAsync(LoginUserDto loginModel);
        public Task Logout();
    }
}
