namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IUnmanagedTree<TValue, TNode> : IEnumerable<TValue>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedTreeNode<TValue, TNode>
{
    /// <summary>
    /// 트리의 루트 노드입니다.
    /// </summary>
    TNode RootNode { get; }

    TNode* RootNodePtrOffset { get; }
}