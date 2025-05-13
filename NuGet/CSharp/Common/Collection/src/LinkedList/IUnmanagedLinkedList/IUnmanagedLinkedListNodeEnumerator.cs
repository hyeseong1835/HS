using System.Collections;

namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IUnmanagedLinkedListNodeEnumerator<TNode>
    : IEnumerator<TNode>, 
      IPtrEnumerator<TNode>
    where TNode : unmanaged, IUnmanagedLinkedListNode<TNode>
{
    #region Field & Property

    object IEnumerator.Current => throw new NotImplementedException();

    TNode* HeadNodePtr { get; protected set; }

    TNode* CurrentNodePtr { get; }
    TNode IEnumerator<TNode>.Current => *CurrentNodePtr;
    TNode* IPtrEnumerator<TNode>.CurrentPtr => CurrentNodePtr;

    bool IsEnd { get; }

    #endregion

    #region Method

    /// <summary>
    /// HeadNodePtr를 재설정하고 Reset()을 호출합니다.
    /// </summary>
    /// <param name="headNodePtr"></param>
    public void Init(TNode* headNodePtr)
    {
        HeadNodePtr = headNodePtr;

        Reset();
    }

    #endregion
}