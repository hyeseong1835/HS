namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IUnmanagedPtrLinkedListNode<TValue, TNode> 
    : IUnmanagedValueLinkedListNode<nint, TNode>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedPtrLinkedListNode<TValue, TNode> 
{
    #region Static

    public static TNode CreateNode(TValue* value)
    {
        TNode newNode = new ();
        newNode.Value = value;

        return newNode;
    }
    public static TNode CreateNode(TValue* value, TNode* nextNodePtr)
    {
        TNode newNode = new ();
        newNode.Value = value;
        newNode.NextNodePtr = nextNodePtr;

        return newNode;
    }

    #endregion


    #region Instance

    nint IUnmanagedValueLinkedListNode<nint, TNode>.Value { 
        get => (nint)Value;
        set => Value = (TValue*)value;
    }
    new TValue* Value { get; set; }

    #endregion
}