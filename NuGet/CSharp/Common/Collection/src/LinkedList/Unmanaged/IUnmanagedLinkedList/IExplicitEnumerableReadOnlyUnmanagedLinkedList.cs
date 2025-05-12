namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IExplicitEnumerableReadOnlyUnmanagedLinkedList<TValue, TValueEnumerator, TNode> 
    : IReadOnlyUnmanagedLinkedList<TValue, TNode>, 
      IExplicitEnumerable<TValue, TValueEnumerator>
    where TValue : unmanaged
    where TValueEnumerator : IEnumerator<TValue>
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
{
    
}