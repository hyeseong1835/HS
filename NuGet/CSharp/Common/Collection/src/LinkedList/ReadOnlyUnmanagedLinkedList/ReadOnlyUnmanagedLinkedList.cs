namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public readonly struct ReadonlyUnmanagedLinkedList<TValue, TNode> 
    : IExplicitEnumerableReadOnlyUnmanagedLinkedList<
        TValue, 
        UnmanagedLinkedListValueEnumerator<TValue, TNode>, 
        TNode
    >
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
{
    #region Static

    public static implicit operator ReadonlyUnmanagedLinkedList<TValue, TNode>(UnmanagedLinkedList<TValue, TNode> list)
        => new ReadonlyUnmanagedLinkedList<TValue, TNode>(list);

    #endregion


    #region Instance

    #region Field & Property
    
    readonly UnmanagedLinkedList<TValue, TNode> list;

    public int Count => list.Count;

    #endregion


    #region Constructor

    public ReadonlyUnmanagedLinkedList(UnmanagedLinkedList<TValue, TNode> list)
    {
        this.list = list;
    }

    #endregion
    
    public UnmanagedLinkedListValueEnumerator<TValue, TNode> GetEnumerator()
        => list.GetValueEnumerator();

    #endregion
}