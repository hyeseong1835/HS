namespace HS.CSharp.Common.Collection.Unmanaged;

public struct UnmanagedPtrLinkedListValueEnumerator<TValue> 
    : IUnmanagedLinkedListValueEnumerator<nint, UnmanagedPtrLinkedListNode<TValue>>
    where TValue : unmanaged
{
    #region Static

    // this => UnmanagedPtrLinkedListValueEnumerator<TValue, UnmanagedPtrLinkedListNode<TValue>>
    public static implicit operator UnmanagedPtrLinkedListValueEnumerator<TValue, UnmanagedPtrLinkedListNode<TValue>>(UnmanagedPtrLinkedListValueEnumerator<TValue> enumerator)
        => new UnmanagedPtrLinkedListValueEnumerator<TValue, UnmanagedPtrLinkedListNode<TValue>>(enumerator);

    // UnmanagedPtrLinkedListValueEnumerator<TValue, UnmanagedPtrLinkedListNode<TValue>> => this
    public static implicit operator UnmanagedPtrLinkedListValueEnumerator<TValue>(UnmanagedPtrLinkedListValueEnumerator<TValue, UnmanagedPtrLinkedListNode<TValue>> enumerator)
        => new UnmanagedPtrLinkedListValueEnumerator<TValue>(enumerator);


    // this => UnmanagedPtrLinkedListNodeEnumerator<TValue>
    public static implicit operator UnmanagedPtrLinkedListNodeEnumerator<TValue>(UnmanagedPtrLinkedListValueEnumerator<TValue> enumerator)
        => enumerator.nodeEnumerator;

    // UnmanagedPtrLinkedListNodeEnumerator<TValue> => this
    public static implicit operator UnmanagedPtrLinkedListValueEnumerator<TValue>(UnmanagedPtrLinkedListNodeEnumerator<TValue> nodeEnumerator)
        => new UnmanagedPtrLinkedListValueEnumerator<TValue>(nodeEnumerator);

    #endregion


    #region Instance

    #region Field & Property

    internal UnmanagedPtrLinkedListNodeEnumerator<TValue> nodeEnumerator;

    unsafe UnmanagedPtrLinkedListNode<TValue>* IUnmanagedLinkedListValueEnumerator<nint, UnmanagedPtrLinkedListNode<TValue>>.HeadNodePtr { 
        get => nodeEnumerator.HeadNodePtr;
        set => nodeEnumerator.HeadNodePtr = value;
    }

    unsafe public UnmanagedPtrLinkedListNode<TValue>* CurrentNodePtr 
        => nodeEnumerator.CurrentNodePtr;

    bool IUnmanagedLinkedListValueEnumerator<nint, UnmanagedPtrLinkedListNode<TValue>>.IsEnd 
        => nodeEnumerator.IsEnd;


    #endregion


    #region Constructor

    public UnmanagedPtrLinkedListValueEnumerator(UnmanagedPtrLinkedList<TValue> list)
        : this(list.GetNodeEnumerator()) { }
    public UnmanagedPtrLinkedListValueEnumerator(UnmanagedPtrLinkedListValueEnumerator<TValue> enumerator)
        : this(enumerator.nodeEnumerator) { }
    public UnmanagedPtrLinkedListValueEnumerator(UnmanagedPtrLinkedListNodeEnumerator<TValue> nodeEnumerator)
    {
        this.nodeEnumerator = nodeEnumerator;
    }

    #endregion


    #region Method

    void IDisposable.Dispose() => throw new NotImplementedException();

    public bool MoveNext()
        => nodeEnumerator.MoveNext();
    
    public void Reset()
        => nodeEnumerator.Reset();

    public UnmanagedPtrLinkedListValueEnumerator<TValue> Copy()
        => new UnmanagedPtrLinkedListValueEnumerator<TValue>(this);

    #endregion
 
    #endregion
}

unsafe public struct UnmanagedPtrLinkedListValueEnumerator<TValue, TNode>
    : IUnmanagedLinkedListValueEnumerator<nint, TNode>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedLinkedListNode<nint, TNode>
{
    #region Static

    // this => UnmanagedPtrLinkedListNodeEnumerator<TValue, TNode>
    public static implicit operator UnmanagedPtrLinkedListNodeEnumerator<TValue, TNode>(UnmanagedPtrLinkedListValueEnumerator<TValue, TNode> enumerator)
        => enumerator.nodeEnumerator;

    // UnmanagedPtrLinkedListNodeEnumerator<TValue, TNode> => this
    public static implicit operator UnmanagedPtrLinkedListValueEnumerator<TValue, TNode>(UnmanagedPtrLinkedListNodeEnumerator<TValue, TNode> nodeEnumerator)
        => new UnmanagedPtrLinkedListValueEnumerator<TValue, TNode>(nodeEnumerator);

    #endregion


    #region Instance

    #region Field & Property

    UnmanagedPtrLinkedListNodeEnumerator<TValue, TNode> nodeEnumerator;

    unsafe public TNode* HeadNodePtr { 
        get => nodeEnumerator.HeadNodePtr;
        set => nodeEnumerator.HeadNodePtr = value;
    }

    unsafe public TNode* CurrentNodePtr 
        => nodeEnumerator.CurrentNodePtr;

    public TValue* CurrentValue
        => nodeEnumerator.CurrentValuePtr;

    public bool IsEnd 
        => nodeEnumerator.IsEnd;


    #endregion


    #region Constructor

    public UnmanagedPtrLinkedListValueEnumerator(UnmanagedPtrLinkedList<TValue, UnmanagedPtrLinkedListValueEnumerator<TValue, TNode>, TNode, UnmanagedPtrLinkedListNodeEnumerator<TValue, TNode>> list)
        : this(list.GetNodeEnumerator()) { }
    public UnmanagedPtrLinkedListValueEnumerator(UnmanagedPtrLinkedListValueEnumerator<TValue, TNode> enumerator)
        : this(enumerator.nodeEnumerator) { }
    public UnmanagedPtrLinkedListValueEnumerator(UnmanagedPtrLinkedListNodeEnumerator<TValue, TNode> nodeEnumerator)
    {
        this.nodeEnumerator = nodeEnumerator;
    }

    #endregion


    #region Method

    void IDisposable.Dispose() => throw new NotImplementedException();

    public bool MoveNext()
        => nodeEnumerator.MoveNext();
    
    public void Reset()
        => nodeEnumerator.Reset();

    public UnmanagedPtrLinkedListValueEnumerator<TValue, TNode> Copy()
        => new UnmanagedPtrLinkedListValueEnumerator<TValue, TNode>(this);

    #endregion

    #endregion
}
