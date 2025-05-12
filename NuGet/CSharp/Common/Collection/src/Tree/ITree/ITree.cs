namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface ITree<TValue, TNode, TChildList> : IEnumerable<TValue>
    where TNode : ITreeNode<TValue, TNode, TChildList>
    where TChildList : IEnumerable<TNode>
{
    /// <summary>
    /// 트리의 루트 노드입니다.
    /// </summary>
    TNode RootNode { get; }
}