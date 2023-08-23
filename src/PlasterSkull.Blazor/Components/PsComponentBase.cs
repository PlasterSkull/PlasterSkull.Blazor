using MudBlazor.Utilities;

namespace PlasterSkull.Blazor
{
    public abstract class PsComponentBase : MudComponentBase
    {
        #region Css/Style

        protected virtual string RootClassName => string.Empty;

        protected virtual string ClassName
            => new CssBuilder(RootClassName)
                .AddClass(Class)
                .Build();

        protected virtual string StyleName
            => new StyleBuilder()
                .AddStyle(Style)
                .Build();

        #endregion
    }
}
