using System.Runtime.InteropServices;

namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IDefaultUnmanagedLinkedList<TValue, TNode>
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

    #region Property

    TNode* IUnmanagedLinkedList<TValue, TNode>.HeadNodePtr => HeadNodePtr;
    new TNode* HeadNodePtr { get; set; }

    TNode* IUnmanagedLinkedList<TValue, TNode>.TailNodePtr => TailNodePtr;
    new TNode* TailNodePtr { get; set; }

    int IReadOnlyUnmanagedLinkedList<TValue, TNode>.Count => Count;
    new int Count { get; set; }

    #endregion


    #region Method

    public void AddFirst(TValue value)
    {
        // 노드 생성 (new -> head)
        TNode* newNodePtr = CreateNode(value, HeadNodePtr);

        // 시작 노드 재설정
        HeadNodePtr = newNodePtr;

        if (Count == 0) 
        {
            TailNodePtr = newNodePtr;
        }
        
        Count++;
    }
    
    public void AddLast(TValue value)
    {
        // 노드 생성
        TNode* newNodePtr = CreateNode(value);

        // (tail -> new)
        TailNodePtr->NextNodePtr = newNodePtr;

        // 마지막 노드 재설정
        TailNodePtr = newNodePtr;

        if (Count == 0)
        {
            HeadNodePtr = newNodePtr;
        }

        Count++;
    }
    
    public void InsertNextTo(TNode* prevNodePtr, TNode newNode)
    {
        if (prevNodePtr == null)
            throw new InvalidOperationException("이전 노드 포인터가 null입니다.");

        // 노드 생성
        TNode* newNodePtr = CreateNode();
        *newNodePtr = newNode;

        prevNodePtr->NextNodePtr = newNodePtr;

        Count++;
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

        Count++;
    }
    
    public void RemoveFirst()
    {
        if (HeadNodePtr == null)
            throw new InvalidOperationException("리스트가 비어 있습니다.");

        TNode* secondNodePtr = HeadNodePtr->NextNodePtr;

        Marshal.FreeHGlobal((IntPtr)HeadNodePtr);

        HeadNodePtr = secondNodePtr;
    }

    public void Link<TList>(TList list)
        where TList : IUnmanagedLinkedList<TValue, TNode>
    {
        if (list.IsEmpty)
            return;

        if (TailNodePtr == null)
        {
            HeadNodePtr = list.HeadNodePtr;
            TailNodePtr = list.TailNodePtr;
        }
        else
        {
            TailNodePtr->NextNodePtr = list.HeadNodePtr;
            TailNodePtr = list.TailNodePtr;
        }
        
        Count += list.Count;
    }
    
    UnmanagedLinkedListValueEnumerator<TValue, TNode> IExplicitEnumerable<TValue, UnmanagedLinkedListValueEnumerator<TValue, TNode>>.GetEnumerator()
        => new UnmanagedLinkedListValueEnumerator<TValue, TNode>(HeadNodePtr);
    public UnmanagedLinkedListValueEnumerator<TValue, TNode> GetValueEnumerator()
        => new UnmanagedLinkedListValueEnumerator<TValue, TNode>(HeadNodePtr);
    
    IPointerEnumerator<TNode> IPointerEnumerable<TNode>.GetPointerEnumerator()
        => new UnmanagedLinkedListNodeEnumerator<TValue, TNode>(HeadNodePtr);
    public UnmanagedLinkedListNodeEnumerator<TValue, TNode> GetNodePointerEnumerator()
        => new UnmanagedLinkedListNodeEnumerator<TValue, TNode>(HeadNodePtr);
    
    public void Dispose()
    {
        switch (Count)
        {
            case 0: break;

            case 1: {
                Marshal.FreeHGlobal((IntPtr)HeadNodePtr);
                break;
            }
                
            case 2: {
                Marshal.FreeHGlobal((IntPtr)HeadNodePtr);
                Marshal.FreeHGlobal((IntPtr)TailNodePtr);
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
        
        HeadNodePtr = null;
        TailNodePtr = null;
        Count = 0;
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
unsafe public struct UnmanagedLinkedList<TValue>
    : IDefaultUnmanagedLinkedList<TValue, UnmanagedLinkedListNode<TValue>>
    where TValue : unmanaged
{
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
    public int Count {
        get => count;
        set => count = value;
    }

    #endregion


    #region Constructor

    public UnmanagedLinkedList()
    {
        this.headNodePtr = null;
        this.count = 0;
    }

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
    : IDefaultUnmanagedLinkedList<TValue, TNode>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedLinkedListNode<TValue, TNode>
{
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
    public int Count {
        get => count;
        set => count = value;
    }

    #endregion


    #region Constructor

    public UnmanagedLinkedList()
    {
        this.headNodePtr = null;
        this.count = 0;
    }

    #endregion
}