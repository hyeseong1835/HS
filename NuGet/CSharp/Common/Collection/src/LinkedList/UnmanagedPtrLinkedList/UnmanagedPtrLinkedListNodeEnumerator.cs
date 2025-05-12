namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public struct UnmanagedPtrLinkedListNodeEnumerator<TValue> 
    : IUnmanagedLinkedListNodeEnumerator<nint, UnmanagedPtrLinkedListNode<TValue>>
    where TValue : unmanaged
{
    #region Static

    public static implicit operator UnmanagedPtrLinkedListNodeEnumerator<TValue>(UnmanagedPtrLinkedListNodeEnumerator<TValue, UnmanagedPtrLinkedListNode<TValue>> enumerator)
        => new UnmanagedPtrLinkedListNodeEnumerator<TValue>(enumerator);

    public static implicit operator UnmanagedPtrLinkedListNodeEnumerator<TValue, UnmanagedPtrLinkedListNode<TValue>>(UnmanagedPtrLinkedListNodeEnumerator<TValue> enumerator)
        => new UnmanagedPtrLinkedListNodeEnumerator<TValue, UnmanagedPtrLinkedListNode<TValue>>(enumerator);

    #endregion


    #region Instance

    #region Field & Property

    UnmanagedLinkedListNodeEnumerator<nint, UnmanagedPtrLinkedListNode<TValue>> rawNodeEnumerator;

    unsafe public UnmanagedPtrLinkedListNode<TValue>* HeadNodePtr { 
        get => rawNodeEnumerator.HeadNodePtr; 
        set => rawNodeEnumerator.HeadNodePtr = value; 
    }

    unsafe public UnmanagedPtrLinkedListNode<TValue>* CurrentNodePtr 
        => rawNodeEnumerator.CurrentNodePtr;

    public bool IsEnd 
        => rawNodeEnumerator.IsEnd;

    #endregion


    #region Constructor

    public UnmanagedPtrLinkedListNodeEnumerator(UnmanagedPtrLinkedList<TValue> list)
        : this(list.rawList) { }
    public UnmanagedPtrLinkedListNodeEnumerator(UnmanagedLinkedList<nint, UnmanagedPtrLinkedListValueEnumerator<TValue>, UnmanagedPtrLinkedListNode<TValue>, UnmanagedPtrLinkedListNodeEnumerator<TValue>> rawList)
        : this(rawList.HeadNodePtr) { }


    public UnmanagedPtrLinkedListNodeEnumerator(UnmanagedPtrLinkedListNode<TValue>* listHeadNodePtr)
        : this(new UnmanagedLinkedListNodeEnumerator<nint, UnmanagedPtrLinkedListNode<TValue>>(listHeadNodePtr)) { }
    
    /// <summary>
    /// 복사합니다.
    /// </summary>
    /// <param name="enumerator"></param>
    public UnmanagedPtrLinkedListNodeEnumerator(UnmanagedPtrLinkedListNodeEnumerator<TValue> enumerator)
        : this(enumerator.rawNodeEnumerator) { }

    public UnmanagedPtrLinkedListNodeEnumerator(UnmanagedLinkedListNodeEnumerator<nint, UnmanagedPtrLinkedListNode<TValue>> rawNodeEnumerator)
    {
        this.rawNodeEnumerator = rawNodeEnumerator;
    }

    #endregion


    #region Method

    void IDisposable.Dispose() => throw new NotImplementedException();

    public bool MoveNext()
        => rawNodeEnumerator.MoveNext();
    bool IPtrEnumerator<UnmanagedPtrLinkedListNode<TValue>>.PtrMoveNext()
        => rawNodeEnumerator.MoveNext();
    
    public void Reset()
        => rawNodeEnumerator.Reset();

    public UnmanagedPtrLinkedListNodeEnumerator<TValue> Copy()
        => new UnmanagedPtrLinkedListNodeEnumerator<TValue>(this);

    #endregion

    #endregion
}


unsafe public struct UnmanagedPtrLinkedListNodeEnumerator<TValue, TNode>
  : IUnmanagedLinkedListNodeEnumerator<nint, TNode>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedLinkedListNode<nint, TNode>
{
    #region Static
    public static nint valuePtrOffset = new TNode().ValuePtrOffset;

    #endregion


    #region Instance

    #region Field & Property

    UnmanagedLinkedListNodeEnumerator<nint, TNode> rawNodeEnumerator;

    unsafe public TNode* HeadNodePtr { 
        get => rawNodeEnumerator.HeadNodePtr; 
        set => rawNodeEnumerator.HeadNodePtr = value; 
    }

    unsafe public TNode* CurrentNodePtr 
        => rawNodeEnumerator.CurrentNodePtr;

    public TValue* CurrentValuePtr 
        => (TValue*)((nint)rawNodeEnumerator.CurrentNodePtr + valuePtrOffset);

    public bool IsEnd 
        => rawNodeEnumerator.IsEnd;

    #endregion


    #region Constructor

    /// <summary>
    /// 복사합니다.
    /// </summary>
    /// <param name="enumerator"></param>
    public UnmanagedPtrLinkedListNodeEnumerator(UnmanagedPtrLinkedListNodeEnumerator<TValue, TNode> enumerator)
        : this(enumerator.rawNodeEnumerator) { }

    public UnmanagedPtrLinkedListNodeEnumerator(UnmanagedLinkedListNodeEnumerator<nint, TNode> rawNodeEnumerator)
    {
        this.rawNodeEnumerator = rawNodeEnumerator;
    }

    #endregion


    #region Method

    void IDisposable.Dispose() => throw new NotImplementedException();

    public bool MoveNext()
        => rawNodeEnumerator.MoveNext();
    bool IPtrEnumerator<TNode>.PtrMoveNext()
        => rawNodeEnumerator.MoveNext();
    
    public void Reset()
        => rawNodeEnumerator.Reset();

    public UnmanagedPtrLinkedListNodeEnumerator<TValue, TNode> Copy()
        => new UnmanagedPtrLinkedListNodeEnumerator<TValue, TNode>(this);

    #endregion
    
    #endregion
}