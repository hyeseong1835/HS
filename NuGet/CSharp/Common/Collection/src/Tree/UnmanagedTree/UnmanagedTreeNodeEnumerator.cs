using System.Collections;

namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public struct UnmanagedTreeNodeEnumerator<T> 
    : IEnumerator<UnmanagedTreeNode<T>>, IPtrEnumerator<UnmanagedTreeNode<T>>
    where T : unmanaged
{
    UnmanagedPtrStack<UnmanagedTreeNode<T>> findNodePtrStack;

    UnmanagedTreeNode<T>* currentPtr;
    public UnmanagedTreeNode<T>* CurrentPtr => currentPtr;
    public UnmanagedTreeNode<T> Current => *currentPtr;
    object IEnumerator.Current => Current;


    #region Constructor

    public UnmanagedTreeNodeEnumerator(UnmanagedTree<T>* treePtr) 
        : this(treePtr->rootNodePtr) { }

    public UnmanagedTreeNodeEnumerator(UnmanagedTreeNode<T>* rootNodePtr)
    {
        this.findNodePtrStack = new UnmanagedPtrStack<UnmanagedTreeNode<T>>();
        findNodePtrStack.Push(rootNodePtr);

        this.currentPtr = null;
    }

    #endregion


    #region Method

    #region 명시적 인터페이스 구현

    void IEnumerator.Reset()
        => throw new NotImplementedException();

    #endregion

    public bool PtrMoveNext()
    {
        // 모두 탐색함 -> false 반환
        if (findNodePtrStack.IsEmpty) {
            currentPtr = null;
            return false;
        }

        currentPtr = findNodePtrStack.Pop();

        findNodePtrStack.Link(currentPtr->ChildList);
        return true;
    }
    public void Link(UnmanagedLinkedList<UnmanagedTreeNode<T>> childNodeList)
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