using System.Collections;

namespace HS.CSharp.Common.Collection.Unmanaged;

public struct UnmanagedTreeValueEnumerator<TValue> : IEnumerator<TValue>
    where TValue : unmanaged
{
    UnmanagedTreeNodeEnumerator<TValue> nodeEnumerator;

    public TValue Current => nodeEnumerator.Current.value;
    object IEnumerator.Current => nodeEnumerator.Current.value;


    #region Constructor

    public UnmanagedTreeValueEnumerator(IUnmanagedTree<TValue, UnmanagedTreeNode<TValue>> tree) 
    {
        this.nodeEnumerator = new UnmanagedTreeNodeEnumerator<TValue>(tree);
    }

    public UnmanagedTreeValueEnumerator(UnmanagedTreeNode<TValue> rootNode)
    {
        this.nodeEnumerator = new UnmanagedTreeNodeEnumerator<TValue>(rootNode);
    }
    public UnmanagedTreeValueEnumerator(UnmanagedLinkedList<UnmanagedTreeNode<TValue>> childNodeList)
    {
        this.nodeEnumerator = new UnmanagedTreeNodeEnumerator<TValue>(childNodeList);
    }

    #endregion


    #region Method

    void IEnumerator.Reset() => throw new NotImplementedException();

    public bool MoveNext() => nodeEnumerator.MoveNext();
    
    public void Dispose() => nodeEnumerator.Dispose();

    #endregion
}