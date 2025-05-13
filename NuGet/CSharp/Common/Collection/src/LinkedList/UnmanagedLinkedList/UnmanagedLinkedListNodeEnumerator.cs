using System.Collections;

namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public struct UnmanagedLinkedListNodeEnumerator<TNode> 
  : IUnmanagedLinkedListNodeEnumerator<TNode>
    where TNode : unmanaged, IUnmanagedLinkedListNode<TNode>
{
    #region Field & Property

    TNode* headNodePtr;
    public TNode* HeadNodePtr {
        get => headNodePtr;
        set => headNodePtr = value;
    }

    TNode* currentNodePtr;
    public TNode* CurrentNodePtr => currentNodePtr;

    public TNode CurrentNode => *currentNodePtr;
    object IEnumerator.Current => CurrentNode;

    bool isEnd;
    public bool IsEnd => isEnd;

    #endregion


    #region Constructor

    public UnmanagedLinkedListNodeEnumerator(TNode* listHeadNodePtr)
    {
        this.headNodePtr = listHeadNodePtr;
        this.currentNodePtr = null;
        this.isEnd = (listHeadNodePtr == null);
    }
    public UnmanagedLinkedListNodeEnumerator(UnmanagedLinkedListNodeEnumerator<TNode> enumerator)
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
    bool IEnumerator.MoveNext()
        => MoveNext();
    bool IPtrEnumerator<TNode>.PtrMoveNext()
        => MoveNext();
    
    public void Reset()
    {
        currentNodePtr = null;
        isEnd = false;
    }

    public UnmanagedLinkedListNodeEnumerator<TNode> Copy()
        => new UnmanagedLinkedListNodeEnumerator<TNode>(this);

    #endregion
}