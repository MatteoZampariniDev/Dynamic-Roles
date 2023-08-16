using DynamicRoles.ClientDemo;
using DynamicRoles.ClientDemo.Common.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using MudBlazor.Services;
using DynamicRoles.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("DynamicRoles.ClientDemo",
    client =>
    {
        client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
    })
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("DynamicRoles.ClientDemo"));

builder.Services.AddSingleton(services => (IJSInProcessRuntime)services.GetRequiredService<IJSRuntime>());

builder.Services.AddDefaultIdentityWithDynamicRoles();

builder.Services.AddMudServices();

// NOTE: https://github.com/khellang/Scrutor/issues/180
builder.Services.Scan(scan => scan
    .FromAssemblyOf<IRolesClient>()
    .AddClasses()
    .AsImplementedInterfaces()
    .WithScopedLifetime());

await builder.Build().RunAsync();
