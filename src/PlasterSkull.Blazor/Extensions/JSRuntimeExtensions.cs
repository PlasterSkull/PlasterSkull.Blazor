using Microsoft.JSInterop;

namespace PlasterSkull.Blazor
{
    public static class JSRuntimeExtensions
    {
        public static async Task CopyToClipboard(this IJSRuntime jsRuntime, string value)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("eval", $"window.navigator.clipboard.writeText('{value}')");
            }
            catch (Exception)
            {
                //Потеря фокуса (Альт-таб) во время выполнения приводит к ошибке
            }
        }
    }
}
