using System.Runtime.InteropServices;

namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public struct UnmanagedTreeNode<T> : ITreeNode<T, UnmanagedTreeNode<T>, UnmanagedLinkedList<UnmanagedTreeNode<T>>>
    where T : unmanaged
{
    #region Static

    public static UnmanagedTreeNode<T>* CreatePtr()
    {
        UnmanagedTreeNode<T>* nodePtr = (UnmanagedTreeNode<T>*)Marshal.AllocHGlobal(sizeof(UnmanagedTreeNode<T>));
        *nodePtr = new UnmanagedTreeNode<T>();

        return nodePtr;
    }
    public static UnmanagedTreeNode<T>* CreatePtr(T value)
    {
        UnmanagedTreeNode<T>* nodePtr = (UnmanagedTreeNode<T>*)Marshal.AllocHGlobal(sizeof(UnmanagedTreeNode<T>));
        *nodePtr = new UnmanagedTreeNode<T>(value);
        
        return nodePtr;
    }

    #endregion


    #region Instance
    
    public T value;
    public T Value {
        get => value;
        set => this.value = value;
    }

    public readonly UnmanagedLinkedList<UnmanagedTreeNode<T>> childList;
    public UnmanagedLinkedList<UnmanagedTreeNode<T>> ChildList => childList;

    public UnmanagedTreeNode()
    {
        this.value = default(T);
        this.childList = new UnmanagedLinkedList<UnmanagedTreeNode<T>>();
    }
    public UnmanagedTreeNode(T value)
    {
        this.value = value;
        this.childList = new UnmanagedLinkedList<UnmanagedTreeNode<T>>();
    }

    public void InsertChildNext(UnmanagedTreeNode<T>* childNodePtr, T value)
        => childList.InsertNextTo(childNodePtr, new UnmanagedTreeNode<T>(value));

    public void AddChildFirst(T value)
        => childList.AddFirst(new UnmanagedTreeNode<T>(value));
    
    public void AddChildLast(T value)
        => childList.AddLast(new UnmanagedTreeNode<T>(value));

    #endregion
}