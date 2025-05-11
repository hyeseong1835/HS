using System.Collections;

namespace HS.CSharp.Common.Collection;

unsafe public struct UnmanagedLinkedListValueEnumerator<TValue> : IEnumerator<TValue>
    where TValue : unmanaged
{
    #region Field & Property

    object IEnumerator.Current => throw new NotImplementedException();

    UnmanagedLinkedListNode<TValue>* headNodePtr;

    UnmanagedLinkedListNode<TValue>* currentNodePtr;
    public TValue Current => currentNodePtr->Value;

    bool isEnd;

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

    public bool MoveNext()
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
}

unsafe public struct UnmanagedLinkedListValueEnumerator<TValue, TNode> : IEnumerator<TValue>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
{
    #region Field & Property

    object IEnumerator.Current => throw new NotImplementedException();

    TNode* headNodePtr;

    TNode* currentNodePtr;
    public TValue Current => currentNodePtr->Value;

    bool isEnd;

    #endregion


    #region Constructor

    public UnmanagedLinkedListValueEnumerator(TNode* listHeadNodePtr)
    {
        this.headNodePtr = listHeadNodePtr;
        this.currentNodePtr = null;
        this.isEnd = (listHeadNodePtr == null);
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

    public bool MoveNext()
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
