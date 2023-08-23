namespace PlasterSkull.Blazor;

public partial class PsCondition : ComponentBase
{
    #region Params

    [Parameter] public bool Evaluation { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? Match { get; set; }
    [Parameter] public RenderFragment? NotMatch { get; set; }

    #endregion
}
