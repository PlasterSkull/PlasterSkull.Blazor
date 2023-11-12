namespace PlasterSkull.Blazor;

public static class PlasterSkullConfigurator
{
    internal static bool ShowRenderInfoGlobalFlag = false;

    public static void SetShowRenderInfoState(bool visible) =>
        ShowRenderInfoGlobalFlag = visible;

    public static ClientAppBuilder AddPlasterBlazor(this ClientAppBuilder appBuilder)
    {
        return appBuilder;
    }
}
