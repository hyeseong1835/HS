using System.Runtime.InteropServices;

namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IDefaultUnmanagedLinkedList
{
    #region Static

    static TNode* CreateNode<TValue, TNode>()
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
    {
        TNode* newNodePtr = (TNode*)Marshal.AllocHGlobal(sizeof(TNode));

        return newNodePtr;
    }
    static TNode* CreateNode<TValue, TNode>(TValue value, TNode* nextNodePtr = null)
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
    {
        TNode* newNodePtr = (TNode*)Marshal.AllocHGlobal(sizeof(TNode));
        newNodePtr->Value = value;
        newNodePtr->NextNodePtr = nextNodePtr;

        return newNodePtr;
    }

    protected static void AddFirst<TValue, TNode>(TValue value, ref TNode* headNodePtr, ref TNode* tailNodePtr, ref int count)
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
    {
        // 노드 생성 (new -> head)
        TNode* newNodePtr = CreateNode<TValue, TNode>(value, headNodePtr);

        // 시작 노드 재설정
        headNodePtr = newNodePtr;

        if (count == 0) 
        {
            tailNodePtr = newNodePtr;
        }
        
        count++;
    }
    protected static void AddLast<TValue, TNode>(TValue value, ref TNode* headNodePtr, ref TNode* tailNodePtr, ref int count)
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
    {
        // 노드 생성
        TNode* newNodePtr = CreateNode<TValue, TNode>(value);

        // (tail -> new)
        tailNodePtr->NextNodePtr = newNodePtr;

        // 마지막 노드 재설정
        tailNodePtr = newNodePtr;

        if (count == 0)
        {
            headNodePtr = newNodePtr;
        }

        count++;
    }
    protected static void InsertNextTo<TValue, TNode>(TNode* prevNodePtr, TNode newNode, ref int count)
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
    {
        if (prevNodePtr == null)
            throw new InvalidOperationException("이전 노드 포인터가 null입니다.");

        // 노드 생성
        TNode* newNodePtr = CreateNode<TValue, TNode>();
        *newNodePtr = newNode;

        prevNodePtr->NextNodePtr = newNodePtr;

        count++;
    }
    protected static void InsertNextTo<TValue, TNode>(TValue* prevValuePtr, TValue value, ref int count)
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
    {
        if (prevValuePtr == null)
            throw new InvalidOperationException("이전 값 포인터가 null입니다.");

        // 노드 생성
        TNode* newNodePtr = CreateNode<TValue, TNode>();
        newNodePtr->Value = value;

        TNode* prevNodePtr = (TNode*)((IntPtr)prevValuePtr - newNodePtr->ValuePtrOffset);

        prevNodePtr->NextNodePtr = newNodePtr;

        count++;
    }
    protected static void RemoveFirst<TValue, TNode>(ref TNode* headNodePtr, ref int count)
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
    {
        if (headNodePtr == null)
            throw new InvalidOperationException("리스트가 비어 있습니다.");

        TNode* secondNodePtr = headNodePtr->NextNodePtr;

        Marshal.FreeHGlobal((IntPtr)headNodePtr);

        headNodePtr = secondNodePtr;

        count--;
    }
    protected static void Link<TValue, TNode, TList>(TList list, ref TNode* headNodePtr, ref TNode* tailNodePtr, ref int count)
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
        where TList : IUnmanagedLinkedList<TValue, TNode>
    {
        if (list.IsEmpty)
            return;

        if (tailNodePtr == null)
        {
            headNodePtr = list.HeadNodePtr;
            tailNodePtr = list.TailNodePtr;
        }
        else
        {
            tailNodePtr->NextNodePtr = list.HeadNodePtr;
            tailNodePtr = list.TailNodePtr;
        }
        
        count += list.Count;
    }
    protected static void Dispose<TValue, TNode, TNodeEnumerator>(ref TNode* headNodePtr, ref TNode* tailNodePtr, ref int count)
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
        where TNodeEnumerator : IUnmanagedLinkedListNodeEnumerator<TValue, TNode>, new()
    {
        switch (count)
        {
            case 0: break;

            case 1: {
                Marshal.FreeHGlobal((IntPtr)headNodePtr);
                break;
            }
                
            case 2: {
                Marshal.FreeHGlobal((IntPtr)headNodePtr);
                Marshal.FreeHGlobal((IntPtr)tailNodePtr);
                break;
            }
                
            default: {
                TNodeEnumerator childListEnumerator = new();
                childListEnumerator.Init(headNodePtr);

                childListEnumerator.MoveNext();
                TNode* curNodePtr = childListEnumerator.CurrentNodePtr;
                TNode* prevNodePtr;

                while (childListEnumerator.MoveNext())
                {
                    prevNodePtr = curNodePtr;

                    curNodePtr = childListEnumerator.CurrentNodePtr;

                    Marshal.FreeHGlobal((IntPtr)prevNodePtr);
                }

                Marshal.FreeHGlobal((IntPtr)curNodePtr);
                break;
            }
        }
        
        headNodePtr = null;
        tailNodePtr = null;
        count = 0;
    }
    
    #endregion
}
unsafe public interface IDefaultUnmanagedLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator>
    : IDefaultUnmanagedLinkedList,
      IExplicitEnumerableUnmanagedLinkedList<
          TValue, 
          TValueEnumerator, 
          TNode
      >,
      IPointerEnumerable<TNode>
    where TValue : unmanaged
    where TValueEnumerator : unmanaged, IUnmanagedLinkedListValueEnumerator<TValue, TNode>
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
    where TNodeEnumerator : unmanaged, IUnmanagedLinkedListNodeEnumerator<TValue, TNode>
{
    #region Instance

    #region Method

    public void AddFirst(TValue value);
    
    public void AddLast(TValue value);
    
    public void InsertNextTo(TNode* prevNodePtr, TNode newNode);
    public void InsertNextTo(TValue* prevValuePtr, TValue value);
    
    public void RemoveFirst();

    public void Link<TList>(TList list)
        where TList : IUnmanagedLinkedList<TValue, TNode>;

    public TValueEnumerator GetValueEnumerator();
    public TNodeEnumerator GetNodeEnumerator();

    #endregion

    #endregion
}


/// <summary>
/// 관리되지 않는 메모리에서 싱글 링크드 리스트를 구현합니다. <br/>
/// <br/>
/// 더 이상 사용하지 않을 때 Dispose()를 호출해야합니다.
/// </summary>
/// <typeparam name="TValue"></typeparam>
[StructLayout(LayoutKind.Sequential)]
unsafe public struct UnmanagedLinkedList<TValue>
    : IDefaultUnmanagedLinkedList<
        TValue, 
        UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>>, 
        UnmanagedLinkedListNode<TValue>,
        UnmanagedLinkedListNodeEnumerator<TValue, UnmanagedLinkedListNode<TValue>>
      >
    where TValue : unmanaged
{
    #region Field & Property

    UnmanagedLinkedListNode<TValue>* headNodePtr;
    public UnmanagedLinkedListNode<TValue>* HeadNodePtr => headNodePtr;
    public TValue HeadValue => (headNodePtr->Value);

    UnmanagedLinkedListNode<TValue>* tailNodePtr;
    public UnmanagedLinkedListNode<TValue>* TailNodePtr => tailNodePtr;
    public TValue TailValue => tailNodePtr->Value;

    int count;
    public int Count => count;

    #endregion


    #region Constructor

    public UnmanagedLinkedList()
    {
        this.headNodePtr = null;
        this.count = 0;
    }

    #endregion

    #region Method

    public void AddFirst(TValue value)
        => IDefaultUnmanagedLinkedList.AddFirst(value, ref headNodePtr, ref tailNodePtr, ref count);

    public void AddLast(TValue value)
        => IDefaultUnmanagedLinkedList.AddLast(value, ref headNodePtr, ref tailNodePtr, ref count);

    public void InsertNextTo(UnmanagedLinkedListNode<TValue>* prevNodePtr, UnmanagedLinkedListNode<TValue> newNode)
        => IDefaultUnmanagedLinkedList.InsertNextTo<TValue, UnmanagedLinkedListNode<TValue>>(prevNodePtr, newNode, ref count);

    public void InsertNextTo(TValue* prevValuePtr, TValue value)
        => IDefaultUnmanagedLinkedList.InsertNextTo<TValue, UnmanagedLinkedListNode<TValue>>(prevValuePtr, value, ref count);
    
    public void RemoveFirst()
        => IDefaultUnmanagedLinkedList.RemoveFirst<TValue, UnmanagedLinkedListNode<TValue>>(ref headNodePtr, ref count);

    public void Link<TList>(TList list)
        where TList : IUnmanagedLinkedList<TValue, UnmanagedLinkedListNode<TValue>>
        => IDefaultUnmanagedLinkedList.Link<TValue, UnmanagedLinkedListNode<TValue>, TList>(list, ref headNodePtr, ref tailNodePtr, ref count);
    
    public void Dispose()
        => IDefaultUnmanagedLinkedList.Dispose<TValue, UnmanagedLinkedListNode<TValue>, UnmanagedLinkedListNodeEnumerator<TValue>>(ref headNodePtr, ref tailNodePtr, ref count);

    UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>> IExplicitEnumerable<TValue, UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>>>.GetEnumerator()
        => new UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>>(HeadNodePtr);
    public UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>> GetValueEnumerator()
        => new UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>>(HeadNodePtr);
    
    IPointerEnumerator<UnmanagedLinkedListNode<TValue>> IPointerEnumerable<UnmanagedLinkedListNode<TValue>>.GetPointerEnumerator()
        => new UnmanagedLinkedListNodeEnumerator<TValue, UnmanagedLinkedListNode<TValue>>(HeadNodePtr);
    public UnmanagedLinkedListNodeEnumerator<TValue, UnmanagedLinkedListNode<TValue>> GetNodeEnumerator()
        => new UnmanagedLinkedListNodeEnumerator<TValue, UnmanagedLinkedListNode<TValue>>(HeadNodePtr);
    
    #endregion
}


/// <summary>
/// 관리되지 않는 메모리에서 싱글 링크드 리스트를 구현합니다. <br/>
/// <br/>
/// 더 이상 사용하지 않을 때 Dispose()를 호출해야합니다.
/// </summary>
/// <typeparam name="TValue"></typeparam>
[StructLayout(LayoutKind.Sequential)]
unsafe public struct UnmanagedLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator>
    : IDefaultUnmanagedLinkedList<TValue, TValueEnumerator, TNode, TNodeEnumerator>
    where TValue : unmanaged
    where TValueEnumerator : unmanaged, IUnmanagedLinkedListValueEnumerator<TValue, TNode>
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
    where TNodeEnumerator : unmanaged, IUnmanagedLinkedListNodeEnumerator<TValue, TNode>
{
    #region Field & Property

    TNode* headNodePtr;
    public TNode* HeadNodePtr => headNodePtr;
    public TValue HeadValue => (headNodePtr->Value);

    TNode* tailNodePtr;
    public TNode* TailNodePtr => tailNodePtr;
    public TValue TailValue => tailNodePtr->Value;

    int count;
    public int Count => count;

    #endregion


    #region Constructor

    public UnmanagedLinkedList()
    {
        this.headNodePtr = null;
        this.count = 0;
    }

    #endregion

    #region Method

    public void AddFirst(TValue value)
        => IDefaultUnmanagedLinkedList.AddFirst(value, ref headNodePtr, ref tailNodePtr, ref count);

    public void AddLast(TValue value)
        => IDefaultUnmanagedLinkedList.AddLast(value, ref headNodePtr, ref tailNodePtr, ref count);

    public void InsertNextTo(TNode* prevNodePtr, TNode newNode)
        => IDefaultUnmanagedLinkedList.InsertNextTo<TValue, TNode>(prevNodePtr, newNode, ref count);

    public void InsertNextTo(TValue* prevValuePtr, TValue value)
        => IDefaultUnmanagedLinkedList.InsertNextTo<TValue, TNode>(prevValuePtr, value, ref count);
    
    public void RemoveFirst()
        => IDefaultUnmanagedLinkedList.RemoveFirst<TValue, TNode>(ref headNodePtr, ref count);

    public void Link<TList>(TList list)
        where TList : IUnmanagedLinkedList<TValue, TNode>
        => IDefaultUnmanagedLinkedList.Link<TValue, TNode, TList>(list, ref headNodePtr, ref tailNodePtr, ref count);
    
    public void Dispose()
        => IDefaultUnmanagedLinkedList.Dispose<TValue, TNode, TNodeEnumerator>(ref headNodePtr, ref tailNodePtr, ref count);

    TValueEnumerator IExplicitEnumerable<TValue, TValueEnumerator>.GetEnumerator()
    {
        TValueEnumerator enumerator = new TValueEnumerator();
        enumerator.Init(headNodePtr);

        return enumerator;
    }
    public TValueEnumerator GetValueEnumerator()
    {
        TValueEnumerator enumerator = new TValueEnumerator();
        enumerator.Init(headNodePtr);

        return enumerator;
    }
    
    IPointerEnumerator<TNode> IPointerEnumerable<TNode>.GetPointerEnumerator()
        => new UnmanagedLinkedListNodeEnumerator<TValue, TNode>(HeadNodePtr);
    public TNodeEnumerator GetNodeEnumerator()
    {
        TNodeEnumerator enumerator = new ();
        enumerator.Init(headNodePtr);

        return enumerator;
    }
        
    #endregion
}