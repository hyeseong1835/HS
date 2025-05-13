namespace HS.CSharp.Common.Collection.Unmanaged;

public struct UnmanagedPtrLinkedListValueEnumerator<TValue> 
    : IPtrEnumerator<TValue>
    where TValue : unmanaged
{
    #region Static

    // this => UnmanagedPtrLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator>
    public static implicit operator UnmanagedPtrLinkedListValueEnumerator<TValue, UnmanagedPtrLinkedListNode<TValue>, UnmanagedLinkedListNodeEnumerator<UnmanagedPtrLinkedListNode<TValue>>>(UnmanagedPtrLinkedListValueEnumerator<TValue> valueEnumerator)
        => new UnmanagedPtrLinkedListValueEnumerator<TValue, UnmanagedPtrLinkedListNode<TValue>, UnmanagedLinkedListNodeEnumerator<UnmanagedPtrLinkedListNode<TValue>>>(valueEnumerator.nodeEnumerator);

    // UnmanagedPtrLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator> => this
    public static implicit operator UnmanagedPtrLinkedListValueEnumerator<TValue>(UnmanagedPtrLinkedListValueEnumerator<TValue, UnmanagedPtrLinkedListNode<TValue>, UnmanagedLinkedListNodeEnumerator<UnmanagedPtrLinkedListNode<TValue>>> valueEnumerator)
        => new UnmanagedPtrLinkedListValueEnumerator<TValue>(valueEnumerator.nodeEnumerator);


    // this => UnmanagedLinkedListNodeEnumerator<UnmanagedPtrLinkedListNode<TValue>>
    public static implicit operator UnmanagedLinkedListNodeEnumerator<UnmanagedPtrLinkedListNode<TValue>>(UnmanagedPtrLinkedListValueEnumerator<TValue> valueEnumerator)
        => valueEnumerator.nodeEnumerator;

    // UnmanagedLinkedListNodeEnumerator<UnmanagedPtrLinkedListNode<TValue>> => this
    public static implicit operator UnmanagedPtrLinkedListValueEnumerator<TValue>(UnmanagedLinkedListNodeEnumerator<UnmanagedPtrLinkedListNode<TValue>> nodeEnumerator)
        => new UnmanagedPtrLinkedListValueEnumerator<TValue>(nodeEnumerator);

    #endregion


    #region Instance

    #region Field & Property

    internal UnmanagedLinkedListNodeEnumerator<UnmanagedPtrLinkedListNode<TValue>> nodeEnumerator;

    unsafe public TValue* CurrentValue 
        => nodeEnumerator.CurrentNodePtr->Value;
    unsafe TValue* IPtrEnumerator<TValue>.CurrentPtr => CurrentValue;

    #endregion


    #region Constructor

    public UnmanagedPtrLinkedListValueEnumerator(UnmanagedPtrLinkedList<TValue> list)
        : this(list.GetNodeEnumerator()) { }
    public UnmanagedPtrLinkedListValueEnumerator(UnmanagedPtrLinkedListValueEnumerator<TValue> enumerator)
        : this(enumerator.nodeEnumerator) { }
    public UnmanagedPtrLinkedListValueEnumerator(UnmanagedLinkedListNodeEnumerator<UnmanagedPtrLinkedListNode<TValue>> nodeEnumerator)
    {
        this.nodeEnumerator = nodeEnumerator;
    }

    #endregion


    #region Method

    public bool MoveNext()
        => nodeEnumerator.MoveNext();
    bool IPtrEnumerator<TValue>.PtrMoveNext()
        => MoveNext();
    
    public void Reset()
        => nodeEnumerator.Reset();

    public UnmanagedPtrLinkedListValueEnumerator<TValue> Copy()
        => new UnmanagedPtrLinkedListValueEnumerator<TValue>(this);

    #endregion
 
    #endregion
}

unsafe public struct UnmanagedPtrLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator>
    : IPtrEnumerator<TValue>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedPtrLinkedListNode<TValue, TNode>
    where TNodeEnumerator : IUnmanagedLinkedListNodeEnumerator<TNode>, new()
{
    #region Static

    // this => TNodeEnumerator
    public static implicit operator TNodeEnumerator(UnmanagedPtrLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator> valueEnumerator)
        => valueEnumerator.nodeEnumerator;

    // TNodeEnumerator => this
    public static implicit operator UnmanagedPtrLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator>(TNodeEnumerator nodeEnumerator)
        => new UnmanagedPtrLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator>(nodeEnumerator);

    #endregion


    #region Instance

    #region Field & Property

    internal TNodeEnumerator nodeEnumerator;

    unsafe public TValue* CurrentValue 
        => nodeEnumerator.CurrentNodePtr->Value;
    unsafe TValue* IPtrEnumerator<TValue>.CurrentPtr => CurrentValue;

    public bool IsEnd 
        => nodeEnumerator.IsEnd;

    #endregion


    #region Constructor

    public UnmanagedPtrLinkedListValueEnumerator(UnmanagedPtrLinkedList<TValue, TNode, TNodeEnumerator> list)
        : this(list.GetNodeEnumerator()) { }
    public UnmanagedPtrLinkedListValueEnumerator(UnmanagedPtrLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator> enumerator)
        : this(enumerator.nodeEnumerator) { }
    public UnmanagedPtrLinkedListValueEnumerator(TNodeEnumerator nodeEnumerator)
    {
        this.nodeEnumerator = nodeEnumerator;
    }

    #endregion


    #region Method

    public bool MoveNext()
        => nodeEnumerator.MoveNext();
    bool IPtrEnumerator<TValue>.PtrMoveNext()
        => MoveNext();
    
    public void Reset()
        => nodeEnumerator.Reset();

    public UnmanagedPtrLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator> Copy()
        => new UnmanagedPtrLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator>(this);

    #endregion

    #endregion
}
