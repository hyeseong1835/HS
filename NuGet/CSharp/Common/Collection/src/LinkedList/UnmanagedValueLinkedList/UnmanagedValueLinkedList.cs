using System.Runtime.InteropServices;

namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IDefaultUnmanagedValueLinkedList
{
    #region Static

    static TNode CreateNode<TValue, TNode>(TValue value)
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
    {
        TNode newNode = new ();
        newNode.Value = value;

        return newNode;
    }
    static TNode* AllocNodePtr<TValue, TNode>()
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
    {
        TNode* newNodePtr = (TNode*)Marshal.AllocHGlobal(sizeof(TNode));

        return newNodePtr;
    }
    static TNode* AllocNodePtr<TValue, TNode>(TNode node)
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
    {
        TNode* newNodePtr = (TNode*)Marshal.AllocHGlobal(sizeof(TNode));
        *newNodePtr = node;

        return newNodePtr;
    }
    
    static TNode* AllocNodePtr<TValue, TNode>(TValue value)
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
    {
        TNode* newNodePtr = (TNode*)Marshal.AllocHGlobal(sizeof(TNode));
        newNodePtr->Value = value;

        return newNodePtr;
    }
    static TNode* AllocNodePtr<TValue, TNode>(TValue value, TNode* nextNodePtr)
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
    {
        TNode* newNodePtr = (TNode*)Marshal.AllocHGlobal(sizeof(TNode));
        newNodePtr->Value = value;
        newNodePtr->NextNodePtr = nextNodePtr;

        return newNodePtr;
    }

    protected static void AddFirst<TValue, TNode>(TValue value, ref TNode* headNodePtr, ref TNode* tailNodePtr, ref int count)
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
    {
        // 노드 생성 (new -> head)
        TNode* newNodePtr = AllocNodePtr<TValue, TNode>(value, headNodePtr);

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
        where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
    {
        // 노드 생성
        TNode* newNodePtr = AllocNodePtr<TValue, TNode>(value);

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
        where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
    {
        if (prevNodePtr == null)
            throw new InvalidOperationException("이전 노드 포인터가 null입니다.");

        // 노드 생성
        TNode* newNodePtr = AllocNodePtr<TValue, TNode>();
        *newNodePtr = newNode;

        prevNodePtr->NextNodePtr = newNodePtr;

        count++;
    }
    protected static void RemoveFirst<TValue, TNode>(ref TNode* headNodePtr, ref int count)
        where TValue : unmanaged
        where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
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
        where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
        where TList : IUnmanagedLinkedList<TNode>
    {
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
        where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
        where TNodeEnumerator : IUnmanagedLinkedListNodeEnumerator<TNode>, new()
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
unsafe public interface IDefaultUnmanagedValueLinkedList<TValue, TNode, TNodeEnumerator>
    : IDefaultUnmanagedValueLinkedList,
      IUnmanagedLinkedList<TNode>,
      IExplicitEnumerable<TValue, UnmanagedValueLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator>>,
      IExplicitPtrEnumerable<TNode, TNodeEnumerator>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
    where TNodeEnumerator : IUnmanagedLinkedListNodeEnumerator<TNode>, new ()
{
    #region Method

    public void AddFirst(TValue value);
    
    public void AddLast(TValue value);
    
    public void InsertNextTo(TNode* prevNodePtr, TNode newNode);
    public void InsertNextTo(TValue* prevValuePtr, TValue value);
    
    public void RemoveFirst();

    public void Link<TList>(TList list)
        where TList : IUnmanagedLinkedList<TNode>;

    public UnmanagedValueLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator> GetValueEnumerator();
    UnmanagedValueLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator> IExplicitEnumerable<TValue, UnmanagedValueLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator>>.GetEnumerator()
        => GetValueEnumerator();

    public TNodeEnumerator GetNodeEnumerator();
    TNodeEnumerator IExplicitPtrEnumerable<TNode, TNodeEnumerator>.GetPtrEnumerator()
        => GetNodeEnumerator();

    #endregion
}


/// <summary>
/// 관리되지 않는 메모리에서 싱글 링크드 리스트를 구현합니다. <br/>
/// <br/>
/// 더 이상 사용하지 않을 때 Dispose()를 호출해야합니다.
/// </summary>
/// <typeparam name="TValue"></typeparam>
[StructLayout(LayoutKind.Sequential)]
unsafe public struct UnmanagedValueLinkedList<TValue>
    : IDefaultUnmanagedValueLinkedList<
        TValue, 
        UnmanagedValueLinkedListNode<TValue>,
        UnmanagedLinkedListNodeEnumerator<UnmanagedValueLinkedListNode<TValue>>
      >
    where TValue : unmanaged
{
    #region Static

    public static UnmanagedValueLinkedListNode<TValue>* GetNodePtrByValuePtr(TValue* valuePtr)
    {
        if (valuePtr == null)
            throw new ArgumentNullException(nameof(valuePtr));

        return ((UnmanagedValueLinkedListNode<TValue>*)(nint)valuePtr - UnmanagedValueLinkedListNode<TValue>.valuePtrOffset);
    }

    #endregion


    #region Instance

    #region Field & Property

    UnmanagedValueLinkedListNode<TValue>* headNodePtr;
    public UnmanagedValueLinkedListNode<TValue>* HeadNodePtr => headNodePtr;
    public TValue HeadValue => (headNodePtr->Value);

    UnmanagedValueLinkedListNode<TValue>* tailNodePtr;
    public UnmanagedValueLinkedListNode<TValue>* TailNodePtr => tailNodePtr;
    public TValue TailValue => tailNodePtr->Value;

    int count;
    public int Count {
        get => count;
        set => count = value;
    }

    public bool IsEmpty => (count == 0);

    #endregion


    #region Constructor

    public UnmanagedValueLinkedList()
    {
        this.headNodePtr = null;
        this.count = 0;
    }

    #endregion

    #region Method

    public void AddFirst(TValue value)
        => IDefaultUnmanagedValueLinkedList.AddFirst(value, ref headNodePtr, ref tailNodePtr, ref count);

    public void AddLast(TValue value)
        => IDefaultUnmanagedValueLinkedList.AddLast(value, ref headNodePtr, ref tailNodePtr, ref count);

    public void InsertNextTo(UnmanagedValueLinkedListNode<TValue>* prevNodePtr, UnmanagedValueLinkedListNode<TValue> newNode)
        => IDefaultUnmanagedValueLinkedList.InsertNextTo<TValue, UnmanagedValueLinkedListNode<TValue>>(prevNodePtr, newNode, ref count);

    public void InsertNextTo(TValue* prevValuePtr, TValue value)
        => IDefaultUnmanagedValueLinkedList.InsertNextTo<TValue, UnmanagedValueLinkedListNode<TValue>>(
            GetNodePtrByValuePtr(prevValuePtr), 
            IDefaultUnmanagedValueLinkedList.CreateNode<TValue, UnmanagedValueLinkedListNode<TValue>>(value), 
            ref count
        );
    
    public void RemoveFirst()
        => IDefaultUnmanagedValueLinkedList.RemoveFirst<TValue, UnmanagedValueLinkedListNode<TValue>>(ref headNodePtr, ref count);

    public void Link<TList>(TList list)
        where TList : IUnmanagedLinkedList<UnmanagedValueLinkedListNode<TValue>>
        => IDefaultUnmanagedValueLinkedList.Link<TValue, UnmanagedValueLinkedListNode<TValue>, TList>(list, ref headNodePtr, ref tailNodePtr, ref count);
    
    public void Dispose()
        => IDefaultUnmanagedValueLinkedList.Dispose<TValue, UnmanagedValueLinkedListNode<TValue>, UnmanagedLinkedListNodeEnumerator<UnmanagedValueLinkedListNode<TValue>>>(
            ref headNodePtr, 
            ref tailNodePtr, 
            ref count
        );


    public UnmanagedLinkedListNodeEnumerator<UnmanagedValueLinkedListNode<TValue>> GetNodeEnumerator()
    {
        UnmanagedLinkedListNodeEnumerator<UnmanagedValueLinkedListNode<TValue>> enumerator = new ();
        ((IUnmanagedLinkedListNodeEnumerator<UnmanagedValueLinkedListNode<TValue>>)enumerator).Init(headNodePtr);

        return enumerator;
    }

    public UnmanagedValueLinkedListValueEnumerator<TValue, UnmanagedValueLinkedListNode<TValue>, UnmanagedLinkedListNodeEnumerator<UnmanagedValueLinkedListNode<TValue>>> GetValueEnumerator()
    {
        UnmanagedValueLinkedListValueEnumerator<TValue, UnmanagedValueLinkedListNode<TValue>, UnmanagedLinkedListNodeEnumerator<UnmanagedValueLinkedListNode<TValue>>> enumerator = new (headNodePtr);

        return enumerator;
    }

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
unsafe public struct UnmanagedValueLinkedList<TValue, TNode, TNodeEnumerator>
    : IDefaultUnmanagedValueLinkedList<TValue, TNode, TNodeEnumerator>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
    where TNodeEnumerator : IUnmanagedLinkedListNodeEnumerator<TNode>, new()
{
    #region Static

    public static nint valuePtrOffset = new TNode().ValuePtrOffset;

    public static TNode CreateNode(TValue value)
        => IDefaultUnmanagedValueLinkedList.CreateNode<TValue, TNode>(value);

    public static TNode* GetNodePtrByValuePtr(TValue* valuePtr)
    {
        if (valuePtr == null)
            throw new ArgumentNullException(nameof(valuePtr));

        return ((TNode*)(nint)valuePtr - valuePtrOffset);
    }

    #endregion


    #region Instance

    #region Field & Property

    TNode* headNodePtr;
    public TNode* HeadNodePtr => headNodePtr;
    public TValue HeadValue => (headNodePtr->Value);

    TNode* tailNodePtr;
    public TNode* TailNodePtr => tailNodePtr;
    public TValue TailValue => tailNodePtr->Value;

    int count;
    public int Count {
        get => count;
        set => count = value;
    }

    public bool IsEmpty => (count == 0);

    #endregion


    #region Constructor

    public UnmanagedValueLinkedList()
    {
        this.headNodePtr = null;
        this.count = 0;
    }

    #endregion

    #region Method

    public void AddFirst(TValue value)
        => IDefaultUnmanagedValueLinkedList.AddFirst(value, ref headNodePtr, ref tailNodePtr, ref count);

    public void AddLast(TValue value)
        => IDefaultUnmanagedValueLinkedList.AddLast(value, ref headNodePtr, ref tailNodePtr, ref count);

    public void InsertNextTo(TNode* prevNodePtr, TNode newNode)
        => IDefaultUnmanagedValueLinkedList.InsertNextTo<TValue, TNode>(prevNodePtr, newNode, ref count);

    public void InsertNextTo(TValue* prevValuePtr, TValue value)
        => IDefaultUnmanagedValueLinkedList.InsertNextTo<TValue, TNode>(
            GetNodePtrByValuePtr(prevValuePtr), 
            CreateNode(value), 
            ref count
        );
    
    public void RemoveFirst()
        => IDefaultUnmanagedValueLinkedList.RemoveFirst<TValue, TNode>(ref headNodePtr, ref count);

    public void Link<TList>(TList list)
        where TList : IUnmanagedLinkedList<TNode>
        => IDefaultUnmanagedValueLinkedList.Link<TValue, TNode, TList>(list, ref headNodePtr, ref tailNodePtr, ref count);
    
    public void Dispose()
        => IDefaultUnmanagedValueLinkedList.Dispose<TValue, TNode, TNodeEnumerator>(ref headNodePtr, ref tailNodePtr, ref count);


    public TNodeEnumerator GetNodeEnumerator()
    {
        TNodeEnumerator enumerator = new ();
        enumerator.Init(headNodePtr);

        return enumerator;
    }

    public UnmanagedValueLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator> GetValueEnumerator()
        => new UnmanagedValueLinkedListValueEnumerator<TValue, TNode, TNodeEnumerator>(headNodePtr);

    #endregion

    #endregion
}