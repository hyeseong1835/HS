using System.Collections;

namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public struct UnmanagedLinkedListValueEnumerator<TValue>
    : IUnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>>
    where TValue : unmanaged
{
    #region Static

    public static implicit operator UnmanagedLinkedListValueEnumerator<TValue>(UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>> enumerator)
        => new UnmanagedLinkedListValueEnumerator<TValue>(enumerator);

    public static implicit operator UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>>(UnmanagedLinkedListValueEnumerator<TValue> enumerator)
        => new UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>>(enumerator);

    #endregion


    #region Instance

    #region Field & Property

    object IEnumerator.Current => throw new NotImplementedException();

    UnmanagedLinkedListNode<TValue>* headNodePtr;
    public UnmanagedLinkedListNode<TValue>* HeadNodePtr {
        get => headNodePtr;
        set => headNodePtr = value;
    }

    UnmanagedLinkedListNode<TValue>* currentNodePtr;
    public UnmanagedLinkedListNode<TValue>* CurrentNodePtr => currentNodePtr;

    public TValue Current => currentNodePtr->Value;

    bool isEnd;
    public bool IsEnd => isEnd;

    #endregion


    #region Constructor

    public UnmanagedLinkedListValueEnumerator(UnmanagedLinkedListNode<TValue>* listHeadNodePtr)
    {
        this.headNodePtr = listHeadNodePtr;
        this.currentNodePtr = null;
        this.isEnd = (listHeadNodePtr == null);
    }
    public UnmanagedLinkedListValueEnumerator(UnmanagedLinkedListValueEnumerator<TValue> enumerator)
    {
        this.headNodePtr = enumerator.headNodePtr;
        this.currentNodePtr = enumerator.currentNodePtr;
        this.isEnd = enumerator.isEnd;
    }

    #endregion


    #region Method

    void IDisposable.Dispose() => throw new NotImplementedException();

    public bool PtrMoveNext()
    {
        if (isEnd)
            return false;

        if (currentNodePtr == null)
        {
            currentNodePtr = headNodePtr;
        }
        else
        {
            currentNodePtr = currentNodePtr->NextNodePtr;
        }

        return currentNodePtr != null;
    }
    
    public void Reset()
    {
        currentNodePtr = null;
        isEnd = false;
    }

    public UnmanagedLinkedListValueEnumerator<TValue> Copy()
        => new UnmanagedLinkedListValueEnumerator<TValue>(this);

    #endregion

    #endregion
}

unsafe public struct UnmanagedLinkedListValueEnumerator<TValue, TNode>
    : IUnmanagedLinkedListValueEnumerator<TValue, TNode>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
{
    #region Field & Property

    object IEnumerator.Current => throw new NotImplementedException();

    TNode* headNodePtr;
    public TNode* HeadNodePtr {
        get => headNodePtr;
        set => headNodePtr = value;
    }

    TNode* currentNodePtr;
    public TNode* CurrentNodePtr => currentNodePtr;

    public TValue Current => currentNodePtr->Value;

    bool isEnd;
    public bool IsEnd => isEnd;

    #endregion


    #region Constructor

    public UnmanagedLinkedListValueEnumerator(TNode* listHeadNodePtr)
    {
        ((IUnmanagedLinkedListValueEnumerator<TValue, TNode>)this).Init(listHeadNodePtr);
    }
    public UnmanagedLinkedListValueEnumerator(UnmanagedLinkedListValueEnumerator<TValue, TNode> enumerator)
    {
        this.headNodePtr = enumerator.headNodePtr;
        this.currentNodePtr = enumerator.currentNodePtr;
        this.isEnd = enumerator.isEnd;
    }

    #endregion


    #region Method

    void IDisposable.Dispose() => throw new NotImplementedException();

    public bool PtrMoveNext()
    {
        if (isEnd)
            return false;

        if (currentNodePtr == null)
        {
            currentNodePtr = headNodePtr;
        }
        else
        {
            currentNodePtr = currentNodePtr->NextNodePtr;
        }

        return currentNodePtr != null;
    }
    
    public void Reset()
    {
        currentNodePtr = null;
        isEnd = false;
    }

    public UnmanagedLinkedListValueEnumerator<TValue, TNode> Copy()
        => new UnmanagedLinkedListValueEnumerator<TValue, TNode>(this);

    #endregion
}
