using BookStoreApp.Blazor.Server.Services.Authentication;
using BookStoreApp.Blazor.Server.Services.Base;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.Server.Components.Pages.User
{
    public partial class UserLogin
    {
        [Inject] IAuthenticationService authService { get; set; }
        [Inject] NavigationManager navManager { get; set; }

        LoginUserDto LoginModel = new LoginUserDto();
        string message = string.Empty;

        public async Task HandleLogin()
        {
            var response = await authService.AuthenticateAsync(LoginModel);

            if (response.Success)
            {
                navManager.NavigateTo("/");
            }

            message = response.Message;
        }
    }
}
