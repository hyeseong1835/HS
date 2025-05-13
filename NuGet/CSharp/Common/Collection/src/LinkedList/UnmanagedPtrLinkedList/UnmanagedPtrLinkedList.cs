using System.Runtime.InteropServices;

namespace HS.CSharp.Common.Collection.Unmanaged;

/// <summary>
/// 관리되지 않는 메모리에서 싱글 링크드 리스트를 구현합니다. <br/>
/// <br/>
/// 더 이상 사용하지 않을 때 Dispose()를 호출해야합니다.
/// </summary>
/// <typeparam name="TValue"></typeparam>
[StructLayout(LayoutKind.Sequential)]
unsafe public struct UnmanagedPtrLinkedList<TValue>
    : IUnmanagedValueLinkedList<nint, UnmanagedPtrLinkedListNode<TValue>>,
      IExplicitPtrEnumerable<TValue, UnmanagedPtrLinkedListValueEnumerator<TValue>>
    where TValue : unmanaged
{
    #region Field & Property

    internal UnmanagedValueLinkedList<nint, UnmanagedPtrLinkedListNode<TValue>, UnmanagedLinkedListNodeEnumerator<UnmanagedPtrLinkedListNode<TValue>>> rawList;
    
    unsafe UnmanagedPtrLinkedListNode<TValue>* IUnmanagedLinkedList<UnmanagedPtrLinkedListNode<TValue>>.HeadNodePtr => rawList.HeadNodePtr;
    unsafe UnmanagedPtrLinkedListNode<TValue>* IUnmanagedLinkedList<UnmanagedPtrLinkedListNode<TValue>>.TailNodePtr => rawList.TailNodePtr;

    public TValue* HeadValue => (TValue*)rawList.HeadValue;
    nint IUnmanagedValueLinkedList<nint, UnmanagedPtrLinkedListNode<TValue>>.HeadValue => rawList.HeadValue;

    public TValue* TailValue => (TValue*)rawList.TailValue;
    nint IUnmanagedValueLinkedList<nint, UnmanagedPtrLinkedListNode<TValue>>.TailValue => rawList.TailValue;

    public int Count {
        get => rawList.Count;
        set => rawList.Count = value;
    }
    public bool IsEmpty => rawList.IsEmpty;

    #endregion


    #region Constructor

    public UnmanagedPtrLinkedList()
        : this(new UnmanagedValueLinkedList<nint, UnmanagedPtrLinkedListNode<TValue>, UnmanagedLinkedListNodeEnumerator<UnmanagedPtrLinkedListNode<TValue>>>()) { }
    public UnmanagedPtrLinkedList(UnmanagedValueLinkedList<nint, UnmanagedPtrLinkedListNode<TValue>, UnmanagedLinkedListNodeEnumerator<UnmanagedPtrLinkedListNode<TValue>>> rawList)
    {
        this.rawList = rawList;
    }
    

    #endregion

    #region Method

    public void AddFirst(TValue* value)
        => rawList.AddFirst((nint)value);

    public void AddLast(TValue* value)
        => rawList.AddLast((nint)value);

    public void InsertNextTo(UnmanagedPtrLinkedListNode<TValue>* prevNodePtr, UnmanagedPtrLinkedListNode<TValue> newNode)
        => rawList.InsertNextTo(prevNodePtr, newNode);

    public void InsertNextTo(TValue** prevValuePtr, TValue* value)
        => rawList.InsertNextTo((nint*)prevValuePtr, (nint)value);
    
    public void RemoveFirst()
        => rawList.RemoveFirst();

    public void Link<TList>(TList list)
        where TList : IUnmanagedLinkedList<UnmanagedPtrLinkedListNode<TValue>>
        => this.rawList.Link(list);
    
    public void Dispose()
        => rawList.Dispose();

    public UnmanagedPtrLinkedListValueEnumerator<TValue> GetValueEnumerator()
        => new UnmanagedPtrLinkedListValueEnumerator<TValue>(this);
    UnmanagedPtrLinkedListValueEnumerator<TValue> IExplicitPtrEnumerable<TValue, UnmanagedPtrLinkedListValueEnumerator<TValue>>.GetPtrEnumerator()
        => GetValueEnumerator();

    public UnmanagedLinkedListNodeEnumerator<UnmanagedPtrLinkedListNode<TValue>> GetNodeEnumerator()
        => rawList.GetNodeEnumerator();

    #endregion
}


/// <summary>
/// 관리되지 않는 메모리에서 싱글 링크드 리스트를 구현합니다. <br/>
/// <br/>
/// 더 이상 사용하지 않을 때 Dispose()를 호출해야합니다.
/// </summary>
/// <typeparam name="TValue"></typeparam>
[StructLayout(LayoutKind.Sequential)]
unsafe public struct UnmanagedPtrLinkedList<TValue, TNode, TNodeEnumerator>
    : IUnmanagedValueLinkedList<nint, TNode>, 
      IExplicitPtrEnumerable<TValue, UnmanagedPtrLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator>>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedPtrLinkedListNode<TValue, TNode>
    where TNodeEnumerator : IUnmanagedLinkedListNodeEnumerator<TNode>, new()
{
    #region Field & Property

    internal UnmanagedValueLinkedList<nint, TNode, TNodeEnumerator> rawList;
    
    unsafe TNode* IUnmanagedLinkedList<TNode>.HeadNodePtr => rawList.HeadNodePtr;
    unsafe TNode* IUnmanagedLinkedList<TNode>.TailNodePtr => rawList.TailNodePtr;

    public TValue* HeadValue => (TValue*)rawList.HeadValue;
    nint IUnmanagedValueLinkedList<nint, TNode>.HeadValue => rawList.HeadValue;

    public TValue* TailValue => (TValue*)rawList.TailValue;
    nint IUnmanagedValueLinkedList<nint, TNode>.TailValue => rawList.TailValue;

    public int Count => rawList.Count;
    int IUnmanagedLinkedList<TNode>.Count {
        get => rawList.Count;
        set => rawList.Count = value;
    }
    public bool IsEmpty => rawList.IsEmpty;

    #endregion


    #region Constructor

    public UnmanagedPtrLinkedList()
        : this(new UnmanagedValueLinkedList<nint, TNode, TNodeEnumerator>()) { }
    public UnmanagedPtrLinkedList(UnmanagedValueLinkedList<nint, TNode, TNodeEnumerator> rawList)
    {
        this.rawList = rawList;
    }

    #endregion

    #region Method

    public void AddFirst(TValue* value)
        => rawList.AddFirst((nint)value);

    public void AddLast(TValue* value)
        => rawList.AddLast((nint)value);

    public void InsertNextTo(TNode* prevNodePtr, TNode newNode)
        => rawList.InsertNextTo(prevNodePtr, newNode);

    public void InsertNextTo(TValue** prevValuePtr, TValue* value)
        => rawList.InsertNextTo((nint*)prevValuePtr, (nint)value);
    
    public void RemoveFirst()
        => rawList.RemoveFirst();

    public void Link<TList>(TList list)
        where TList : IUnmanagedLinkedList<TNode>
        => this.rawList.Link(list);
    
    public void Dispose()
        => rawList.Dispose();

    public UnmanagedPtrLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator> GetValueEnumerator()
        => new UnmanagedPtrLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator>(this);
    UnmanagedPtrLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator> IExplicitPtrEnumerable<TValue, UnmanagedPtrLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator>>.GetPtrEnumerator()
        => GetValueEnumerator();
    
    public TNodeEnumerator GetNodeEnumerator()
        => rawList.GetNodeEnumerator();

    #endregion
}