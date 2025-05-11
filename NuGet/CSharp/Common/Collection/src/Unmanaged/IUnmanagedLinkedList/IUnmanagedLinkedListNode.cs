namespace HS.CSharp.Common.Collection;

unsafe public interface IUnmanagedLinkedListNode<TValue, TNode> 
    : IReadOnlyUnmanagedLinkedListNode<TValue, TNode>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
{
    IntPtr ValuePtrOffset { get; }

    new TValue Value { get; set;}
    TValue IReadOnlyUnmanagedLinkedListNode<TValue, TNode>.Value => Value;

    /// <summary>
    /// 다음 노드의 포인터입니다.
    /// </summary>
    TNode* NextNodePtr { get; set; }
}