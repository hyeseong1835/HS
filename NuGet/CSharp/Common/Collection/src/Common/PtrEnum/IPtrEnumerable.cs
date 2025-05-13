namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IPtrEnumerable<T>
    where T : unmanaged
{
    IPtrEnumerator<T> GetPtrEnumerator();
}