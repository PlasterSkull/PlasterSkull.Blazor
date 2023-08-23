

namespace PlasterSkull.Blazor;

public partial class PsIndexedIterator<T> : ComponentBase
{
    #region Params

    [Parameter] public IEnumerable<T>? Items { get; set; }
    [Parameter] public RenderFragment<PsIndexedIteratorItem<T>>? ChildContent { get; set; }

    #endregion

    #region UI Fields

    private List<PsIndexedIteratorItem<T>> _items = new();

    private bool _canIterateItems
        => _items?.Count > 0
        && ChildContent is not null;

    #endregion

    #region LC Methods

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (Items == null)
            return;

        var listOfItems = Items.ToList();

        _items = Enumerable.Range(0, listOfItems.Count)
            .Select(index => new PsIndexedIteratorItem<T>
            {
                Value = listOfItems[index],
                Index = index,
            })
            .ToList();
    }

    #endregion
}
