namespace HS.CSharp.Common.Collection.Unmanaged;

/// <summary>
/// 관리되지 않는 메모리에서 싱글 링크드 리스트를 구현합니다. <br/>
/// <br/>
/// 더 이상 사용하지 않을 때 Dispose()를 호출해야합니다.
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TNode"></typeparam>
unsafe public interface IReadOnlyUnmanagedLinkedList<TValue, TNode> : IEnumerable<TValue>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
{
    public int Count { get; }
    public bool IsEmpty => (Count == 0);
}