namespace PlasterSkull.Blazor;

public record PsIndexedIteratorItem<T>
{
    public required int Index { get; init; }
    public required T Value { get; init; }
}