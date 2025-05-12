using System.Collections;

namespace HS.CSharp.Common.Collection;

public interface IExplicitEnumerable<TValue, TEnumerator> : IEnumerable<TValue>
    where TEnumerator : IEnumerator<TValue>
{
    new TEnumerator GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
    IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        => GetEnumerator();
}