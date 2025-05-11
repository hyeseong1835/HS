using System.Collections;

namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public struct UnmanagedTreeNodeEnumerator<TValue> 
    : IEnumerator<UnmanagedTreeNode<TValue>>, IPointerEnumerator<UnmanagedTreeNode<TValue>>
    where TValue : unmanaged
{
    UnmanagedStack<IntPtr> findNodePtrStack;

    UnmanagedTreeNode<TValue>* currentPtr;
    public UnmanagedTreeNode<TValue>* CurrentPtr => currentPtr;
    public UnmanagedTreeNode<TValue> Current => *currentPtr;
    object IEnumerator.Current => Current;


    #region Constructor

    public UnmanagedTreeNodeEnumerator(IUnmanagedTree<TValue, UnmanagedTreeNode<TValue>>* treePtr) 
        : this((IntPtr)treePtr + treePtr->RootNodePtrOffset) { }

    public UnmanagedTreeNodeEnumerator(UnmanagedTreeNode<TValue>* rootNodePtr)
    {
        this.findNodePtrStack = new UnmanagedStack<IntPtr>();
        findNodePtrStack.Push((IntPtr)rootNodePtr);

        this.currentPtr = null;
    }

    #endregion


    #region Method

    #region 명시적 인터페이스 구현

    void IEnumerator.Reset()
        => throw new NotImplementedException();

    #endregion

    public bool MoveNext()
    {
        // 모두 탐색함 -> false 반환
        if (findNodePtrStack.IsEmpty) {
            currentPtr = null;
            return false;
        }

        currentPtr = (UnmanagedTreeNode<TValue>*)findNodePtrStack.Pop();

        findNodePtrStack.Link(currentPtr->ChildList);
        return true;
    }
    public void Link(UnmanagedLinkedList<UnmanagedTreeNode<TValue>> childNodeList)
    {
        UnmanagedLinkedList<IntPtr> childNodePtrList = new ();
        for (int i = 0; i < childNodeList.Count; i++)
        {
            childNodePtrList.AddLast((IntPtr)childNodeList[i]);
        }
    }
    
    public void Dispose()
    {
        findNodePtrStack.Dispose();
    }

    #endregion
}