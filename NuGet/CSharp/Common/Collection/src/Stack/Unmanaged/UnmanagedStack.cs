namespace HS.CSharp.Common.Collection.Unmanaged;

/// <summary>
/// 관리되지 않는 메모리에서 싱글 링크드 리스트 기반 스택을 구현합니다. <br/>
/// <br/>
/// 더 이상 사용하지 않을 때 Dispose()를 호출해야합니다.
/// </summary>
/// <typeparam name="T"></typeparam>
unsafe public struct UnmanagedStack<T> : IDisposable
    where T : unmanaged
{
    #region Static

    public static implicit operator UnmanagedStack<T>(UnmanagedLinkedList<T> list)
        => new UnmanagedStack<T>(list);

    #endregion


    #region Instance

    UnmanagedLinkedList<T> list;
    public int Count => list.Count;
    public bool IsEmpty => list.IsEmpty;

    public UnmanagedStack()
    {
        list = new UnmanagedLinkedList<T>();
    }
    public UnmanagedStack(UnmanagedLinkedList<T> list)
    {
        this.list = list;
    }

    public void Push(T value)
    {
        list.AddFirst(value);
    }
    public T PushAndGet(T value)
    {
        list.AddFirst(value);

        return value;
    }

    public T Pop()
    {
        T value = list.HeadValue;

        list.RemoveFirst();

        return value;
    }

    public T Peek()
        => list.HeadValue;

    public void Link<TList>(TList list)
        where TList : IUnmanagedLinkedList<T, UnmanagedLinkedListNode<T>>
    {
        this.list.Link(list);
    }
    public void Dispose()
        => list.Dispose();

    #endregion
}