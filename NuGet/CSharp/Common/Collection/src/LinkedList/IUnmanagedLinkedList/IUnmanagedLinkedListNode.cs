namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IUnmanagedLinkedListNode<TValue, TNode> 
    : IReadOnlyUnmanagedLinkedListNode<TValue, TNode>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
{
    #region Static

    public static TNode CreateNode(TValue value)
    {
        TNode newNode = new ();
        newNode.Value = value;

        return newNode;
    }
    public static TNode CreateNode(TValue value, TNode* nextNodePtr)
    {
        TNode newNode = new ();
        newNode.Value = value;
        newNode.NextNodePtr = nextNodePtr;

        return newNode;
    }

    #endregion


    #region Instance

    nint ValuePtrOffset { get; }

    new TValue Value { get; set;}
    TValue IReadOnlyUnmanagedLinkedListNode<TValue, TNode>.Value => Value;

    /// <summary>
    /// 다음 노드의 포인터입니다.
    /// </summary>
    TNode* NextNodePtr { get; set; }

    #endregion
}