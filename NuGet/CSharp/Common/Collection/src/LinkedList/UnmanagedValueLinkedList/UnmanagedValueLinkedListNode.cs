using System.Runtime.InteropServices;

namespace HS.CSharp.Common.Collection.Unmanaged;

[StructLayout(LayoutKind.Sequential)]
unsafe public struct UnmanagedValueLinkedListNode<TValue> 
    : IUnmanagedValueLinkedListNode<TValue, UnmanagedValueLinkedListNode<TValue>>
    where TValue : unmanaged
{
    #region Static

    public static IntPtr valuePtrOffset
        => (IntPtr)(&((UnmanagedValueLinkedListNode<TValue>*)0)->value);

    public static UnmanagedValueLinkedListNode<TValue>* CreateNodePtr(TValue value, UnmanagedValueLinkedListNode<TValue>* nextNodePtr = null)
    {
        UnmanagedValueLinkedListNode<TValue>* newNodePtr = (UnmanagedValueLinkedListNode<TValue>*)Marshal.AllocHGlobal(sizeof(UnmanagedValueLinkedListNode<TValue>));
        newNodePtr->Value = value;
        newNodePtr->NextNodePtr = nextNodePtr;

        return newNodePtr;
    }

    #endregion


    #region Instance

    public nint ValuePtrOffset => (nint)valuePtrOffset;
    
    public TValue value;
    public TValue Value {
        get => value;
        set => this.value = value;
    }

    public UnmanagedValueLinkedListNode<TValue>* nextNodePtr;
    public UnmanagedValueLinkedListNode<TValue>* NextNodePtr {
        get => nextNodePtr;
        set => nextNodePtr = value;
    }

    // 생성자
    public UnmanagedValueLinkedListNode(TValue value, UnmanagedValueLinkedListNode<TValue>* nextNodePtr = null)
    {
        this.value = value;
        this.nextNodePtr = nextNodePtr;
    }

    #endregion
}