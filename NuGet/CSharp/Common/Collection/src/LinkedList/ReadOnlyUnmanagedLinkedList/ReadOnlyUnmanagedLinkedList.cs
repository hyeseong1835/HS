namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public readonly struct ReadonlyUnmanagedLinkedList<TValue> 
    : IExplicitEnumerableReadOnlyUnmanagedLinkedList<
        TValue, 
        UnmanagedLinkedListValueEnumerator<TValue>, 
        UnmanagedLinkedListNode<TValue>
    >
    where TValue : unmanaged
{
    #region Static

    public static implicit operator ReadonlyUnmanagedLinkedList<TValue>(UnmanagedLinkedList<TValue> list)
        => new ReadonlyUnmanagedLinkedList<TValue>(list);

    #endregion


    #region Instance

    #region Field & Property
    
    readonly UnmanagedLinkedList<TValue> list;

    public int Count => list.Count;

    #endregion


    #region Constructor

    public ReadonlyUnmanagedLinkedList(UnmanagedLinkedList<TValue> list)
    {
        this.list = list;
    }

    #endregion
    
    UnmanagedLinkedListValueEnumerator<TValue> IExplicitEnumerable<TValue, UnmanagedLinkedListValueEnumerator<TValue>>.GetEnumerator()
        => list.GetValueEnumerator();
    public UnmanagedLinkedListValueEnumerator<TValue> GetValueEnumerator()
        => list.GetValueEnumerator();

    #endregion
}

unsafe public readonly struct ReadonlyUnmanagedLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator> 
    : IExplicitEnumerableReadOnlyUnmanagedLinkedList<
        TValue, 
        TValueEnumerator, 
        TNode
    >
    where TValue : unmanaged
    where TValueEnumerator : IEnumerator<TValue>
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
    where TNodeEnumerator : IEnumerator<TNode>
{
    #region Static

    public static implicit operator ReadonlyUnmanagedLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator>(UnmanagedLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator> list)
        => new ReadonlyUnmanagedLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator>(list);

    #endregion


    #region Instance

    #region Field & Property
    
    readonly UnmanagedLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator> list;

    public int Count => list.Count;

    #endregion


    #region Constructor

    public ReadonlyUnmanagedLinkedList(UnmanagedLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator> list)
    {
        this.list = list;
    }

    #endregion
    
    UnmanagedLinkedListValueEnumerator<TValue> IExplicitEnumerable<TValue, UnmanagedLinkedListValueEnumerator<TValue>>.GetEnumerator()
        => list.GetValueEnumerator();
    public UnmanagedLinkedListValueEnumerator<TValue> GetValueEnumerator()
        => list.GetValueEnumerator();

    #endregion
}