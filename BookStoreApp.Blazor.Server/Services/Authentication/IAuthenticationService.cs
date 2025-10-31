using BookStoreApp.Blazor.Server.Services.Base;

namespace BookStoreApp.Blazor.Server.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<Response<AuthResponse>> AuthenticateAsync(LoginUserDto loginModel);
        public Task Logout();
    }
}
