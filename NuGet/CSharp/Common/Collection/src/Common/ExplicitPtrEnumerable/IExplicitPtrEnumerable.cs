namespace HS.CSharp.Common.Collection.Unmanaged;

public interface IExplicitPtrEnumerable<T, TEnumerator> : IPtrEnumerable<T>
    where T : unmanaged
    where TEnumerator : IPtrEnumerator<T>
{
    new TEnumerator GetPtrEnumerator();
    IPtrEnumerator<T> IPtrEnumerable<T>.GetPtrEnumerator()
        => GetPtrEnumerator();

}