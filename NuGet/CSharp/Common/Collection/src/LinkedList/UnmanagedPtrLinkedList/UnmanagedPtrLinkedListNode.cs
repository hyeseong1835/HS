using System.Runtime.InteropServices;

namespace HS.CSharp.Common.Collection.Unmanaged;

[StructLayout(LayoutKind.Sequential)]
unsafe public struct UnmanagedPtrLinkedListNode<TValue> 
    : IUnmanagedPtrLinkedListNode<TValue, UnmanagedPtrLinkedListNode<TValue>>
    where TValue : unmanaged
{
    #region Static

    public static nint valuePtrOffset
        => (nint)(&((UnmanagedPtrLinkedListNode<TValue>*)0)->value);


    public static UnmanagedPtrLinkedListNode<TValue>* CreateNode()
    {
        UnmanagedPtrLinkedListNode<TValue>* newNodePtr = (UnmanagedPtrLinkedListNode<TValue>*)Marshal.AllocHGlobal(sizeof(UnmanagedValueLinkedListNode<TValue>));

        return newNodePtr;
    }
    public static UnmanagedPtrLinkedListNode<TValue>* CreateNode(TValue* value, UnmanagedPtrLinkedListNode<TValue>* nextNodePtr = null)
    {
        UnmanagedPtrLinkedListNode<TValue>* newNodePtr = (UnmanagedPtrLinkedListNode<TValue>*)Marshal.AllocHGlobal(sizeof(UnmanagedPtrLinkedListNode<TValue>));
        newNodePtr->value = value;
        newNodePtr->NextNodePtr = nextNodePtr;

        return newNodePtr;
    }

    #endregion


    #region Instance

    public nint ValuePtrOffset => valuePtrOffset;

    TValue* value;
    public TValue* Value {
        get => value;
        set => this.value = value;
    }

    public UnmanagedPtrLinkedListNode<TValue>* nextNodePtr;
    public UnmanagedPtrLinkedListNode<TValue>* NextNodePtr {
        get => nextNodePtr;
        set => nextNodePtr = value;
    }


    public UnmanagedPtrLinkedListNode(TValue* value, UnmanagedPtrLinkedListNode<TValue>* nextNodePtr)
    {
        this.value = value;
        this.nextNodePtr = nextNodePtr;
    }

    #endregion
}