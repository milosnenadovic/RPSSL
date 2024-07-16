using Blazored.SessionStorage;
using MatBlazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RPSSL.Web;
using RPSSL.Web.Auth;
using RPSSL.Web.Configurations;
using RPSSL.Web.Contracts.User;
using RPSSL.Web.Handlers;
using RPSSL.Web.Helpers;
using RPSSL.Web.Services;
using RPSSL.Web.Services.Abstractions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

#region AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
#endregion

builder.Services.AddTransient<JwtHandler>();

builder.Services.AddHttpClient("API", options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetSection("AppSettings:ApiSettings:BaseUrl").Value!);
})
.AddHttpMessageHandler<JwtHandler>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IChoiceService, ChoiceService>();

builder.Services.AddScoped<LoginUserRequest>();

builder.Services.AddOptions();

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddMatBlazor();
builder.Services.AddMatToaster(config =>
{
    config.Position = MatToastPosition.TopCenter;
    config.PreventDuplicates = true;
    config.NewestOnTop = true;
    config.ShowCloseButton = true;
    config.MaximumOpacity = 90;
    config.VisibleStateDuration = 10000;
});

#region Authorization
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
#endregion

#region Localization
builder.Services.AddSingleton<LocalizationManager>();
#endregion

await builder.Build().RunAsync();
