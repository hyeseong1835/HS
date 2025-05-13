using System.Collections;

namespace HS.CSharp.Common.Collection.Unmanaged;

unsafe public interface IUnmanagedLinkedListValueEnumerator<TValue, TNode>
    : IEnumerator<TValue>
    where TValue : unmanaged
    where TNode : unmanaged, IUnmanagedValueLinkedListNode<TValue, TNode>
{
    #region Field & Property

    object IEnumerator.Current => throw new NotImplementedException();

    TNode* HeadNodePtr { get; protected set; }
    TNode* CurrentNodePtr { get; }

    public TValue CurrentValue => CurrentNodePtr->Value;
    TValue IEnumerator<TValue>.Current => CurrentValue;

    bool IsEnd { get; }

    #endregion

    #region Method

    /// <summary>
    /// HeadNode를 재설정하고 Reset()을 호출합니다.
    /// </summary>
    /// <param name="headNodePtr"></param>
    public void Init(TNode* headNodePtr)
    {
        HeadNodePtr = headNodePtr;

        Reset();
    }

    #endregion
}