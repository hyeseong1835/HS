namespace HS.CSharp.Common.Collection;

/// <summary>
/// 관리되지 않는 메모리에서 싱글 링크드 리스트 기반 스택을 구현합니다. <br/>
/// <br/>
/// 더 이상 사용하지 않을 때 Dispose()를 호출해야합니다.
/// </summary>
/// <typeparam name="T"></typeparam>
unsafe public ref struct UnmanagedSpanStack<T>
    where T : unmanaged
{
    Span<T> span;
    int count;
    public int Count => count;
    public bool IsEmpty => count == 0;

    public UnmanagedSpanStack(Span<T> buffer)
    {
        this.span = buffer;
        this.count = 0;
    }

    public void Push(T value)
    {
        if (count >= span.Length)
            throw new InvalidOperationException("버퍼가 가득 찼습니다.");

        span[count++] = value;
    }
    public T PushAndGet(T value)
    {
        if (count >= span.Length)
            throw new InvalidOperationException("버퍼가 가득 찼습니다.");

        span[count++] = value;

        return value;
    }

    public T Pop()
    {
        if (count <= 0)
            throw new InvalidOperationException("버퍼가 비었습니다.");

        T value = span[--count];

        return value;
    }

    public T Peek()
    {
        if (count <= 0)
            throw new InvalidOperationException("버퍼가 비었습니다.");

        T value = span[count - 1];

        return value;
    }
}