namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IUnmanagedValueLinkedListNode<TValue, TNode> 
    : IUnmanagedLinkedListNode<TNode>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
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

    TValue Value { get; set;}

    #endregion
}