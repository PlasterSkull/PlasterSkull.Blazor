using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PlasterSkull.Blazor.Wasm;

public static class PlasterSkullWasmConfigurator
{
    public static WebAssemblyHostBuilder AddPlasterBlazorWasmConfiguration(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddSingleton(s => new HostInfo
        {
            AppKind = AppKind.WasmApp,
            ClientKind = ClientKind.Wasm,
            Environment = builder.HostEnvironment.Environment,
            BaseUrl = builder.HostEnvironment.BaseAddress,
        });

        return builder;
    }
}
