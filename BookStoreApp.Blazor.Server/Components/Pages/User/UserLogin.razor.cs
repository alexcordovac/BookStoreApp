﻿using BookStoreApp.Blazor.Server.Services.Authentication;
using BookStoreApp.Blazor.Server.Services.Base;
using Microsoft.AspNetCore.Components;

namespace BookStoreApp.Blazor.Server.Components.Pages.User
{
    public partial class UserLogin
    {
        [Inject] IAuthenticationService authService { get; set; }
        [Inject] NavigationManager navManager { get; set; }

        //[SupplyParameterFromForm]
        LoginUserDto LoginModel { get; set; } = new LoginUserDto();
        string ErrorMessage;

        public async Task HandleLogin()
        {
            var response = await authService.AuthenticateAsync(LoginModel);

            if (response.Success)
            {
                navManager.NavigateTo("/");
            }

            ErrorMessage = response.Message;
        }
    }
}
