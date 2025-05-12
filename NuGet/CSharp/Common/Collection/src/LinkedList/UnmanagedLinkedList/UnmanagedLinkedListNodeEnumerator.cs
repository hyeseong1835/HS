using HS.CSharp.Common.Collection.Unmanaged;
using System.Collections;

namespace HS.CSharp.Common.Collection;

unsafe public struct UnmanagedLinkedListNodeEnumerator<TValue> 
  : IUnmanagedLinkedListNodeEnumerator<TValue, UnmanagedLinkedListNode<TValue>>
    where TValue : unmanaged
{
    #region Static

    public static implicit operator UnmanagedLinkedListNodeEnumerator<TValue>(UnmanagedLinkedListNodeEnumerator<TValue, UnmanagedLinkedListNode<TValue>> enumerator)
        => new UnmanagedLinkedListNodeEnumerator<TValue>(enumerator);

    public static implicit operator UnmanagedLinkedListNodeEnumerator<TValue, UnmanagedLinkedListNode<TValue>>(UnmanagedLinkedListNodeEnumerator<TValue> enumerator)
        => new UnmanagedLinkedListNodeEnumerator<TValue, UnmanagedLinkedListNode<TValue>>(enumerator);

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

    bool isEnd;
    public bool IsEnd => isEnd;

    #endregion


    #region Constructor

    public UnmanagedLinkedListNodeEnumerator(UnmanagedLinkedListNode<TValue>* listHeadNodePtr)
    {
        this.headNodePtr = listHeadNodePtr;
        this.currentNodePtr = null;
        this.isEnd = (listHeadNodePtr == null);
    }
    public UnmanagedLinkedListNodeEnumerator(UnmanagedLinkedListNodeEnumerator<TValue> enumerator)
    {
        this.headNodePtr = enumerator.headNodePtr;
        this.currentNodePtr = enumerator.currentNodePtr;
        this.isEnd = enumerator.isEnd;
    }
    public UnmanagedLinkedListNodeEnumerator(UnmanagedLinkedListNodeEnumerator<TValue, UnmanagedLinkedListNode<TValue>> enumerator)
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

    public UnmanagedLinkedListNodeEnumerator<TValue> Copy()
        => new UnmanagedLinkedListNodeEnumerator<TValue>(this);

    #endregion

    #endregion
}


unsafe public struct UnmanagedLinkedListNodeEnumerator<TValue, TNode> 
  : IUnmanagedLinkedListNodeEnumerator<TValue, TNode>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
{
    #region Field & Property

    object IEnumerator.Current => throw new NotImplementedException();

    internal TNode* headNodePtr;
    public TNode* HeadNodePtr {
        get => headNodePtr;
        set => headNodePtr = value;
    }

    internal TNode* currentNodePtr;
    public TNode* CurrentNodePtr => currentNodePtr;

    internal bool isEnd;
    public bool IsEnd => isEnd;

    #endregion


    #region Constructor

    public UnmanagedLinkedListNodeEnumerator(TNode* listHeadNodePtr)
    {
        this.headNodePtr = listHeadNodePtr;
        this.currentNodePtr = null;
        this.isEnd = (listHeadNodePtr == null);
    }
    public UnmanagedLinkedListNodeEnumerator(UnmanagedLinkedListNodeEnumerator<TValue, TNode> enumerator)
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

    public UnmanagedLinkedListNodeEnumerator<TValue, TNode> Copy()
        => new UnmanagedLinkedListNodeEnumerator<TValue, TNode>(this);

    #endregion
}