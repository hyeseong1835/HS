using System.Runtime.InteropServices;

namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public struct UnmanagedTree<T> : IUnmanagedTree<T, UnmanagedTreeNode<T>>, IDisposable
    where T : unmanaged
{
    public UnmanagedTreeNode<T> rootNode;
    public UnmanagedTreeNode<T> RootNode => rootNode;

    public UnmanagedTree()
    {
        this.rootNode = new UnmanagedTreeNode<T>(default(T));
    }

    public UnmanagedTreeNodePtrEnumerator<T> GetNodePtrEnumerator()
        => new UnmanagedTreeNodePtrEnumerator<T>(rootNode);
    public UnmanagedTreeNodeEnumerator<T> GetNodeEnumerator()
        => new UnmanagedTreeNodeEnumerator<T>(rootNode);

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => new UnmanagedTreeValueEnumerator<T>(rootNode);
    public UnmanagedTreeValueEnumerator<T> GetValueEnumerator()
        => new UnmanagedTreeValueEnumerator<T>(rootNode);
    
    public void Dispose()
    {
        // 열거자 생성
        UnmanagedTreeNodeEnumerator<T> enumerator = GetNodeEnumerator();

        // 현재 노드 포인터
        if (enumerator.MoveNext() == false) 
        {
            rootNode = new UnmanagedTreeNode<T>();
            return;
        }
        UnmanagedTreeNode<T>* nodePtr = enumerator.CurrentPtr;

        // 다음 노드가 없을 때까지 열거
        UnmanagedTreeNode<T>* tempNodePtr;
        while (enumerator.MoveNext())
        {
            // 현재 노드 포인터 임시 저장
            tempNodePtr = nodePtr;

            nodePtr = enumerator.CurrentPtr;

            Marshal.FreeHGlobal((IntPtr)tempNodePtr);
        }

        // 다음 노드가 없다면 현재 노드를 해제하고 종료
        Marshal.FreeHGlobal((IntPtr)nodePtr);
        return;
    }
}