using System.Collections;

namespace HS.CSharp.Common.Collection;

unsafe public struct UnmanagedPtrLinkedListValueEnumerator<TValue> : IEnumerator<IntPtr>
    where TValue : unmanaged
{
    #region Field & Property

    object IEnumerator.Current => throw new NotImplementedException();

    UnmanagedLinkedListNode<IntPtr>* headNodePtr;

    UnmanagedLinkedListNode<IntPtr>* currentNodePtr;
    public TValue* Current => (TValue*)currentNodePtr->Value;
    IntPtr IEnumerator<IntPtr>.Current => currentNodePtr->Value;

    bool isEnd;

    #endregion


    #region Constructor

    public UnmanagedPtrLinkedListValueEnumerator(UnmanagedLinkedListNode<IntPtr>* listHeadNodePtr)
    {
        this.headNodePtr = listHeadNodePtr;
        this.currentNodePtr = null;
        this.isEnd = (listHeadNodePtr == null);
    }
    public UnmanagedPtrLinkedListValueEnumerator(UnmanagedPtrLinkedListValueEnumerator<TValue> enumerator)
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

    public UnmanagedPtrLinkedListValueEnumerator<TValue> Copy()
        => new UnmanagedPtrLinkedListValueEnumerator<TValue>(this);

    #endregion
}

unsafe public struct UnmanagedPtrLinkedListValueEnumerator<TValue, TNode> : IEnumerator<IntPtr>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedLinkedListNode<IntPtr, TNode>
{
    #region Field & Property

    object IEnumerator.Current => throw new NotImplementedException();

    TNode* headNodePtr;

    TNode* currentNodePtr;
    public TValue* Current => (TValue*)currentNodePtr->Value;
    IntPtr IEnumerator<IntPtr>.Current => currentNodePtr->Value;

    bool isEnd;

    #endregion


    #region Constructor

    public UnmanagedPtrLinkedListValueEnumerator(TNode* listHeadNodePtr)
    {
        this.headNodePtr = listHeadNodePtr;
        this.currentNodePtr = null;
        this.isEnd = (listHeadNodePtr == null);
    }
    public UnmanagedPtrLinkedListValueEnumerator(UnmanagedPtrLinkedListValueEnumerator<TValue, TNode> enumerator)
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

    public UnmanagedPtrLinkedListValueEnumerator<TValue, TNode> Copy()
        => new UnmanagedPtrLinkedListValueEnumerator<TValue, TNode>(this);

    #endregion
}
