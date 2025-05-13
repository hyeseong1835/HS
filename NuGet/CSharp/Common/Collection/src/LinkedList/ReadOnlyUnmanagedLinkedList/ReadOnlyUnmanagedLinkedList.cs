namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public readonly struct ReadonlyUnmanagedLinkedList<TValue> 
    : IExplicitEnumerableReadOnlyUnmanagedLinkedList<
        TValue, 
        UnmanagedValueLinkedListValueEnumerator<TValue>, 
        UnmanagedValueLinkedListNode<TValue>
    >
    where TValue : unmanaged
{
    #region Static

    public static implicit operator ReadonlyUnmanagedLinkedList<TValue>(UnmanagedValueLinkedList<TValue> list)
        => new ReadonlyUnmanagedLinkedList<TValue>(list);

    #endregion


    #region Instance

    #region Field & Property
    
    readonly UnmanagedValueLinkedList<TValue> list;

    public int Count => list.Count;

    #endregion


    #region Constructor

    public ReadonlyUnmanagedLinkedList(UnmanagedValueLinkedList<TValue> list)
    {
        this.list = list;
    }

    #endregion
    
    UnmanagedValueLinkedListValueEnumerator<TValue> IExplicitEnumerable<TValue, UnmanagedValueLinkedListValueEnumerator<TValue>>.GetEnumerator()
        => list.GetValueEnumerator();
    public UnmanagedValueLinkedListValueEnumerator<TValue> GetValueEnumerator()
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

    public static implicit operator ReadonlyUnmanagedLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator>(UnmanagedValueLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator> list)
        => new ReadonlyUnmanagedLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator>(list);

    #endregion


    #region Instance

    #region Field & Property
    
    readonly UnmanagedValueLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator> list;

    public int Count => list.Count;

    #endregion


    #region Constructor

    public ReadonlyUnmanagedLinkedList(UnmanagedValueLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator> list)
    {
        this.list = list;
    }

    #endregion
    
    UnmanagedValueLinkedListValueEnumerator<TValue> IExplicitEnumerable<TValue, UnmanagedValueLinkedListValueEnumerator<TValue>>.GetEnumerator()
        => list.GetValueEnumerator();
    public UnmanagedValueLinkedListValueEnumerator<TValue> GetValueEnumerator()
        => list.GetValueEnumerator();

    #endregion
}