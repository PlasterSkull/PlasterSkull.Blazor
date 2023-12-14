using System.Diagnostics;

namespace PlasterSkull.Blazor;

public abstract class PsComponentBase : ComponentBase, IHandleEvent, IDisposable
{
    #region Params

    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object?>? UserAttributes { get; set; }

    #endregion

    #region UI Fields

    protected bool _disposed;
    protected bool IsJSRuntimeAvailable { get; set; }

    // Css/Style fields

    private string _defaultRootClassName = string.Empty;
    private string? _customRootClassName;

    protected string RootClassName =>
        _customRootClassName ?? _defaultRootClassName;

    // OverrideHandleEventBehavior fields

    private bool _overrideHandleEventBehavior;

    // Render info fields

    private bool _isRenderInfoVisible;
    private PsRenderInfoSettings? _renderInfoSettings;
    private int _renderCount;
    private Stopwatch? _renderTimer;
    private int _onParametersSetCallCount;

    protected bool CanShowRenderInfo =>
        _isRenderInfoVisible &&
        PlasterSkullConfigurator.ShowRenderInfoGlobalFlag;

    // rc - render count 
    // psc - OnParametersSet call count
    protected virtual string RenderInfoMessage =>
        !CanShowRenderInfo
            ? string.Empty
            : $"{RootClassName} | rc: {_renderCount} | psc: {_onParametersSetCallCount}";

    #endregion

    #region Css/Style

    protected virtual string ClassName =>
        new CssBuilder(RootClassName)
            .AddClass(() => ExtendClassNameBuilder!.Value, ExtendClassNameBuilder.HasValue)
            .AddClass(() => RenderInfoClassNameBuilder, CanShowRenderInfo)
            .AddClass(Class)
            .Build();

    protected virtual CssBuilder? ExtendClassNameBuilder => null;

    protected CssBuilder RenderInfoClassNameBuilder =>
        new CssBuilder()
            .AddClass("show-render-info");

    protected virtual string StyleName =>
        new StyleBuilder()
            .AddStyle(() => ExtendStyleNameBuilder!.Value, ExtendStyleNameBuilder.HasValue)
            .AddStyle(() => RenderInfoStyleNameBuilder, CanShowRenderInfo)
            .AddStyle(Style)
            .Build();

    protected virtual StyleBuilder? ExtendStyleNameBuilder => null;

    protected StyleBuilder RenderInfoStyleNameBuilder =>
        new StyleBuilder()
            .AddStyle("--render-info-font-size", _renderInfoSettings!.RenderInfoFontSize)
            .AddStyle(
                "--render-info-margin",
                _renderInfoSettings!.RenderInfoMargin,
                !string.IsNullOrEmpty(_renderInfoSettings!.RenderInfoMargin))
            .AddStyle("--render-info-color", _renderInfoSettings!.RenderInfoHexColor)
            .AddStyle("--render-info-left-offset", _renderInfoSettings!.RenderInfoOrigin.AbsoluteLeftOffset())
            .AddStyle("--render-info-top-offset", _renderInfoSettings!.RenderInfoOrigin.AbsoluteTopOffset())
            .AddStyle("--render-info-right-offset", _renderInfoSettings!.RenderInfoOrigin.AbsoluteRightOffset())
            .AddStyle("--render-info-bottom-offset", _renderInfoSettings!.RenderInfoOrigin.AbsoluteBottomOffset());

    #endregion

    #region LC Methods

    public override Task SetParametersAsync(ParameterView parameters)
    {
        _defaultRootClassName = GetType().GetRootClassName();

        return base.SetParametersAsync(parameters);
    }

    protected void ConfigureRootClassName(string? rootClassName)
    {
        _customRootClassName = rootClassName;
    }

    protected void ConfigureOverrideHandleEventBehavior(bool @override = true)
    {
        _overrideHandleEventBehavior = @override;
    }

    protected void ConfigureRenderInfo(bool visible, PsRenderInfoSettings? renderInfoSettings = null)
    {
        _isRenderInfoVisible = visible;
        if (!_isRenderInfoVisible)
            return;

        _renderInfoSettings = PsRenderInfoSettings.CheckValues(renderInfoSettings ?? new());
    }

    protected override Task OnInitializedAsync()
    {
        if (CanShowRenderInfo)
        {
            _renderTimer = new();
            _renderCount++;
            UserAttributes = new()
            {
                ["render-info"] = RenderInfoMessage,
            };
        }

        return base.OnInitializedAsync();
    }

    protected override bool ShouldRender()
    {
        if (CanShowRenderInfo && !_disposed)
        {
            _renderTimer!.Restart();
            _renderCount++;
            UserAttributes!["render-info"] = RenderInfoMessage;
        }

        return base.ShouldRender();
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _onParametersSetCallCount++;
    }

    protected override void OnAfterRender(bool firstRender)
    {
        IsJSRuntimeAvailable = true;
        base.OnAfterRender(firstRender);

        if (!CanShowRenderInfo || _disposed)
            return;

        _renderTimer!.Stop();
        Console.WriteLine($"#{_renderCount} {RootClassName} took {_renderTimer.Elapsed}");
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        Dispose(disposing: true);
        GC.SuppressFinalize(this);
        _disposed = true;
    }

    protected virtual void Dispose(bool disposing)
    {
        _renderTimer?.Stop();
        _renderTimer = null;
    }

    #endregion

    #region HandleEvent Behaviors

    protected Task InvokeStateHasChangedInUIThread() =>
        InvokeAsync(StateHasChanged);

    private async Task CustomCallStateHasChangedOnAsyncCompletion(Task task)
    {
        try
        {
            await task;
        }
        catch // avoiding exception filters for AOT runtime support
        {
            // Ignore exceptions from task cancellations, but don't bother issuing a state change.
            if (task.IsCanceled)
            {
                return;
            }

            throw;
        }

        StateHasChanged();
    }

    Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem callback, object? arg)
    {
        var task = callback.InvokeAsync(arg);
        if (_overrideHandleEventBehavior)
            return task;

        var shouldAwaitTask =
            task.Status != TaskStatus.RanToCompletion &&
            task.Status != TaskStatus.Canceled;

        StateHasChanged();

        return shouldAwaitTask ?
            CustomCallStateHasChangedOnAsyncCompletion(task) :
            Task.CompletedTask;
    }

    #endregion
}
