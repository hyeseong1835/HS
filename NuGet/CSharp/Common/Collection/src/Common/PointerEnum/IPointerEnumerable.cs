namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IPointerEnumerable<T>
    where T : unmanaged
{
    IPointerEnumerator<T> GetPointerEnumerator();
}