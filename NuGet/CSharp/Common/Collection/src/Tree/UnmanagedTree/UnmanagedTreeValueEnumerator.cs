using System.Collections;

namespace HS.CSharp.Common.Collection.Unmanaged;

public struct UnmanagedTreeValueEnumerator<T> : IEnumerator<T>
    where T : unmanaged
{
    UnmanagedTreeNodeEnumerator<T> nodeEnumerator;

    public T Current => nodeEnumerator.Current.value;
    object IEnumerator.Current => nodeEnumerator.Current.value;


    #region Constructor

    unsafe public UnmanagedTreeValueEnumerator(UnmanagedTree<T>* treePtr) 
    {
        this.nodeEnumerator = new UnmanagedTreeNodeEnumerator<T>(treePtr);
    }

    unsafe public UnmanagedTreeValueEnumerator(UnmanagedTreeNode<T>* rootNodePtr)
    {
        this.nodeEnumerator = new UnmanagedTreeNodeEnumerator<T>(rootNodePtr);
    }

    #endregion


    #region Method

    void IEnumerator.Reset() => throw new NotImplementedException();

    public bool PtrMoveNext() => nodeEnumerator.PtrMoveNext();
    
    public void Dispose() => nodeEnumerator.Dispose();

    #endregion
}