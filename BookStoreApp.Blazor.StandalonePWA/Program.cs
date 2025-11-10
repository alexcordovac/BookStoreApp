using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BookStoreApp.Blazor.StandalonePWA;
using BookStoreApp.Blazor.StandalonePWA.Providers;
using BookStoreApp.Blazor.StandalonePWA.Services.Authentication;
using BookStoreApp.Blazor.StandalonePWA.Services;
using Microsoft.AspNetCore.Components.Authorization;
using BookStoreApp.Blazor.StandalonePWA.Configuration;
using BookStoreApp.Blazor.StandalonePWA.Services.Base;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7147")
});

/*
 The license key should NOT be set as this would result in secrets transmitted to the client. Instead, omit the license key configuration and mute the license message category
https://docs.automapper.io/en/latest/License-configuration.html

 */
//string luckyStrikeLicenseKey = @"ey....";

//builder.Services.AddAutoMapper(cfg => { cfg.LicenseKey = luckyStrikeLicenseKey; }, typeof(MapperConfig));
builder.Services.AddAutoMapper(cfg => { }, typeof(MapperConfig));
builder.Logging.AddFilter("LuckyPennySoftware.AutoMapper.License", LogLevel.None);

builder.Services.AddScoped<IClient, Client>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();


//builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p => p.GetRequiredService<ApiAuthenticationStateProvider>());
builder.Services.AddAuthorizationCore();
//



//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
