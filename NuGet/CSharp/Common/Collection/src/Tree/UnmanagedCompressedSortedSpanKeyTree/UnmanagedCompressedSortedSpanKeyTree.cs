namespace HS.CSharp.Common.Collection.Unmanaged;

public class CompressedSortedSpanKeyTree<TKey, TValue> 
    where TKey : unmanaged
{
    #region Object

    public class Node
    {
        public TValue value;
    }

    #endregion


    public UnmanagedCompressedSortedSpanKeyTree(UnmanagedCompressedSortedSpanKeyTreeNode<TKey, TValue, TNode> root)
    {
        _root = root;
    }

    public UnmanagedCompressedSortedSpanKeyTreeNode<TKey, TValue, TNode> Root => _root;
}