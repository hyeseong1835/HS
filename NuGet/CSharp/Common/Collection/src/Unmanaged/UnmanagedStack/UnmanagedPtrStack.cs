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

    public static explicit operator UnmanagedPtrStack<T>(UnmanagedLinkedList<nint> list)
        => new UnmanagedPtrStack<T>(list);

    #endregion


    #region Instance

    UnmanagedLinkedList<nint> list;
    public int Count => list.Count;
    public bool IsEmpty => list.IsEmpty;

    public UnmanagedPtrStack()
    {
        list = new UnmanagedLinkedList<nint>();
    }
    public UnmanagedPtrStack(UnmanagedLinkedList<nint> list)
    {
        this.list = list;
    }

    public void Push(T* ptr)
    {
        list.AddFirst((nint)ptr);
    }
    public T* PushAndGet(T* ptr)
    {
        list.AddFirst((nint)ptr);

        return ptr;
    }

    public T* Pop()
    {
        T* ptr = (T*)list.HeadValue;

        list.RemoveFirst();

        return ptr;
    }

    public T* Peek()
        => (T*)list.HeadValue;

    public void Link<TList>(TList list)
        where TList : IUnmanagedLinkedList<nint, UnmanagedLinkedListNode<nint>>
    {
        this.list.Link(list);
    }
    public void DisposeAndFree()
    {
        UnmanagedLinkedListValueEnumerator<nint> listNodeEnumerator = list.GetNodePointerEnumerator();
        while (listNodeEnumerator.MoveNext())
        {
            var ptr = (T*)listNodeEnumerator.CurrentPtr;
            Marshal.FreeHGlobal((IntPtr)ptr);
        }

        list.Dispose();
    }
    public void Dispose()
        => list.Dispose();

    #endregion
}