namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IExplicitEnumerableUnmanagedLinkedList<TValue, TValueEnumerator, TNode> 
    : IUnmanagedLinkedList<TValue, TNode>, 
      IExplicitEnumerable<TValue, TValueEnumerator>
    where TValue : unmanaged
    where TValueEnumerator : IEnumerator<TValue>
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
{
    
}