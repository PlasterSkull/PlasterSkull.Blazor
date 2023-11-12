namespace PlasterSkull.Blazor;

public class PsRedirectTo : ComponentBase
{
    #region Params

    [Parameter] public string Uri { get; set; } = "/";

    #endregion

    #region Injects

    [Inject] NavigationManager _navigationManager { get; init; } = null!;

    #endregion

    protected override void OnInitialized() => _navigationManager.NavigateTo(Uri);
}