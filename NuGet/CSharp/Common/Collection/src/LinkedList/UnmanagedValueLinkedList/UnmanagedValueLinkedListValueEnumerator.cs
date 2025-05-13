using System.Collections;

namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public struct UnmanagedValueLinkedListValueEnumerator<TValue>
    : IEnumerator<TValue>
    where TValue : unmanaged
{
    #region Static

    // UnmanagedValueLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>, UnmanagedLinkedListNodeEnumerator<TValue>> => this
    public static implicit operator UnmanagedValueLinkedListValueEnumerator<TValue>(UnmanagedValueLinkedListValueEnumerator<TValue, UnmanagedValueLinkedListNode<TValue>, UnmanagedLinkedListNodeEnumerator<UnmanagedValueLinkedListNode<TValue>>> enumerator)
        => new UnmanagedValueLinkedListValueEnumerator<TValue>(enumerator);

    public static implicit operator UnmanagedValueLinkedListValueEnumerator<TValue, UnmanagedValueLinkedListNode<TValue>, UnmanagedLinkedListNodeEnumerator<UnmanagedValueLinkedListNode<TValue>>>(UnmanagedValueLinkedListValueEnumerator<TValue> enumerator)
        => new UnmanagedValueLinkedListValueEnumerator<TValue, UnmanagedValueLinkedListNode<TValue>, UnmanagedLinkedListNodeEnumerator<UnmanagedValueLinkedListNode<TValue>>>(enumerator);

    #endregion


    #region Instance

    #region Field & Property

    UnmanagedLinkedListNodeEnumerator<UnmanagedValueLinkedListNode<TValue>> nodeEnumerator;
    public bool IsEnd => nodeEnumerator.IsEnd;

    public TValue CurrentValue => nodeEnumerator.CurrentNodePtr->Value;
    object IEnumerator.Current => CurrentValue;
    TValue IEnumerator<TValue>.Current => CurrentValue;

    #endregion


    #region Constructor

    public UnmanagedValueLinkedListValueEnumerator(UnmanagedValueLinkedListNode<TValue>* listHeadNodePtr)
    {
        this.nodeEnumerator = new UnmanagedLinkedListNodeEnumerator<UnmanagedValueLinkedListNode<TValue>>(listHeadNodePtr);
    }
    
    public UnmanagedValueLinkedListValueEnumerator(UnmanagedValueLinkedListValueEnumerator<TValue> enumerator)
        : this(enumerator.nodeEnumerator) { }
    public UnmanagedValueLinkedListValueEnumerator(UnmanagedLinkedListNodeEnumerator<UnmanagedValueLinkedListNode<TValue>> nodeEnumerator)
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

    public UnmanagedValueLinkedListValueEnumerator<TValue> Copy()
        => new UnmanagedValueLinkedListValueEnumerator<TValue>(this);

    #endregion

    #endregion
}

unsafe public struct UnmanagedValueLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator>
    : IEnumerator<TValue>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
    where TNodeEnumerator : IUnmanagedLinkedListNodeEnumerator<TNode>, new()
{
    #region Field & Property

    object IEnumerator.Current => throw new NotImplementedException();
    TValue IEnumerator<TValue>.Current => nodeEnumerator.CurrentNodePtr->Value;

    TNodeEnumerator nodeEnumerator;
    
    public bool IsEnd => nodeEnumerator.IsEnd;

    #endregion


    #region Constructor

    public UnmanagedValueLinkedListValueEnumerator(TNode* listHeadNodePtr)
    {
        this.nodeEnumerator = new TNodeEnumerator();
        this.nodeEnumerator.Init(listHeadNodePtr);
    }

    public UnmanagedValueLinkedListValueEnumerator(UnmanagedValueLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator> enumerator)
        : this(enumerator.nodeEnumerator) { }
    public UnmanagedValueLinkedListValueEnumerator(TNodeEnumerator nodeEnumerator)
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

    public UnmanagedValueLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator> Copy()
        => new UnmanagedValueLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator>(this);

    #endregion
}
