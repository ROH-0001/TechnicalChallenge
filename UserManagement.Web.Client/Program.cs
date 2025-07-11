using Blazored.Toast;
using Blazored.Toast.Configuration;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UserManagement.Web.Client;
using UserManagement.Web.Client.Services.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7010/") });
builder.Services.AddClientServices();
builder.Services.AddBlazoredToast();

await builder.Build().RunAsync();
