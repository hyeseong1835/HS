using System.Collections;

namespace HS.CSharp.Common.Collection;

public interface IExplicitPtrEnumerable<TValue, TEnumerator> : IEnumerable<nint>
    where TEnumerator : IEnumerator<nint>
{
    new TEnumerator GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    IEnumerator<nint> IEnumerable<nint>.GetEnumerator()
        => GetEnumerator();
}