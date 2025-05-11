namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IPointerEnumerator<T>
    where T : unmanaged
{
    /// <summary>
    /// 현재 열거자가 가리키는 값의 포인터입니다.
    /// </summary>
    T* CurrentPtr { get; }
}