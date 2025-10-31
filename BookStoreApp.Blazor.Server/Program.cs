using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.Components;
using BookStoreApp.Blazor.Server.Providers;
using BookStoreApp.Blazor.Server.Services.Authentication;
using BookStoreApp.Blazor.Server.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();


builder.Services.AddHttpClient<IClient, Client >(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrl"]);

});

builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p =>p.GetRequiredService<ApiAuthenticationStateProvider>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
