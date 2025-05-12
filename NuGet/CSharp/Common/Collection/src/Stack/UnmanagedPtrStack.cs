using System.Runtime.InteropServices;

namespace HS.CSharp.Common.Collection.Unmanaged;

/// <summary>
/// 관리되지 않는 메모리에서 싱글 링크드 리스트 기반 스택을 구현합니다. <br/>
/// <br/>
/// 더 이상 사용하지 않을 때 Dispose()를 호출해야합니다.
/// </summary>
/// <typeparam name="T"></typeparam>
unsafe public struct UnmanagedPtrStack<T> : IDisposable
    where T : unmanaged
{
    #region Static

    public static explicit operator UnmanagedPtrStack<T>(UnmanagedPtrLinkedList<T> list)
        => new UnmanagedPtrStack<T>(list);

    #endregion


    #region Instance

    UnmanagedPtrLinkedList<T> list;
    public int Count => list.Count;
    public bool IsEmpty => list.IsEmpty;

    public UnmanagedPtrStack()
        : this(new UnmanagedPtrLinkedList<T>()) { }
    public UnmanagedPtrStack(UnmanagedPtrLinkedList<T> list)
    {
        this.list = list;
    }

    public void Push(T* ptr)
    {
        list.AddFirst(ptr);
    }
    public T* PushAndGet(T* ptr)
    {
        list.AddFirst(ptr);

        return ptr;
    }

    public T* Pop()
    {
        T* ptr = list.HeadValue;

        list.RemoveFirst();

        return ptr;
    }

    public T* Peek()
        => list.HeadValue;

    public void Link<TList>(TList list)
        where TList : IUnmanagedLinkedList<nint, UnmanagedPtrLinkedListNode<T>>
    {
        this.list.Link(list);
    }
    public void LinkPtrEnumerator<TPtrEnumerator>(TPtrEnumerator ptrEnumerator)
        where TPtrEnumerator : IPtrEnumerator<T>
    {
        UnmanagedPtrLinkedList<T> list = new UnmanagedPtrLinkedList<T>();

        while (ptrEnumerator.PtrMoveNext())
        {
            list.AddLast(ptrEnumerator.CurrentPtr);
        }
    }
    public void LinkEnumerator<TEnumerator>(TEnumerator ptrEnumerator)
        where TEnumerator : IEnumerator<nint>
    {
        UnmanagedPtrLinkedList<T> list = new UnmanagedPtrLinkedList<T>();

        while (ptrEnumerator.MoveNext())
        {
            list.AddLast((T*)ptrEnumerator.Current);
        }
    }
    public void DisposeAndFree()
    {
        while (list.Count > 0)
        {
            T* ptr = Pop();
            Marshal.FreeHGlobal((IntPtr)ptr);
        }
        
        list.Dispose();
    }
    public void Dispose()
        => list.Dispose();

    #endregion
}