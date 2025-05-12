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
    : IUnmanagedLinkedList<nint, UnmanagedPtrLinkedListNode<TValue>>,
      IExplicitPtrEnumerable<TValue, UnmanagedPtrLinkedListValueEnumerator<TValue>>,
      IPtrEnumerable<UnmanagedPtrLinkedListNode<TValue>>
    where TValue : unmanaged
{
    #region Field & Property

    internal UnmanagedLinkedList<nint, UnmanagedPtrLinkedListValueEnumerator<TValue>, UnmanagedPtrLinkedListNode<TValue>, UnmanagedPtrLinkedListNodeEnumerator<TValue>> rawList;

    unsafe UnmanagedPtrLinkedListNode<TValue>* IUnmanagedLinkedList<nint, UnmanagedPtrLinkedListNode<TValue>>.HeadNodePtr => rawList.HeadNodePtr;

    unsafe UnmanagedPtrLinkedListNode<TValue>* IUnmanagedLinkedList<nint, UnmanagedPtrLinkedListNode<TValue>>.TailNodePtr => rawList.TailNodePtr;

    public TValue* HeadValue => (TValue*)rawList.HeadValue;
    nint IUnmanagedLinkedList<nint, UnmanagedPtrLinkedListNode<TValue>>.HeadValue => rawList.HeadValue;

    public TValue* TailValue => (TValue*)rawList.TailValue;
    nint IUnmanagedLinkedList<nint, UnmanagedPtrLinkedListNode<TValue>>.TailValue => rawList.TailValue;

    public int Count => rawList.Count;

    public bool IsEmpty => rawList.IsEmpty;

    #endregion


    #region Constructor

    public UnmanagedPtrLinkedList()
    {
        this.rawList = new UnmanagedLinkedList<nint, UnmanagedPtrLinkedListValueEnumerator<TValue>, UnmanagedPtrLinkedListNode<TValue>, UnmanagedPtrLinkedListNodeEnumerator<TValue>>();
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
        where TList : IUnmanagedLinkedList<nint, UnmanagedPtrLinkedListNode<TValue>>
        => this.rawList.Link(list);
    
    public void Dispose()
        => rawList.Dispose();

    public UnmanagedPtrLinkedListValueEnumerator<TValue> GetValueEnumerator()
        => new UnmanagedPtrLinkedListValueEnumerator<TValue>(this);
    UnmanagedPtrLinkedListValueEnumerator<TValue> IExplicitPtrEnumerable<TValue, UnmanagedPtrLinkedListValueEnumerator<TValue>>.GetEnumerator()
        => new UnmanagedPtrLinkedListValueEnumerator<TValue>(this);
    
    public UnmanagedPtrLinkedListNodeEnumerator<TValue> GetNodeEnumerator()
        => new UnmanagedPtrLinkedListNodeEnumerator<TValue>(this);
    IPtrEnumerator<UnmanagedPtrLinkedListNode<TValue>> IPtrEnumerable<UnmanagedPtrLinkedListNode<TValue>>.GetPointerEnumerator()
        => new UnmanagedPtrLinkedListNodeEnumerator<TValue>(this);

    #endregion
}


/// <summary>
/// 관리되지 않는 메모리에서 싱글 링크드 리스트를 구현합니다. <br/>
/// <br/>
/// 더 이상 사용하지 않을 때 Dispose()를 호출해야합니다.
/// </summary>
/// <typeparam name="TValue"></typeparam>
[StructLayout(LayoutKind.Sequential)]
unsafe public struct UnmanagedPtrLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator>
    : IUnmanagedLinkedList<nint, TNode>,
      IExplicitPtrEnumerable<TValue, TValueEnumerator>,
      IPtrEnumerable<TNode>
    where TValue : unmanaged
    where TValueEnumerator : IUnmanagedLinkedListValueEnumerator<nint, TNode>, new()
    where TNode : unmanaged, IUnmanagedLinkedListNode<nint, TNode>
    where TNodeEnumerator : IUnmanagedLinkedListNodeEnumerator<nint, TNode>, new()
{
    #region Field & Property

    internal UnmanagedLinkedList<nint, TValueEnumerator, TNode, TNodeEnumerator> rawList;

    unsafe TNode* IUnmanagedLinkedList<nint, TNode>.HeadNodePtr => rawList.HeadNodePtr;

    unsafe TNode* IUnmanagedLinkedList<nint, TNode>.TailNodePtr => rawList.TailNodePtr;

    public TValue* HeadValue => (TValue*)rawList.HeadValue;
    nint IUnmanagedLinkedList<nint, TNode>.HeadValue => rawList.HeadValue;

    public TValue* TailValue => (TValue*)rawList.TailValue;
    nint IUnmanagedLinkedList<nint, TNode>.TailValue => rawList.TailValue;

    public int Count => rawList.Count;

    #endregion


    #region Constructor

    public UnmanagedPtrLinkedList()
    {
        this.rawList = new UnmanagedLinkedList<nint, TValueEnumerator, TNode, TNodeEnumerator>();
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
        where TList : IUnmanagedLinkedList<nint, TNode>
        => this.rawList.Link(list);
    
    public void Dispose()
        => rawList.Dispose();

    public TValueEnumerator GetValueEnumerator()
        => rawList.GetValueEnumerator();
    TValueEnumerator IExplicitPtrEnumerable<TValue, TValueEnumerator>.GetEnumerator()
        => rawList.GetValueEnumerator();
    
    public TNodeEnumerator GetNodeEnumerator()
        => rawList.GetNodeEnumerator();
    IPtrEnumerator<TNode> IPtrEnumerable<TNode>.GetPointerEnumerator()
        => rawList.GetNodeEnumerator();

    #endregion
}