namespace HS.CSharp.Common.Collection.Unmanaged;

public interface IReadOnlyUnmanagedValueLinkedListNode<TValue>
    where TValue : unmanaged
{
    /// <summary>
    /// 노드의 값입니다.
    /// </summary>
    TValue Value { get; }
}