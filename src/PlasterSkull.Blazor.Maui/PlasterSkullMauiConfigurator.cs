using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Hosting;
using PlasterSkull.Core;

namespace PlasterSkull.Blazor.Maui;

public static class PlasterSkullMauiConfigurator
{
    public static MauiAppBuilder AddPlasterBlazorMauiConfiguration(
        this MauiAppBuilder builder,
        string baseUrl,
        string environment)
    {
        var platform = DeviceInfo.Current.Platform;

        builder.Services.AddSingleton(s => new HostInfo
        {
            AppKind = AppKind.MauiApp,
            ClientKind = true switch
            {
                _ when platform == DevicePlatform.Android => ClientKind.Android,
                _ when platform == DevicePlatform.iOS => ClientKind.iOS,
                _ when platform == DevicePlatform.WinUI => ClientKind.Windows,
                _ when platform == DevicePlatform.macOS => ClientKind.MacCatalyst,
                _ => ClientKind.Unknown,
            },
            BaseUrl = baseUrl,
            Environment = environment,
            DeviceModel = DeviceInfo.Model
        });

        return builder;
    }
}
