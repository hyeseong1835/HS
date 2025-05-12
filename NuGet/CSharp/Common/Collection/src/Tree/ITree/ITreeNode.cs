namespace HS.CSharp.Common.Collection.Unmanaged;

public interface ITreeNode<TValue, TNode, TChildList>
    where TNode : ITreeNode<TValue, TNode, TChildList>
    where TChildList : IEnumerable<TNode>
{
    TValue Value { get; set; }
    TChildList ChildList { get; }
}