namespace HS.CSharp.Common.Collection.Unmanaged;

public interface IUnmanagedTreeNode<TValue, TNode>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedTreeNode<TValue, TNode>
{
    TValue Value { get; set; }
    UnmanagedLinkedList<TNode> ChildList { get; }
}