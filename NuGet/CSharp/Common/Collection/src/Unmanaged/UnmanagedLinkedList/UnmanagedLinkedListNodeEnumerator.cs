using HS.CSharp.Common.Collection.Unmanaged;
using System.Collections;

namespace HS.CSharp.Common.Collection;

unsafe public interface IDefaultUnmanagedLinkedListNodeEnumerator<TValue, TNode> 
    : IEnumerator<TNode>, IPointerEnumerator<TNode>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
{
    #region Static

    public static UnmanagedLinkedListNodeEnumerator<TValue, TNode> Create(TNode* listHeadNodePtr)
        => new UnmanagedLinkedListNodeEnumerator<TValue, TNode>(listHeadNodePtr);

    #endregion

    #region Instance

    TNode* HeadNodePtr { get; set; }

    TNode* CurrentNodePtr { get; set; }
    bool IsEnd { get; set; }


    void IDisposable.Dispose() => throw new NotImplementedException();

    public bool MoveNext()
    {
        if (IsEnd)
            return false;

        if (CurrentNodePtr == null)
        {
            CurrentNodePtr = HeadNodePtr;
        }
        else
        {
            CurrentNodePtr = CurrentNodePtr->NextNodePtr;
        }

        return CurrentNodePtr != null;
    }
    
    public void Reset()
    {
        CurrentNodePtr = null;
        IsEnd = false;
    }

    #endregion
}

unsafe public struct UnmanagedLinkedListNodeEnumerator<TValue, TNode> : IEnumerator<TNode>, IPointerEnumerator<TNode>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
{
    #region Field & Property

    object IEnumerator.Current => throw new NotImplementedException();

    TNode* headNodePtr;

    TNode* currentNodePtr;
    public TNode Current => *currentNodePtr;
    public TNode* CurrentPtr => currentNodePtr;
    public ref TNode CurrentRef => ref (*currentNodePtr);

    bool isEnd;

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
