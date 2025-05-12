using System.Runtime.InteropServices;

namespace HS.CSharp.Common.Collection;

[StructLayout(LayoutKind.Sequential)]
unsafe public struct UnmanagedLinkedListNode<TValue> : IUnmanagedLinkedListNode<TValue, UnmanagedLinkedListNode<TValue>>
    where TValue : unmanaged
{
    #region Static

    public static IntPtr valuePtrOffset
        => (IntPtr)(&((UnmanagedLinkedListNode<TValue>*)0)->value);


    public static UnmanagedLinkedListNode<TValue>* CreateNode()
    {
        UnmanagedLinkedListNode<TValue>* newNodePtr = (UnmanagedLinkedListNode<TValue>*)Marshal.AllocHGlobal(sizeof(UnmanagedLinkedListNode<TValue>));

        return newNodePtr;
    }
    public static UnmanagedLinkedListNode<TValue>* CreateNode(TValue value, UnmanagedLinkedListNode<TValue>* nextNodePtr = null)
    {
        UnmanagedLinkedListNode<TValue>* newNodePtr = (UnmanagedLinkedListNode<TValue>*)Marshal.AllocHGlobal(sizeof(UnmanagedLinkedListNode<TValue>));
        newNodePtr->Value = value;
        newNodePtr->NextNodePtr = nextNodePtr;

        return newNodePtr;
    }

    #endregion


    #region Instance
    
    IntPtr IUnmanagedLinkedListNode<TValue, UnmanagedLinkedListNode<TValue>>.ValuePtrOffset => valuePtrOffset;

    public TValue value;
    public TValue Value {
        get => value;
        set => this.value = value;
    }

    public UnmanagedLinkedListNode<TValue>* nextNodePtr;
    public UnmanagedLinkedListNode<TValue>* NextNodePtr {
        get => nextNodePtr;
        set => nextNodePtr = value;
    }

    // 생성자
    public UnmanagedLinkedListNode(TValue value, UnmanagedLinkedListNode<TValue>* nextNodePtr)
    {
        this.value = value;
        this.nextNodePtr = nextNodePtr;
    }

    #endregion
}