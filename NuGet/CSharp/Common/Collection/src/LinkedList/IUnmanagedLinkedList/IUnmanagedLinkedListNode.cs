namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IUnmanagedLinkedListNode<TNode> 
    where TNode : unmanaged, IUnmanagedLinkedListNode<TNode>
{
    #region Static

    public static TNode CreateNode()
    {
        TNode newNode = new ();

        return newNode;
    }
    public static TNode CreateNode(TNode* nextNodePtr)
    {
        TNode newNode = new ();
        newNode.NextNodePtr = nextNodePtr;

        return newNode;
    }

    #endregion


    #region Instance

    /// <summary>
    /// 다음 노드의 포인터입니다.
    /// </summary>
    TNode* NextNodePtr { get; set; }

    #endregion
}