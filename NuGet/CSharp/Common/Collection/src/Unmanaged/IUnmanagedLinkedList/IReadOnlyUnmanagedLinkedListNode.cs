namespace HS.CSharp.Common.Collection;

unsafe public interface IReadOnlyUnmanagedLinkedListNode<TValue, TNode> 
    where TValue : unmanaged
    where TNode : unmanaged, IReadOnlyUnmanagedLinkedListNode<TValue, TNode>
{
    /// <summary>
    /// 노드의 값입니다.
    /// </summary>
    TValue Value { get; }
}