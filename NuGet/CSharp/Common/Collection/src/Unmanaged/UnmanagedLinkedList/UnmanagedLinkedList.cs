using System.Runtime.InteropServices;

namespace HS.CSharp.Common.Collection.Unmanaged;

/// <summary>
/// 관리되지 않는 메모리에서 싱글 링크드 리스트를 구현합니다. <br/>
/// <br/>
/// 더 이상 사용하지 않을 때 Dispose()를 호출해야합니다.
/// </summary>
/// <typeparam name="TValue"></typeparam>
unsafe public struct UnmanagedLinkedList<TValue>
    : IExplicitEnumerableUnmanagedLinkedList<
        TValue, 
        UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>>, 
        UnmanagedLinkedListNode<TValue>
      >,
      IPointerEnumerable<UnmanagedLinkedListNode<TValue>>
    where TValue : unmanaged
{
    #region Static

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

    #region Field & Property

    UnmanagedLinkedListNode<TValue>* headNodePtr;
    public UnmanagedLinkedListNode<TValue>* HeadNodePtr {
        get => headNodePtr;
        set => headNodePtr = value;
    }
    public TValue HeadValue => (headNodePtr->Value);

    UnmanagedLinkedListNode<TValue>* tailNodePtr;
    public UnmanagedLinkedListNode<TValue>* TailNodePtr {
        get => tailNodePtr;
        set => tailNodePtr = value;
    }
    public TValue TailValue => tailNodePtr->Value;

    int count;
    public int Count => count;
    public bool IsEmpty => count == 0;

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
    {
        // 노드 생성 (new -> head)
        UnmanagedLinkedListNode<TValue>* newNodePtr = CreateNode(value, headNodePtr);

        // 시작 노드 재설정
        headNodePtr = newNodePtr;

        if (count == 0) 
        {
            tailNodePtr = newNodePtr;
        }
        
        count++;
    }
    
    public void AddLast(TValue value)
    {
        // 노드 생성
        UnmanagedLinkedListNode<TValue>* newNodePtr = CreateNode(value, null);

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
    
    public void InsertNextTo(UnmanagedLinkedListNode<TValue>* prevNodePtr, UnmanagedLinkedListNode<TValue> newNode)
    {
        if (prevNodePtr == null)
            throw new InvalidOperationException("이전 노드 포인터가 null입니다.");

        // 노드 생성
        UnmanagedLinkedListNode<TValue>* newNodePtr = CreateNode();
        *newNodePtr = newNode;

        prevNodePtr->NextNodePtr = newNodePtr;

        count++;
    }
    public void InsertNextTo(TValue* prevValuePtr, TValue value)
    {
        if (prevValuePtr == null)
            throw new InvalidOperationException("이전 값 포인터가 null입니다.");

        // 노드 생성
        UnmanagedLinkedListNode<TValue>* newNodePtr = CreateNode();
        newNodePtr->Value = value;

        UnmanagedLinkedListNode<TValue>* prevNodePtr = (UnmanagedLinkedListNode<TValue>*)((IntPtr)prevValuePtr - UnmanagedLinkedListNode<TValue>.valuePtrOffset);

        prevNodePtr->NextNodePtr = newNodePtr;

        count++;
    }
    
    public void RemoveFirst()
    {
        if (headNodePtr == null)
            throw new InvalidOperationException("리스트가 비어 있습니다.");

        UnmanagedLinkedListNode<TValue>* secondNodePtr = headNodePtr->NextNodePtr;

        Marshal.FreeHGlobal((IntPtr)headNodePtr);

        headNodePtr = secondNodePtr;
    }

    public void Link<TList>(TList list)
        where TList : IUnmanagedLinkedList<TValue, UnmanagedLinkedListNode<TValue>>
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
    
    UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>> IExplicitEnumerable<TValue, UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>>>.GetEnumerator()
        => new UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>>(headNodePtr);
    public UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>> GetValueEnumerator()
        => new UnmanagedLinkedListValueEnumerator<TValue, UnmanagedLinkedListNode<TValue>>(headNodePtr);
    
    IPointerEnumerator<UnmanagedLinkedListNode<TValue>> IPointerEnumerable<UnmanagedLinkedListNode<TValue>>.GetPointerEnumerator()
        => new UnmanagedLinkedListNodeEnumerator<TValue, UnmanagedLinkedListNode<TValue>>(headNodePtr);
    public UnmanagedLinkedListNodeEnumerator<TValue, UnmanagedLinkedListNode<TValue>> GetNodePointerEnumerator()
        => new UnmanagedLinkedListNodeEnumerator<TValue, UnmanagedLinkedListNode<TValue>>(headNodePtr);
    
    public void Dispose()
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
                UnmanagedLinkedListNodeEnumerator<TValue, UnmanagedLinkedListNode<TValue>> childListEnumerator 
                    = GetNodePointerEnumerator();

                childListEnumerator.MoveNext();
                UnmanagedLinkedListNode<TValue>* curNodePtr = childListEnumerator.CurrentPtr;
                UnmanagedLinkedListNode<TValue>* prevNodePtr;

                while (childListEnumerator.MoveNext())
                {
                    prevNodePtr = curNodePtr;

                    curNodePtr = childListEnumerator.CurrentPtr;

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

    #endregion
}

/// <summary>
/// 관리되지 않는 메모리에서 싱글 링크드 리스트를 구현합니다. <br/>
/// <br/>
/// 더 이상 사용하지 않을 때 Dispose()를 호출해야합니다.
/// </summary>
/// <typeparam name="TValue"></typeparam>
[StructLayout(LayoutKind.Sequential)]
unsafe public struct UnmanagedLinkedList<TValue, TNode>
    : IExplicitEnumerableUnmanagedLinkedList<
        TValue, 
        UnmanagedLinkedListValueEnumerator<TValue, TNode>, 
        TNode
      >,
      IPointerEnumerable<TNode>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
{
    #region Static

    public static TNode* CreateNode()
    {
        TNode* newNodePtr = (TNode*)Marshal.AllocHGlobal(sizeof(TNode));

        return newNodePtr;
    }
    public static TNode* CreateNode(TValue value, TNode* nextNodePtr = null)
    {
        TNode* newNodePtr = (TNode*)Marshal.AllocHGlobal(sizeof(TNode));
        newNodePtr->Value = value;
        newNodePtr->NextNodePtr = nextNodePtr;

        return newNodePtr;
    }

    #endregion


    #region Instance

    #region Field & Property

    TNode* headNodePtr;
    public TNode* HeadNodePtr {
        get => headNodePtr;
        set => headNodePtr = value;
    }
    public TValue HeadValue => (headNodePtr->Value);

    TNode* tailNodePtr;
    public TNode* TailNodePtr {
        get => tailNodePtr;
        set => tailNodePtr = value;
    }
    public TValue TailValue => tailNodePtr->Value;

    int count;
    public int Count => count;
    public bool IsEmpty => count == 0;

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
    {
        // 노드 생성 (new -> head)
        TNode* newNodePtr = CreateNode(value, headNodePtr);

        // 시작 노드 재설정
        headNodePtr = newNodePtr;

        if (count == 0) 
        {
            tailNodePtr = newNodePtr;
        }
        
        count++;
    }
    
    public void AddLast(TValue value)
    {
        // 노드 생성
        TNode* newNodePtr = CreateNode(value);

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
    
    public void InsertNextTo(TNode* prevNodePtr, TNode newNode)
    {
        if (prevNodePtr == null)
            throw new InvalidOperationException("이전 노드 포인터가 null입니다.");

        // 노드 생성
        TNode* newNodePtr = CreateNode();
        *newNodePtr = newNode;

        prevNodePtr->NextNodePtr = newNodePtr;

        count++;
    }
    public void InsertNextTo(TValue* prevValuePtr, TValue value)
    {
        if (prevValuePtr == null)
            throw new InvalidOperationException("이전 값 포인터가 null입니다.");

        // 노드 생성
        TNode* newNodePtr = CreateNode();
        newNodePtr->Value = value;

        TNode* prevNodePtr = (TNode*)((IntPtr)prevValuePtr - newNodePtr->ValuePtrOffset);

        prevNodePtr->NextNodePtr = newNodePtr;

        count++;
    }
    
    public void RemoveFirst()
    {
        if (headNodePtr == null)
            throw new InvalidOperationException("리스트가 비어 있습니다.");

        TNode* secondNodePtr = headNodePtr->NextNodePtr;

        Marshal.FreeHGlobal((IntPtr)headNodePtr);

        headNodePtr = secondNodePtr;
    }

    public void Link<TList>(TList list)
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
    
    UnmanagedLinkedListValueEnumerator<TValue, TNode> IExplicitEnumerable<TValue, UnmanagedLinkedListValueEnumerator<TValue, TNode>>.GetEnumerator()
        => new UnmanagedLinkedListValueEnumerator<TValue, TNode>(headNodePtr);
    public UnmanagedLinkedListValueEnumerator<TValue, TNode> GetValueEnumerator()
        => new UnmanagedLinkedListValueEnumerator<TValue, TNode>(headNodePtr);
    
    IPointerEnumerator<TNode> IPointerEnumerable<TNode>.GetPointerEnumerator()
        => new UnmanagedLinkedListNodeEnumerator<TValue, TNode>(headNodePtr);
    public UnmanagedLinkedListNodeEnumerator<TValue, TNode> GetNodePointerEnumerator()
        => new UnmanagedLinkedListNodeEnumerator<TValue, TNode>(headNodePtr);
    
    public void Dispose()
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
                UnmanagedLinkedListNodeEnumerator<TValue, TNode> childListEnumerator 
                    = GetNodePointerEnumerator();

                childListEnumerator.MoveNext();
                TNode* curNodePtr = childListEnumerator.CurrentPtr;
                TNode* prevNodePtr;

                while (childListEnumerator.MoveNext())
                {
                    prevNodePtr = curNodePtr;

                    curNodePtr = childListEnumerator.CurrentPtr;

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

    #endregion
}