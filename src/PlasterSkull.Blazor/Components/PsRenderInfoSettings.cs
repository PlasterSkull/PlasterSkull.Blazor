﻿using MudBlazor.Utilities;

namespace PlasterSkull.Blazor;

public record PsRenderInfoSettings
{
    public string? RenderInfoHexColor { get; set; } = MudExtensions.GenerateRandomMudColor().ToString(MudColorOutputFormats.HexA);
    public int ZIndex { get; set; } = 10;
    public Origin RenderInfoOrigin { get; set; } = Origin.TopRight;
    public string? RenderInfoFontSize { get; set; }
    public string? RenderInfoMargin { get; set; }


    public static PsRenderInfoSettings CheckValues(PsRenderInfoSettings renderInfoSettings)
    {
        renderInfoSettings.RenderInfoHexColor = !string.IsNullOrEmpty(renderInfoSettings.RenderInfoHexColor)
            ? renderInfoSettings.RenderInfoHexColor
            : MudExtensions.GenerateRandomMudColor().ToString(MudColorOutputFormats.HexA);
        renderInfoSettings.ZIndex = renderInfoSettings.ZIndex;
        renderInfoSettings.RenderInfoOrigin = renderInfoSettings.RenderInfoOrigin;
        renderInfoSettings.RenderInfoFontSize = !string.IsNullOrEmpty(renderInfoSettings.RenderInfoFontSize)
            ? renderInfoSettings.RenderInfoFontSize
            : "9px";
        renderInfoSettings.RenderInfoMargin = renderInfoSettings.RenderInfoMargin;
        return renderInfoSettings;
    }
}
