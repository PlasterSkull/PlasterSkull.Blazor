using Microsoft.Extensions.DependencyInjection;

namespace PlasterSkull.Blazor;

public static class PlasterSkullConfigurator
{
    internal static bool ShowRenderInfoGlobalFlag = false;

    public static void SetShowRenderInfoState(bool visible) =>
        ShowRenderInfoGlobalFlag = visible;

    public static IServiceCollection AddPlasterBlazor(this IServiceCollection services)
    {
        return services;
    }
}
